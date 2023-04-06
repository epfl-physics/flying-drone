using UnityEngine;

public class Vector : MonoBehaviour
{
    public LineRenderer body;
    public ConeMesh head;
    public Material defaultMaterial;

    public Vector3 components;
    public Color color = Color.black;
    [Min(0)] public float lineWidth = 0.2f;

    private float currentHeadRadius;
    private float currentHeadHeight;
    private const float headAngle = 30;

    public virtual void OnEnable()
    {
        Redraw();

        // Make sure colors and materials are synchronized
        SetColor();
    }

    public virtual void Redraw()
    {
        Vector3 direction = components.normalized;
        float magnitude = components.magnitude;

        // Prevent line width from exceeding 1/3 of the magnitude
        float trueWidth = Mathf.Min(magnitude / 3, lineWidth);
        float headRadius = trueWidth;
        float headHeight = headRadius / Mathf.Tan(Mathf.Deg2Rad * headAngle);

        if (head)
        {
            head.radius = 1.2f * headRadius;
            head.height = headHeight;
            head.transform.localPosition = components - headHeight * direction;
            if (direction != Vector3.zero)
            {
                head.transform.localRotation = Quaternion.LookRotation(direction);
            }

            // Redraw the head mesh if necessary
            if (currentHeadRadius != headRadius || currentHeadHeight != headHeight)
            {
                currentHeadRadius = headRadius;
                currentHeadHeight = headHeight;
                head.Redraw();
            }
        }

        if (body)
        {
            body.startWidth = trueWidth;
            body.endWidth = trueWidth;
            body.SetPositions(new Vector3[] { Vector3.zero, components - headHeight * direction });
        }
    }

    public void RedrawHead()
    {
        if (head) head.Redraw();
    }

    public void SetColor()
    {
        if (defaultMaterial)
        {
            Material material = new Material(defaultMaterial);
            material.color = color;
            material.name = "Copy of " + defaultMaterial;

            if (body) body.GetComponent<LineRenderer>().sharedMaterial = material;
            if (head) head.SetMaterial(material);
        }
    }
}
