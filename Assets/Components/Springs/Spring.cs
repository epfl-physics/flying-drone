using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Spring : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public enum Type { Simple, ZigZag, Coiled }
    public Type type = Type.Simple;
    [Min(0)] public int numCoils = 3;
    public Color color = Color.black;
    public float k = 1f;  // [N / m]
    public float height = 1f;  // [m]
    public float lineWidth = 0.5f;

    [Header("Endpoints (Object Space)")]
    public Vector3 point1 = Vector3.left;
    public Vector3 point2 = Vector3.right;

    public void SetEndpoints(Vector3 point1, Vector3 point2, bool redraw = true)
    {
        this.point1 = point1;
        this.point2 = point2;
        if (redraw) Redraw();
    }

    public void Redraw()
    {
        if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();

        // Update color
        // lineRenderer.startColor = color;
        // lineRenderer.endColor = color;
        lineRenderer.sharedMaterial.SetColor("_BaseColor", color);

        // Update line width
        // float width = 0.1f * Mathf.Log10(1 + Mathf.Max(1, k));
        float width = 0.1f * lineWidth;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        int numPositions;
        Vector3[] positions;
        Vector3 displacement = point2 - point1;
        float length = displacement.magnitude;
        float step;

        switch (type)
        {
            case Type.Simple:
                lineRenderer.positionCount = 2;
                lineRenderer.numCornerVertices = 0;
                lineRenderer.SetPositions(new Vector3[] { point1, point2 });
                break;
            case Type.ZigZag:
                numPositions = 4 + 2 * numCoils;
                int numSegments = numPositions - 2;
                lineRenderer.positionCount = numPositions;
                lineRenderer.numCornerVertices = 5;
                Vector3 xHat = displacement.normalized;
                Vector3 yHat = Vector3.Cross(Vector3.Cross(xHat, Vector3.up), xHat);
                step = length / numSegments;
                positions = new Vector3[numPositions];
                positions[0] = point1;
                positions[1] = point1 + 1 * step * xHat;
                for (int i = 0; i < numSegments; i++)
                {
                    float sign = (i % 2) == 0 ? 1 : -1;
                    positions[2 + i] = point1 + (1.5f + i) * step * xHat + sign * height * yHat;
                }
                positions[numPositions - 2] = point2 - step * xHat;
                positions[numPositions - 1] = point2;
                lineRenderer.SetPositions(positions);
                break;
            case Type.Coiled:
                // Set up a local coordinate system
                Vector3 e1 = displacement.normalized;  // Along the spring axis
                Vector3 e2 = (Mathf.Abs(Vector3.Dot(e1, Vector3.up)) == 1) ? Vector3.right : Vector3.Cross(Vector3.Cross(e1, Vector3.up), e1).normalized;
                Vector3 e3 = Vector3.Cross(e1, e2);

                int numStepsPerCoil = 60;
                float endLength = 0.1f * length;
                int totalSteps = Mathf.FloorToInt(numStepsPerCoil * (numCoils + 0.5f));
                step = Mathf.Max((length - 2 * endLength) / totalSteps, 0);

                numPositions = 4 + totalSteps;
                lineRenderer.positionCount = numPositions;
                positions = new Vector3[numPositions];
                positions[0] = point1;
                positions[1] = point1 + endLength * e1;

                for (int i = 0; i < totalSteps; i++)
                {
                    float angle = (i % numStepsPerCoil) * 2 * Mathf.PI / numStepsPerCoil;
                    positions[2 + i] = positions[1];
                    positions[2 + i] += height * (Mathf.Cos(angle) * e2 + Mathf.Sin(angle) * e3);
                    positions[2 + i] += i * step * e1;
                }
                positions[numPositions - 2] = point2 - endLength * e1;
                positions[numPositions - 1] = point2;

                // lineRenderer.SetPositions(new Vector3[] { point1, point2 });
                lineRenderer.SetPositions(positions);
                lineRenderer.numCornerVertices = 5;
                break;
        }
    }
}
