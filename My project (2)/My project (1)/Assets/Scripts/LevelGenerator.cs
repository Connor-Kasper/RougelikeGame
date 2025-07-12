using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    [Header("Tilemap things")]
    public Tilemap masterTilemap;
    public GameObject[] roomPrefabs;
    private GameObject startingRoom;
    public GameObject wideDoorPrefab;
    public GameObject singleDoorPrefab;

    [Header("Generation settings")]
    public int numOfRoomsTotal;
    public LayerMask roomBoundaryLayer;

    private List<GameObject> placedRooms = new List<GameObject>();
    private List<GameObject> openConnectors = new List<GameObject>();

    // Start is called before the first frame update
    public void GenerateLevel()
    {
        startingRoom = roomPrefabs[0];
        SetUpStartingRoom();

        int attempts = 0;
        int maxAttempts = 50;

        while (placedRooms.Count < numOfRoomsTotal && openConnectors.Count > 0 && attempts < maxAttempts)
        {
            if (TryPlaceRoom())
            {
                attempts = 0;
            }
            else
            {
                attempts++;
            }
        }
        if(placedRooms.Count < numOfRoomsTotal)
        {
            print("Generation finished, but not all rooms were placed. Total placed: " + placedRooms.Count);
        }
        else
        {
            print("LEVEL GEN COMPLETE");
        }
    }

    bool TryPlaceRoom()
    {
        GameObject curConnectorGO = openConnectors[Random.Range(0, openConnectors.Count)];
        DoorConnector curConnector = curConnectorGO.GetComponent<DoorConnector>();

        GameObject roomToPlacePrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
        GameObject roomInstance = Instantiate(roomToPlacePrefab);
        List<GameObject> newRoomConnectors = GetAllConnectors(roomInstance);

        DoorConnector newConnector = FindMatchingConnector(roomInstance, -curConnector.direction);

        if(newConnector == null)
        {
            Destroy(roomInstance);
            return false;
        }

        AlignRooms(roomInstance, newConnector, curConnector);

        if(CheckForOverlap(roomInstance))
        {
            Destroy(roomInstance);
            return false;
        }

        CommitRoom(roomInstance, curConnector.gameObject, newConnector.gameObject);
        return true;
    }

    DoorConnector FindMatchingConnector(GameObject roomInstance, Vector2Int dirToMatch)
    {
        DoorConnector[] allConnectors = roomInstance.GetComponentsInChildren<DoorConnector>();

        List<DoorConnector> matchingConnectors = allConnectors.Where(c => c.direction == dirToMatch).ToList();
        if(matchingConnectors.Count == 0) { return null; }

        return matchingConnectors[Random.Range(0, matchingConnectors.Count)];
    }

    void AlignRooms(GameObject roomInstance, DoorConnector newConnector, DoorConnector curConnector)
    {
        Vector3 positionOffset = curConnector.transform.position - newConnector.transform.position;
        roomInstance.transform.position += positionOffset;

        Vector3 pushDirection = (Vector3Int)curConnector.direction;
        roomInstance.transform.position += pushDirection * masterTilemap.cellSize.x;
    }

    bool CheckForOverlap(GameObject roomInstance)
    {
        Physics2D.SyncTransforms();

        Transform newBoundaryTransform = roomInstance.transform.Find("Boundary");
        Collider2D boundaryCollider = newBoundaryTransform.gameObject.GetComponent<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(roomBoundaryLayer);

        Collider2D[] results = new Collider2D[1];

        int overlapCount = boundaryCollider.Overlap(filter, results);
        return overlapCount > 0;
    }

    void CommitRoom(GameObject roomInstance, GameObject curConnector, GameObject newConnector)
    {
        placedRooms.Add(roomInstance);
        CopyRoomToMaster(roomInstance);

        Vector3 doorDirection = (Vector3Int)curConnector.GetComponent<DoorConnector>().direction;
        float tileSize = masterTilemap.cellSize.x;
        Vector3 idealPosition = curConnector.transform.position + (doorDirection * tileSize * 0.5f);

        Vector3Int doorCell = masterTilemap.WorldToCell(Vector3Int.FloorToInt(idealPosition));

        Vector3 doorPosition = masterTilemap.GetCellCenterWorld(doorCell);

        if (curConnector.GetComponent<DoorConnector>().type == ConnectorType.Vertical)
        {
            Instantiate(wideDoorPrefab, doorPosition, curConnector.transform.rotation);
        }
        else
        {
            Instantiate(singleDoorPrefab, doorPosition, curConnector.transform.rotation);
        }

        openConnectors.Remove(curConnector);
        List<GameObject> newConnectorsList = GetAllConnectors(roomInstance);
        newConnectorsList.Remove(newConnector);
        openConnectors.AddRange(newConnectorsList);

        Destroy(curConnector);
        Destroy(newConnector);
    }

    void SetUpStartingRoom()
    {
        //Set up the starter room
        GameObject roomInstance = Instantiate(startingRoom, new Vector3(1000.25f, 1000.25f, 0), transform.rotation);
        /*
        Tilemap roomTilemap = roomInstance.GetComponentInChildren<Tilemap>();
        roomTilemap.CompressBounds();
        Vector3 tileDataCenter = roomTilemap.transform.TransformPoint(roomTilemap.localBounds.center);
        Vector3 offsetToCenter = -tileDataCenter;
        roomInstance.transform.position += offsetToCenter;*/


        placedRooms.Add(roomInstance);

        CopyRoomToMaster(roomInstance);
        openConnectors.AddRange(GetAllConnectors(roomInstance));
    }

    void CopyRoomToMaster(GameObject roomInstance)
    {
        print("copying...");
        //sets a new variable to the roomsTilemap and makes sure its there then continues
        Tilemap roomTilemap = roomInstance.GetComponentInChildren<Tilemap>();
        if (roomTilemap == null) { print("returning..."); return; }

        roomTilemap.CompressBounds();
        BoundsInt localBounds = roomTilemap.cellBounds;
        TileBase[] allTiles = roomTilemap.GetTilesBlock(localBounds);

        /*
        Vector3Int instanceCellPos = masterTilemap.WorldToCell(roomInstance.transform.position);
        Vector3Int offset = instanceCellPos - localBounds.position;
        BoundsInt newBounds = new BoundsInt(localBounds.position + offset, localBounds.size);

        masterTilemap.SetTilesBlock(newBounds, allTiles);
        */
        /*
        Vector3 worldPosOfMinCorner = roomTilemap.transform.TransformPoint(roomTilemap.CellToLocal(localBounds.min));

        Vector3Int masterPastePos = masterTilemap.WorldToCell(worldPosOfMinCorner);

        BoundsInt bounds = new BoundsInt(masterPastePos, localBounds.size);

        masterTilemap.SetTilesBlock(bounds, allTiles);
        */
       
        foreach(var pos in localBounds.allPositionsWithin)
        {
            if(roomTilemap.HasTile(pos))
            {
                TileBase tile = roomTilemap.GetTile(pos);
                Vector3 localTilePos = roomTilemap.GetCellCenterWorld(pos);
                Vector3 offsetFromParent = localTilePos - roomTilemap.transform.position;
                Vector3 worldPos = roomInstance.transform.position + offsetFromParent;
                Vector3Int masterCellPos = masterTilemap.WorldToCell(worldPos);
                masterTilemap.SetTile(masterCellPos, tile);
            }
        }

        Destroy(roomTilemap.GetComponent<TilemapRenderer>());
        Destroy(roomTilemap);
    }

    List<GameObject> GetAllConnectors(GameObject roomInstance)
    {
        List<GameObject> connectors = new List<GameObject>();
        DoorConnector[] connectorScripts = roomInstance.GetComponentsInChildren<DoorConnector>();
        foreach (var script in connectorScripts)
        {
            connectors.Add(script.gameObject);
        }
        return connectors;
    }

    private void OnDrawGizmos()
    {
        if (placedRooms == null) return;
        Gizmos.color = Color.yellow;
        foreach (var room in placedRooms)
        {
            // Get the boundary from the child object for accurate Gizmo drawing.
            Transform boundaryTransform = room.transform.Find("Boundary");
            if (boundaryTransform != null)
            {
                Gizmos.DrawWireCube(boundaryTransform.GetComponent<Collider2D>().bounds.center, boundaryTransform.GetComponent<Collider2D>().bounds.size);
            }
        }
    }
}
