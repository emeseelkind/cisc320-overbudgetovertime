using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public delegate void calculateBridgeStrength();
    public static event calculateBridgeStrength totalStrengthChanged;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            TriggerEvent();
        }
    }

    public static void TriggerEvent()
    {
        totalStrengthChanged?.Invoke();
    }
}
