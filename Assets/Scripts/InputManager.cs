using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventHandler.TriggerGravity();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EventHandler.TriggerDelete();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EventHandler.TriggerMaterialChange(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EventHandler.TriggerMaterialChange(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EventHandler.TriggerMaterialChange(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            EventHandler.TriggerMaterialChange(4);
        }
    }
}
