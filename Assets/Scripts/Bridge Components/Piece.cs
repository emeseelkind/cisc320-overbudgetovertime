using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameObject anchor1;
    public GameObject anchor2;

    public float loadTaken;
    private double appliedForce;

    private float materialStrength = 1;

    public bool playPhase = false;
    public bool takingLoad = false;
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
            //calculateLoad();
        }
    }

    void calculateLoad(float appliedLoad)
    {
        float tempLoad = appliedLoad;

        bool anchor1IsHard = anchor1.GetComponent<Anchor>().isHard;
        bool anchor2IsHard = anchor2.GetComponent<Anchor>().isHard;
        if (anchor1IsHard)
        {
            tempLoad = tempLoad - (tempLoad * 0.3f);
        }
        if(anchor2IsHard)
        {
            tempLoad = tempLoad - (tempLoad * 0.3f);
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

    private double detectForce(double mass, double v)
    {
        appliedForce = mass + (mass * v);
        return appliedForce;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        double mass = collision.gameObject.GetComponent<Rigidbody2D>().mass;
        double velocity = Mathf.Abs(collision.gameObject.GetComponent<Rigidbody2D>().velocity.y);
        double appliedForce = detectForce(mass, velocity);
        calculateLoad((float)appliedForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        takingLoad = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        takingLoad = false;
    }

    public void setDisperseLoad(float dispersion)
    {
        calculateLoad(dispersion);
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
