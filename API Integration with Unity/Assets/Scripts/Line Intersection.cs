using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LineIntersection : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private List<Vector2> points = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            points.Clear();
            lineRenderer.positionCount = 0;

        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(points.Count == 0 || Vector2.Distance(points[points.Count - 1], mousepos) > 0.1f)
            {
                points.Add(mousepos);
                lineRenderer.positionCount = points.Count;
                lineRenderer.SetPosition(points.Count - 1, mousepos);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            DetectIntersection();
        }
        
    }

    private void DetectIntersection()
    {
        spawnCircles spawner = FindObjectOfType<spawnCircles>();
        if (spawner == null) return;
        List<GameObject> circles = spawner.GetSpawnCircle();

        for (int i = circles.Count - 1; i >= 0; i--) // Iterate in reverse to avoid index shifting
        {
            GameObject circle = circles[i];

            if (circle == null) continue; // Skip null or destroyed objects

            if (IsCircleIntersected(circle))
            {
                spawner.RemoveCircle(circle); // Remove from the list
                Destroy(circle); // Destroy the GameObject
            }
        }


    }

    private bool IsCircleIntersected(GameObject circle)
    {
        Vector3 circlepos = circle.transform.position;
        float radius = circle.GetComponent<CircleCollider2D>().radius;

        for(int i = 0; i < points.Count - 1; i++)
        {
            Vector2 p1 = points[i];
            Vector2 p2 = points[i + 1];

            if(isLineIntersected(p1, p2, circlepos, radius))
            {
                return true;
            }
        }
        return false;
    }

    private bool isLineIntersected(Vector2 p1, Vector2 p2, Vector2 centre, float radius)
    {
        Vector2 d = p2 - p1;
        Vector2 f = p1 - centre;

        float a = Vector3.Dot(d, d);
        float b = Vector3.Dot(f, d);
        float c = Vector3.Dot(f, f) - radius*radius;

        float discriminant = b * b - 4 * a * c;

        return discriminant >= 0;
    }
}
