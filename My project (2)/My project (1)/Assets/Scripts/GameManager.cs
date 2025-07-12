using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static int enemyCount;
    public GameObject enemy;
    public GameObject LevelGen;
    private LevelGenerator LevelGeneratorScript;

    public GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        enemyCount = 0;
        LevelGeneratorScript = LevelGen.GetComponent<LevelGenerator>();
        LevelGeneratorScript.GenerateLevel();
        StartCoroutine(SpawnPlayerAfterDelay(0.5f));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnEnemies(Vector3 spawnLoc)
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(enemy, spawnLoc, transform.rotation);
            enemyCount++;
        }
    }
    IEnumerator SpawnPlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(player, new Vector3 (1000, 1000, 0), transform.rotation);
    }
}
