using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float velocityx = Input.GetAxis("Horizontal");
        float velocityy = Input.GetAxis("Vertical");
        rb.linearVelocity = new Vector2(velocityx * speed, velocityy * speed);
    }
}
