using System.Collections;
using System.Collections.Generic;
using System.Data;
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

    public LayerMask anchorLayer;
    public LayerMask bridgeLayer;

    public bool deleting = false;



    // Start is called before the first frame update
    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (!deleting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startDrawing();
            }

            if (Input.GetMouseButtonUp(0) && drawing == true)
            {
                stopDrawing(mouseWorldPos);
            }

            if (drawing == true)
            {
                mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0f;
                drawMaterial(mouseWorldPos);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                delete();
            }
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

        if (drawEndPoint != originAnchor.transform.position)
        {
            if (hit.collider == null)
            {
                build(null);
            }
            else if (hit.collider.gameObject.tag == "Anchor")
            {
                build(hit.collider.gameObject);
            }
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

        HingeJoint2D[] hingeJoints = piece.GetComponents<HingeJoint2D>();
        foreach (HingeJoint2D hinge in hingeJoints)
        {
            if (hinge.anchor == new Vector2(-0.5f, 0))
            {
                hinge.connectedBody = originAnchor.GetComponent<Rigidbody2D>();
                break;
            }
        }

        if (endAnchor == null) 
        {
            GameObject anchor = Instantiate(anchorPiece, drawEndPoint, Quaternion.identity);

            anchor.GetComponent<Anchor>().addPiece(piece);
            piece.GetComponent<Piece>().anchor1 = anchor;

            foreach (HingeJoint2D hinge in hingeJoints)
            {
                if (hinge.anchor == new Vector2(0.5f, 0))
                {
                    hinge.connectedBody = anchor.GetComponent<Rigidbody2D>();
                    break;
                }
            }
        }
        else
        {
            endAnchor.GetComponent<Anchor>().addPiece(piece);
            piece.GetComponent<Piece>().anchor1 = endAnchor;

            foreach (HingeJoint2D hinge in hingeJoints)
            {
                if (hinge.anchor == new Vector2(0.5f, 0))
                {
                    hinge.connectedBody = endAnchor.GetComponent<Rigidbody2D>();
                    break;
                }
            }
        }
    }

    void delete()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, bridgeLayer);

        if (hit.collider == null)
        {
            return;
        }
        else if (hit.collider.gameObject.tag == "Bridge Material")
        {
            Debug.Log("Deleting piece");
            GameObject piece = hit.collider.gameObject;

            GameObject anchor1 = piece.GetComponent<Piece>().anchor1;
            GameObject anchor2 = piece.GetComponent<Piece>().anchor2;

            anchor1.GetComponent<Anchor>().deletePiece(piece);
            anchor2.GetComponent<Anchor>().deletePiece(piece);

            Destroy(piece);
        }
    }
}
