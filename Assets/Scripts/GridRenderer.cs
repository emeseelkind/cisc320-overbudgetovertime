using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GridRenderer : MonoBehaviour
{
    public int gridHeight = 4;
    public int gridWidth = 4;

    public Material material;
    // Start is called before the first frame update
    void Start()
    {
        float gbX = gameObject.transform.position.x;
        float gbY = gameObject.transform.position.y;

        for (int x = -gridHeight; x <= gridHeight; x++)
        {
            drawLine(new Vector3(gbX + x, gbY - gridHeight, 0), new Vector3(gbX + x, gbY + gridHeight, 0));
        }

        for (int y = -gridWidth; y <= gridWidth; y++)
        {
            drawLine(new Vector3(gbX -gridWidth, gbY + y, 0), new Vector3(gbX + gridWidth, gbY + y, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void drawLine(Vector3 start, Vector3 end)
    {
        GameObject line = new ("GridLine");
        line.transform.SetParent(gameObject.transform);    
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        // Set the material and appearance of the line
        lineRenderer.material = material;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.useWorldSpace = true;
    }
}
