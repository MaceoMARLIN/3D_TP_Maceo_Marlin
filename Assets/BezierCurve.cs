using UnityEngine;

[ExecuteAlways] // permet de mettre à jour la courbe dans l'Editor
public class BezierCurve : MonoBehaviour
{
    public Transform point0;
    public Transform point1;
    public Transform point2;
    public Transform point3;

    public bool cubic = false;
    public int resolution = 50;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.positionCount = resolution + 1;
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.1f;
    }

    void Update()
    {
        DrawCurve();
    }

    void DrawCurve()
    {
        if (!cubic)
        {
            for (int i = 0; i <= resolution; i++)
            {
                float t = i / (float)resolution;
                Vector3 position = QuadraticBezier(point0.position, point1.position, point2.position, t);
                lineRenderer.SetPosition(i, position);
            }
        }
        else
        {
            for (int i = 0; i <= resolution; i++)
            {
                float t = i / (float)resolution;
                Vector3 position = CubicBezier(point0.position, point1.position, point2.position, point3.position, t);
                lineRenderer.SetPosition(i, position);
            }
        }
    }

    Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        return u * u * p0 + 2 * u * t * p1 + t * t * p2;
    }

    Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        return u * u * u * p0 +
               3 * u * u * t * p1 +
               3 * u * t * t * p2 +
               t * t * t * p3;
    }
}