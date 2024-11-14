using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Build : MonoBehaviour
{
    private bool drawing = false;

    Vector3 mouseWorldPos;
    private Transform drawStartPoint;
    private Vector3 drawEndPoint;

    private LineRenderer lr;

    public GameObject originAnchor;

    public GameObject anchorPiece;
    public GameObject bridgePiece;
    public GameObject connectingEnd;

    public LayerMask anchorLayer;



    // Start is called before the first frame update
    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // Update is called once per frame
    void Update()
    {
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
            mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
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
            originAnchor = hit.collider.gameObject;
        }
    }

    void stopDrawing(Vector3 mousePos)
    {
        drawEndPoint = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), 0);

        drawing = false;
        lr.positionCount = 0;

        RaycastHit2D hit = Physics2D.Raycast(drawEndPoint, Vector2.zero, Mathf.Infinity, anchorLayer);

        if (hit.collider == null)
        {
            build(null);
        }
        else if (hit.collider.gameObject.tag == "Anchor")
        {
            build(hit.collider.gameObject);
        }    
    }

    void drawMaterial(Vector3 mousePos)
    {
        Vector3 roundedVector = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), 0);
        lr.SetPosition(1, roundedVector);
    }

    void build(GameObject endAnchor)
    {
        
        Vector3 pieceVector = drawEndPoint - originAnchor.transform.position;
        float pieceLength = pieceVector.magnitude;
        Vector3 pieceLoaction = pieceVector / 2 + originAnchor.transform.position;

        
        float angle = Vector2.SignedAngle(Vector2.right, new Vector2(pieceVector.x, pieceVector.y));
        GameObject piece = Instantiate(bridgePiece, pieceLoaction, Quaternion.Euler(0, 0, angle));

        Vector3 adjustedPieceScale = piece.transform.localScale;
        adjustedPieceScale.x *= pieceLength;
        piece.transform.localScale = adjustedPieceScale;

        originAnchor.GetComponent<Anchor>().addPiece(piece);
        piece.GetComponent<Piece>().anchor2 = originAnchor;
        piece.GetComponent<HingeJoint2D>().connectedBody = originAnchor.GetComponent<Rigidbody2D>();
        

        if (endAnchor == null) 
        {
            GameObject anchor = Instantiate(anchorPiece, drawEndPoint, Quaternion.identity);

            anchor.GetComponent<Anchor>().addPiece(piece);
            piece.GetComponent<Piece>().anchor1 = anchor;
            
            anchor.GetComponent<HingeJoint2D>().connectedBody = piece.GetComponent<Rigidbody2D>();
        }
        else
        {
            GameObject endConnector = Instantiate(connectingEnd, Vector3.zero, Quaternion.identity, endAnchor.transform);

            endAnchor.GetComponent<Anchor>().addPiece(piece);
            piece.GetComponent<Piece>().anchor1 = endAnchor;

            endConnector.GetComponent<HingeJoint2D>().connectedBody = piece.GetComponent<Rigidbody2D>();

        }
        //Need to figure out multiple pieces on a joint, and connecting two ends 
        //bug: can't draw to the left
    }
}
