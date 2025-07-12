using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cinemachine;

public class DoorOpener : MonoBehaviour
{
    public bool doorUnlocked;

    //Tile Stuff
    public Tilemap doorTilemap;

    private Vector3Int doorPos1;
    private Vector3Int doorPos2;

    public TileBase openDoorTileL;
    public TileBase closedDoorTileL;

    public TileBase openDoorTileR;
    public TileBase closedDoorTileR;

    // Start is called before the first frame update
    void Start()
    {
        doorTilemap = GameObject.Find("Door").GetComponent<Tilemap>();
        doorUnlocked = false;

        Vector3 leftSide = transform.position - (transform.right * 0.5f);
        Vector3 rightSide = transform.position - (transform.right * 1.5f);

        doorPos1 = doorTilemap.WorldToCell(rightSide);
        doorPos2 = doorTilemap.WorldToCell(leftSide);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.enemyCount == 0)
        {
            doorUnlocked = true;
        }
        else
        {
            doorUnlocked = false;
        }

        if (!doorUnlocked)
        {
            doorTilemap.SetTile(doorPos1, closedDoorTileL);
            doorTilemap.SetTile(doorPos2, closedDoorTileR);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (doorUnlocked)
        {
            doorTilemap.SetTile(doorPos1, openDoorTileL);
            doorTilemap.SetTile(doorPos2, openDoorTileR);
        }
    }
}
