using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.Sockets;
using System.Xml.Serialization;
using UnityEngine;

public class Build : MonoBehaviour
{
    private bool drawing = false;

    Vector3 mouseWorldPos;
    private Transform drawStartPoint;
    private Vector3 drawEndPoint;

    private LineRenderer lr;
    public float maxDrawLength;

    public GameObject originAnchor;

    public GameObject roadPiece;
    public GameObject woodPiece;
    public GameObject brickPiece;
    public GameObject metalPiece;

    public GameObject anchorPiece;
    public GameObject bridgePiece;

    public LayerMask anchorLayer;
    public LayerMask bridgeLayer;

    public bool deleting = false;

    public float budget;

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
        Vector3 direction = mousePos - drawStartPoint.position;
        float length = direction.magnitude;

        if (length > maxDrawLength)
        {
            direction = direction.normalized * maxDrawLength;
        }

        Vector3 endPoint = drawStartPoint.position + direction;

        drawEndPoint = new Vector3(
            Mathf.Round(endPoint.x),
            Mathf.Round(endPoint.y),
            0
        );

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
        Vector3 direction = mousePos - drawStartPoint.position;
        float length = direction.magnitude;

        if (length > maxDrawLength)
        {
            direction = direction.normalized * maxDrawLength;
        }

        Vector3 endPoint = drawStartPoint.position + direction;

        Vector3 roundedEndPoint = new Vector3(
            Mathf.Round(endPoint.x),
            Mathf.Round(endPoint.y),
            0 
        );

        lr.SetPosition(1, roundedEndPoint);
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

        budget = budget - piece.GetComponent<Piece>().getCost();
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

            budget = budget + piece.GetComponent<Piece>().getCost();

            Destroy(piece);
        }
    }

    private void toggleDeleteMode()
    {
        if(!deleting)
        {
            deleting = true;
        }
        else
        {
            deleting = false;
        }
    }

    private void changeMaterial(int type)
    {
        if(type == 1)
        {
            bridgePiece = roadPiece;
        }
        else if(type == 2)
        {
            bridgePiece = woodPiece;
        }
        else if(type == 3)
        {
            bridgePiece = brickPiece;
        }
        else if(type == 4)
        {
            bridgePiece = metalPiece;
        }
    }

    void OnEnable()
    {
        EventHandler.toggleDelete += toggleDeleteMode;
        EventHandler.onMaterialChange += changeMaterial;
    }

    void OnDisable()
    {
        EventHandler.toggleDelete -= toggleDeleteMode;
        EventHandler.onMaterialChange -= changeMaterial;
    }
}
