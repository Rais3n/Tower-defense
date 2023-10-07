using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    private LineRenderer lineRenderer;
    private float drawingTime = .292f; //drawingTime + shootingTime should be equal to 4f, however visual logic isn't 100% accurate and it lasts a little longer than sum of these components
    private float shootingTime = .1f;
    private bool isDrawing;
    private BowVisual bowVisual;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        bowVisual = GetComponentInParent<BowVisual>();
        
    }

    public void DrawBow()
    {
        float offset;
        float drawnBow_localX = 0.25f;
        if (isDrawing)
        {
            offset = Time.deltaTime / drawingTime * drawnBow_localX;

        }
        else
        {
            offset = Time.deltaTime / shootingTime * drawnBow_localX * -1f;
            
        }

        int middlePoint = 1;
        points[middlePoint].localPosition = new Vector3(points[middlePoint].localPosition.x + offset, points[middlePoint].localPosition.y, 0);

        for (int i = 0; i < points.Length; i++)
        {
            lineRenderer.SetPosition(i, points[i].localPosition);
        }
        
        if (points[1].localPosition.x >= 0.25f)
        {
            isDrawing = false;
            bowVisual.OnFalseIsShooting();
        }

        if (points[1].localPosition.x < 0f) isDrawing = true;
    }

}
