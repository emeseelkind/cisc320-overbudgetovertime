using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{
    public bool isHard = false;
    public float startingStrength = 0;

    public List<GameObject> connectedPieces = new List<GameObject>();

    public float strength;
    public float strengthPurity;
    // Start is called before the first frame update
    void Start()
    {
        if(isHard)
        {
            strength = 100;
        }
        else
        {
            calculateConnectingStrength();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addPiece(GameObject newPiece)
    {
        connectedPieces.Add(newPiece);
    }

    public void deletePiece(GameObject piece)
    {
        connectedPieces.Remove(piece);
        if (connectedPieces.Count == 0)
        {
            Destroy(gameObject);
        }
    }

    void calculateConnectingStrength()
    {
        if (!isHard)
        {
            float temp = 0;
            foreach (GameObject piece in connectedPieces)
            {
                temp += piece.GetComponent<Piece>().strength;
            }
            strength = temp;
        }
    }

    void OnEnable()
    {
        EventHandler.totalStrengthChanged += calculateConnectingStrength;
    }

    void OnDisable()
    {
        EventHandler.totalStrengthChanged -= calculateConnectingStrength;
    }
}
