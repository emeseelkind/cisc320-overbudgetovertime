using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{
    public bool isHard = false;

    public List<GameObject> connectedPieces = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
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

    public float disperseLoad(GameObject originPiece, float valueDisperse)
    {
        float undispersedLoad = valueDisperse;
        float dispersionPerPiece = valueDisperse * 0.1f;
        if(connectedPieces.Count > 7)
        {
            dispersionPerPiece = (valueDisperse * 0.6f) / (connectedPieces.Count-1);
        }

        foreach(GameObject piece in connectedPieces)
        {
            if(piece.GetComponent<Piece>().takingLoad() == true)
            {
                piece.GetComponent<Piece>().setDisperseLoad(dispersionPerPiece);
                undispersedLoad -= dispersionPerPiece;
            }
        }

        return undispersedLoad;
    }
    
    /*
    void OnEnable()
    {
        EventHandler.totalStrengthChanged += calculateConnectingStrength;
    }

    void OnDisable()
    {
        EventHandler.totalStrengthChanged -= calculateConnectingStrength;
    }
    */
}
