using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject anchor1;
    public GameObject anchor2;

    public float loadTaken = 0;
    private double appliedForce;

    public float materialStrength;

    public bool playPhase = false;
    public bool takingLoad = false;

    public SpriteRenderer statusColor;

    public int numberFromSource = 100;

    public float costValue;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playPhase)
        {
            //calculateLoad();
        }

        if(loadTaken > materialStrength * (2f / 3f)){
            statusColor.color = new Color(1f, 0f, 0f, 120f / 255f);
        }
        else if(loadTaken > materialStrength * (1f / 10f))
        {
            statusColor.color = new Color(1f, 1f, 0f, 120f / 255f);
        }
        else 
        { 
            statusColor.color = new Color(0f, 0f, 0f, 0f);
        }

        if(loadTaken > materialStrength)
        {
            destroyPiece();
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
            loadTaken = anchor2.GetComponent<Anchor>().disperseLoad(gameObject, tempLoad, numberFromSource + 1);
        }
        else if(!anchor1IsHard && anchor2IsHard)
        {
            loadTaken = anchor1.GetComponent<Anchor>().disperseLoad(gameObject, tempLoad, numberFromSource + 1);
        }
        else if(!anchor1IsHard && !anchor2IsHard)
        {
            loadTaken = anchor1.GetComponent<Anchor>().disperseLoad(gameObject, tempLoad*0.5f, numberFromSource + 1) 
                + anchor2.GetComponent<Anchor>().disperseLoad(gameObject, tempLoad * 0.5f, numberFromSource + 1);           
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
        EventHandler.TriggerMapReset();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //takingLoad = true;
        numberFromSource = 0;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //takingLoad = false;
        loadTaken = 0;
        numberFromSource = 100;
    }

    private void destroyPiece()
    {
        anchor1.GetComponent<Anchor>().deletePiece(gameObject);
        anchor2.GetComponent<Anchor>().deletePiece(gameObject);

        Destroy(gameObject);
    }

    public void setDisperseLoad(float dispersion)
    {
        calculateLoad(dispersion);
    }

    private void setGravity()
    {
        rb.gravityScale = 1;
    }

    private void resetMapLevel()
    {
        if(numberFromSource == 0)
        {
            return;
        }
        else
        {
            numberFromSource = 100;
        }
    }

    void OnEnable()
    {
        EventHandler.addGravity += setGravity;
        EventHandler.resetMap += resetMapLevel;
    }

    void OnDisable()
    {
        EventHandler.addGravity -= setGravity;
        EventHandler.resetMap -= resetMapLevel;
    }

    public float getCost()
    {
        float cost = costValue * transform.localScale.x;

        return cost;
    }
    
}
