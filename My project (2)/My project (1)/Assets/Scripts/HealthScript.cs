using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public GameObject enemyParent;
    public float health;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            GameManager.enemyCount--;
            print(GameManager.enemyCount);
            Destroy(enemyParent);
        }
    }
}
