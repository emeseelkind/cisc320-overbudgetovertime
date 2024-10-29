using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Build : MonoBehaviour
{
    private bool drawing = false;

    private Transform drawStartPoint;
    private Vector3 drawEndPoint;

    private LineRenderer lr;

    public GameObject originAnchor;

    public GameObject anchorPiece;
    public GameObject bridgePiece;

    public LayerMask anchorLayer;



    // Start is called before the first frame update
    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        if (Input.GetMouseButtonDown(0))
        {
            startDrawing();
        }

        if (Input.GetMouseButtonUp(0) && drawing == true)
        {
            stopDrawing(mouseWorldPos);
        }

        if(drawing == true)
        {
            drawMaterial(mouseWorldPos);
        }
    }

    void startDrawing()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, anchorLayer);

        if (hit.collider == null)
        {
            return;
        }
        else if (hit.collider.gameObject.tag == "Anchor")
        {
            drawing = true;
            drawStartPoint = hit.collider.gameObject.transform;

            lr.positionCount = 2;
            lr.SetPosition(0, drawStartPoint.position);
            Debug.Log("CLICKED " + hit.collider.name);
            originAnchor = hit.collider.gameObject;
        }
    }

    void stopDrawing(Vector3 mousePos)
    {
        drawEndPoint = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), 0);

        drawing = false;
        lr.positionCount = 0;

        build();
    }

    void drawMaterial(Vector3 mousePos)
    {
        Vector3 roundedVector = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), 0);
        lr.SetPosition(1, roundedVector);
    }

    void build()
    {
        Vector3 pieceVector = drawEndPoint - originAnchor.transform.position;
        Debug.Log("pieceVector:" + pieceVector);
        float pieceLength = pieceVector.magnitude;
        Debug.Log("piece length: " + pieceLength);
        Vector3 pieceLoaction = pieceVector / 2 + originAnchor.transform.position;
        Debug.Log("piece location: " + pieceLoaction);

        GameObject anchor = Instantiate(anchorPiece, drawEndPoint, Quaternion.identity);
        GameObject piece = Instantiate(bridgePiece, pieceLoaction, Quaternion.FromToRotation(Vector3.right, pieceVector.normalized));

        Vector3 adjustedPieceScale = piece.transform.localScale;
        adjustedPieceScale.x *= pieceLength;
        piece.transform.localScale = adjustedPieceScale;

        originAnchor.GetComponent<Anchor>().addPiece(piece);
        anchor.GetComponent<Anchor>().addPiece(piece);
        piece.GetComponent<Piece>().anchor1 = anchor;
        piece.GetComponent<Piece>().anchor2 = originAnchor;

        
        piece.GetComponent<HingeJoint2D>().connectedBody = originAnchor.GetComponent<Rigidbody2D>();
        anchor.GetComponent<HingeJoint2D>().connectedBody = piece.GetComponent<Rigidbody2D>();

        //Need to figure out multiple pieces on a joint, and connecting two ends 
        //bug: can't draw to the left
    }
}
