using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinePlot : MonoBehaviour
{
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private GameObject lineSegmentPrefab;

    public Color color = Color.black;
    public float lineWidth = 1f;
    public bool showMarkers = true;
    public float markerSize = 0.2f;

    public List<Vector2> points;

    private RectTransform markerContainer;
    private RectTransform lineContainer;

    private void OnEnable()
    {
        points = new List<Vector2>();

        if (!markerContainer) markerContainer = CreateContainer("Markers");
        if (!lineContainer) lineContainer = CreateContainer("Lines");
    }

    private void OnDisable()
    {
        Clear();
    }

    public void PlotPoint(Vector2 position, bool connected)
    {
        // Only plot unique points
        if (points.Contains(position)) return;

        // If the x value of the position to add is the same as the previous value,
        // move the previous value instead of adding a new point
        if (points.Count > 0)
        {
            if (points[points.Count - 1].x == position.x)
            {
                // Update the list value
                points[points.Count - 1] = position;
                // Update the object position
                (markerContainer.GetChild(markerContainer.childCount - 1) as RectTransform).anchoredPosition = position;
                return;
            }
        }

        // Append to the list of points
        points.Add(position);

        // Handle drawing (or moving) of the marker for this position
        if (showMarkers)
        {
            DrawPointMarker(position, markerContainer);
        }
        else
        {
            // Draw the first and only marker
            if (points.Count == 1)
            {
                DrawPointMarker(position, markerContainer);
            }
            else
            {
                (markerContainer.GetChild(0) as RectTransform).anchoredPosition = position;
            }
        }

        if (connected && points.Count > 1)
        {
            DrawLineSegment(points[points.Count - 2], position, lineContainer);
        }
    }

    public void ShiftPoints(Vector2 delta, float xMin)
    {
        int numPointsToRemove = 0;
        for (int i = 0; i < points.Count; i++)
        {
            points[i] += delta;

            if (i < markerContainer.childCount)
            {
                (markerContainer.GetChild(i) as RectTransform).anchoredPosition += delta;
            }

            if (i < lineContainer.childCount)
            {
                (lineContainer.GetChild(i) as RectTransform).anchoredPosition += delta;
            }

            if (points[i].x < xMin) numPointsToRemove++;
        }

        points.RemoveRange(0, numPointsToRemove);

        for (int i = 0; i < numPointsToRemove; i++)
        {
            if (showMarkers) DestroyImmediate(markerContainer.GetChild(0).gameObject);
            DestroyImmediate(lineContainer.GetChild(0).gameObject);
        }
    }

    public void Clear()
    {
        for (int i = markerContainer.childCount; i > 0; i--)
        {
            DestroyImmediate(markerContainer.GetChild(0).gameObject);
        }

        for (int i = lineContainer.childCount; i > 0; i--)
        {
            DestroyImmediate(lineContainer.GetChild(0).gameObject);
        }

        points = new List<Vector2>();
    }

    private RectTransform CreateContainer(string name)
    {
        var container = new GameObject(name, typeof(RectTransform)).GetComponent<RectTransform>();
        container.SetParent(transform);
        container.anchorMin = Vector2.zero;
        container.anchorMax = Vector2.one;
        container.pivot = 0.5f * Vector2.one;
        container.offsetMin = container.offsetMax = Vector2.zero;
        container.localScale = Vector3.one;
        return container;
    }

    private void DrawPointMarker(Vector2 position, RectTransform parent)
    {
        if (!pointPrefab) return;

        Image point = Instantiate(pointPrefab, parent).GetComponent<Image>();
        point.rectTransform.anchoredPosition = position;
        point.rectTransform.localScale = markerSize * Vector3.one;
        point.color = color;
    }

    private void DrawLineSegment(Vector2 position1, Vector2 position2, RectTransform parent)
    {
        if (!lineSegmentPrefab) return;

        Image segment = Instantiate(lineSegmentPrefab, parent).GetComponent<Image>();
        Vector2 displacement = position2 - position1;
        float angle = Mathf.Atan2(displacement.y, displacement.x);
        segment.rectTransform.anchoredPosition = position1 + 0.5f * displacement;
        segment.rectTransform.sizeDelta = new Vector2(displacement.magnitude + 0.25f * lineWidth, lineWidth);
        segment.rectTransform.localEulerAngles = Mathf.Rad2Deg * angle * Vector3.forward;
        segment.color = color;
    }
}
