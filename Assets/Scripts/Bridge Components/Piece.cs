using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameObject anchor1;
    public GameObject anchor2;

    public float strengthTranser;
    public float strength;
    // Start is called before the first frame update
    void Start()
    {
        calculateStrength();
    }

    // Update is called once per frame
    void Update()
    {
        

        
    }

    void calculateStrength()
    {
        float strength1 = anchor1.GetComponent<Anchor>().strength;
        float strength2 = anchor2.GetComponent<Anchor>().strength;
        strength = (strength1 + strength2) * strengthTranser;
    }

    void OnEnable()
    {
        EventHandler.totalStrengthChanged += calculateStrength;
    }

    void OnDisable()
    {
        EventHandler.totalStrengthChanged -= calculateStrength;
    }
}
