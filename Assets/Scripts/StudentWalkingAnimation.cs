using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentWalkingAnimation : MonoBehaviour
{
    public float moveSpeed = 2f;  // Speed of movement
    public float targetX = 10f;  // Target X position

    void Start()
    {
        // Initialization logic (if needed)
    }

    void Update()
    {
        // Move the object towards the target X position
        if (transform.position.x < targetX)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }
}
