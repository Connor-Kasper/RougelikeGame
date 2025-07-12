using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGo : MonoBehaviour
{
    private Rigidbody2D rb;
    public float shootSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.Rotate(0, 0, 90);
        rb.AddForce(transform.right * shootSpeed);

        Destroy(gameObject, 5f);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            HealthScript otherHealth = collision.gameObject.GetComponent<HealthScript>();
            otherHealth.health -= 10;
        }
        if (collision.tag == "Enemy" || collision.tag == "Walls")
        {
            Destroy(gameObject);
        }

    }

}
