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
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

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
        }
    }

    void stopDrawing(Vector3 mousePos)
    {
        drawEndPoint = mousePos;

        drawing = false;
    }

    void drawMaterial(Vector3 mousePos)
    {
        lr.SetPosition(1, mousePos);
    }
}
