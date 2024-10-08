using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeDurability : MonoBehaviour
{
    private double forceApplied;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void detectForce(double mass, double v)
    {
        forceApplied = mass * v;
        Debug.Log(forceApplied);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        double mass = collision.gameObject.GetComponent<Rigidbody2D>().mass;
        double velocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity.y;
        detectForce(mass, velocity); 
    }
}
