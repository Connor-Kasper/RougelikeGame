using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SmallDoorOpener : MonoBehaviour
{
    public bool doorUnlocked;

    //Tile Stuff
    public Tilemap doorTilemap;

    private Vector3Int doorPos;

    public TileBase openDoorTile;
    public TileBase closedDoorTile;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.SyncTransforms();

        doorTilemap = GameObject.Find("Door").GetComponent<Tilemap>();
        doorUnlocked = false;
        transform.position += transform.up * -0.5f;

        Vector3 VecPos = transform.position - (transform.right * 1f);

        doorPos = doorTilemap.WorldToCell(Vector3Int.FloorToInt(transform.position));

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
            doorTilemap.SetTile(doorPos, closedDoorTile);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (doorUnlocked)
        {
            doorTilemap.SetTile(doorPos, openDoorTile);
        }
    }
}
