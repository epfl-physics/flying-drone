using UnityEngine;

public class Vector : MonoBehaviour
{
    public LineRenderer body;
    public ConeMesh head;
    public Material defaultMaterial;
    private Material currentMaterial;
    private int renderQueue = 3000;

    public Vector3 components;
    public Color color = Color.black;
    [Min(0)] public float lineWidth = 0.2f;
    public int sortOrder = 0;

    private float currentHeadRadius;
    private float currentHeadHeight;
    private const float headAngle = 30;

    public virtual void OnEnable()
    {
        // Make sure materials are synchronized
        UpdateCurrentMaterial();
        SetColor();
        SetSortOrder();

        Redraw();
    }

    public void OnDestroy()
    {
        currentMaterial = null;
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

    private void UpdateCurrentMaterial()
    {
        // Create a copy of the default material if available
        if (!currentMaterial && defaultMaterial)
        {
            currentMaterial = new Material(defaultMaterial);
            currentMaterial.name = "Copy of " + defaultMaterial;
            renderQueue = defaultMaterial.renderQueue;
        }

        if (!currentMaterial) return;

        // Assign the copied material to the body and head components
        if (body) body.GetComponent<LineRenderer>().sharedMaterial = currentMaterial;
        if (head) head.SetMaterial(currentMaterial);
    }

    public void SetColor()
    {
        // Make sure the current material is available
        UpdateCurrentMaterial();

        if (currentMaterial) currentMaterial.color = color;
    }

    public void SetSortOrder()
    {
        // Make sure the current material is available
        UpdateCurrentMaterial();

        if (currentMaterial) currentMaterial.renderQueue = renderQueue + sortOrder;
    }
}
