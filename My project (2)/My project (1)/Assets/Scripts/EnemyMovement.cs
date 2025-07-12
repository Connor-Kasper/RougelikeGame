using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float launchSpeed;
    public GameObject visual;
    // Start is called before the first frame update
    void Start()
    {

        transform.Rotate(0, 0, Random.Range(0f, 360f));
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * launchSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 curScale = visual.transform.localScale;

        if(rb.linearVelocity.x < 0f)
        {
            curScale.x = -1.8f;
        } else
        {
            curScale.x = 1.8f;
        }
        visual.transform.localScale = curScale;
    }
}
