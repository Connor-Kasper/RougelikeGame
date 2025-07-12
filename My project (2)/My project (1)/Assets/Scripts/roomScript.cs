using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class roomScript : MonoBehaviour
{
    public GameObject spawnLocObj;
    private Vector3 spawnLoc;
    public static int cinePriority;
    public bool hasEntered;
    private GameManager gameScript;

    public CinemachineVirtualCamera virtualCamera;
    // Start is called before the first frame update
    void Start()
    {
        cinePriority = 10;
        hasEntered = false;
        gameScript = GameManager.instance;
        spawnLoc = spawnLocObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            if(!hasEntered)
            {
                GameManager.instance.SpawnEnemies(spawnLoc);
            }
            hasEntered = true;
            cinePriority++;
            virtualCamera.Priority = cinePriority;
        }
    }
}
