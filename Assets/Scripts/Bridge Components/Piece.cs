using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameObject anchor1;
    public GameObject anchor2;

    public float loadTaken;
    private float tempLoad;
    private double appliedForce;

    private float materialStrength = 1;

    public bool playPhase = false;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playPhase)
        {
            calculateLoad();
        }
    }

    void calculateLoad()
    {
        if(tempLoad <= 0)
        {
            return;
        }

        bool anchor1IsHard = false;
        bool anchor2IsHard = false;
        if(anchor1.GetComponent<Anchor>().isHard)
        {
            tempLoad = tempLoad - (tempLoad * 0.2f);
        }
        if(anchor2.GetComponent<Anchor>().isHard)
        {
            tempLoad = tempLoad - (tempLoad * 0.2f);
        }

        if (anchor1IsHard && !anchor2IsHard)
        {
            loadTaken = anchor2.GetComponent<Anchor>().disperseLoad(gameObject, tempLoad);
        }
        else if(!anchor1IsHard && anchor2IsHard)
        {
            loadTaken = anchor1.GetComponent<Anchor>().disperseLoad(gameObject, tempLoad);
        }
        else if(!anchor1IsHard && !anchor2IsHard)
        {
            loadTaken = anchor1.GetComponent<Anchor>().disperseLoad(gameObject, tempLoad*0.5f) 
                + anchor2.GetComponent<Anchor>().disperseLoad(gameObject, tempLoad * 0.5f);           
        }
    }

    void detectForce(double mass, double v)
    {
        appliedForce = mass + (mass * v);
        tempLoad = (float)appliedForce;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        double mass = collision.gameObject.GetComponent<Rigidbody2D>().mass;
        double velocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity.y;
        detectForce(mass, velocity);
    }

    public void setDisperseLoad(float dispersion)
    {
        tempLoad = dispersion;
    }

    public bool takingLoad()
    {
        if (tempLoad <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    /*
    void OnEnable()
    {
        EventHandler.totalStrengthChanged += calculateStrength;
    }

    void OnDisable()
    {
        EventHandler.totalStrengthChanged -= calculateStrength;
    }
    */
}
