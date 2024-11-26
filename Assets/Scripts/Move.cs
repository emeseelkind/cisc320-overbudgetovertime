using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public bool moving = false;
    public float moveSpeed = 1;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            Vector2 fullVelocity = rb.velocity;
            fullVelocity.x = moveSpeed;
            rb.velocity = fullVelocity;
        }
    }

    private void startMoving()
    {
        moving = true;
    }

    void OnEnable()
    {
        EventHandler.addGravity += startMoving;
    }

    void OnDisable()
    {
        EventHandler.addGravity -= startMoving;
    }
}
