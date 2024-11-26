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
        if (connectedPieces.Count == 0 && isHard == false)
        {
            Destroy(gameObject);
        }
    }

    public float disperseLoad(GameObject originPiece, float valueDisperse, int level)
    {
        float undispersedLoad = valueDisperse;
        float dispersionPerPiece = valueDisperse * 0.25f;
        if(connectedPieces.Count > 4)
        {
            dispersionPerPiece = (valueDisperse * 0.75f) / (connectedPieces.Count-1);
        }

        foreach(GameObject piece in connectedPieces)
        {
            if(piece == originPiece)
            {
                continue;
            }

            if(piece.GetComponent<Piece>().numberFromSource > level)
            {
                //piece.GetComponent<Piece>().takingLoad = true;
                piece.GetComponent<Piece>().numberFromSource = level;
               piece.GetComponent<Piece>().setDisperseLoad(dispersionPerPiece);
                undispersedLoad -= dispersionPerPiece;
                //piece.GetComponent<Piece>().takingLoad = false;               
            }
        }
        return undispersedLoad;
    }
}
