using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO Make a parent class for this and CoordinateSpace2D
public class DynamicGraph : MonoBehaviour
{
    [Header("Plots")]
    [SerializeField] private GameObject linePlotPrefab;
    [SerializeField] private bool showMarkers = true;
    [SerializeField] private float markerSize = 0.1f;
    [SerializeField] private float lineWidth = 1f;

    [Header("Axes")]
    [SerializeField] private Image xAxis;
    [SerializeField] private Image yAxis;
    [SerializeField] private float axisLineWidth = 2;

    [Header("Extent")]
    [SerializeField] private Vector2 xRange = new Vector2(-1, 1);
    [SerializeField] private Vector2 yRange = new Vector2(-1, 1);
    private Vector2 xRangeInit;
    private Vector2 yRangeInit;

    [Header("Behavior")]
    [SerializeField] private bool rolling = false;
    [SerializeField] private float xMax;
    private float xMaxInit;

    private RectTransform rect;
    private List<LinePlot> lines;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        lines = new List<LinePlot>();

        xRangeInit = xRange;
        yRangeInit = yRange;
        xMaxInit = xMax;
    }

    private void OnValidate()
    {
        if (!rect) rect = GetComponent<RectTransform>();

        Vector2 pos = CoordinateToRectPosition(Vector2.zero);
        if (xAxis)
        {
            xAxis.rectTransform.sizeDelta = new Vector2(rect.rect.width, axisLineWidth);
            xAxis.rectTransform.anchoredPosition = pos.y * Vector2.up;
        }
        if (yAxis)
        {
            yAxis.rectTransform.sizeDelta = new Vector2(axisLineWidth, rect.rect.height);
            yAxis.rectTransform.anchoredPosition = pos.x * Vector2.right;
        }
    }

    public void CreateLine(Color color, string label = "")
    {
        if (!linePlotPrefab) return;

        var linePlot = Instantiate(linePlotPrefab, transform).GetComponent<LinePlot>();
        linePlot.color = color;
        linePlot.showMarkers = showMarkers;
        linePlot.lineWidth = lineWidth;
        linePlot.markerSize = markerSize;
        linePlot.name = "Line " + label;

        lines.Add(linePlot);
    }

    public void PlotPoint(int lineIndex, Vector2 position)
    {
        if (lineIndex < 0 || lineIndex >= lines.Count) return;
        if (!lines[lineIndex].gameObject.activeInHierarchy) return;
        if (position.x > xMax && !rolling) return;

        if (rolling && position.x > xMax)
        {
            float deltaX = position.x - xMax;
            xRange.x += deltaX;
            xRange.y += deltaX;
            Vector2 delta = CoordinateToRectDisplacement(deltaX * Vector2.right);
            foreach (LinePlot line in lines)
            {
                line.ShiftPoints(-delta, 0);
            }
            xMax += deltaX;
        }

        position = CoordinateToRectPosition(position);
        lines[lineIndex].PlotPoint(position, true);
    }

    public void Clear()
    {
        foreach (var line in lines)
        {
            line.Clear();
        }

        xRange = xRangeInit;
        yRange = yRangeInit;
        xMax = xMaxInit;
    }

    private Vector2 ScreenPositionToUV(Vector2 position, Camera camera)
    {
        if (!rect) rect = GetComponent<RectTransform>();

        Vector2 normalizedPosition = default;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, position, camera, out var localPosition))
        {
            normalizedPosition = Rect.PointToNormalized(rect.rect, localPosition);
        }

        return normalizedPosition;
    }

    public void SetLineVisibility(int lineIndex, bool visible)
    {
        if (lines == null) return;
        if (lineIndex < 0 || lineIndex >= lines.Count) return;

        lines[lineIndex].gameObject.SetActive(visible);
    }

    private Vector2 UVToCoordinatePosition(Vector2 uv)
    {
        float x = (xRange.y - xRange.x) * uv.x + xRange.x;
        float y = (yRange.y - yRange.x) * uv.y + yRange.x;
        return new Vector2(x, y);
    }

    private Vector2 CoordinatePositionToUV(Vector2 coordinatePosition)
    {
        float u = (coordinatePosition.x - xRange.x) / (xRange.y - xRange.x);
        float v = (coordinatePosition.y - yRange.x) / (yRange.y - yRange.x);
        return new Vector2(u, v);
    }

    private Vector2 CoordinateDisplacementToUV(Vector2 coordinateDisplacement)
    {
        float deltaU = coordinateDisplacement.x / (xRange.y - xRange.x);
        float deltaV = coordinateDisplacement.y / (yRange.y - yRange.x);
        return new Vector2(deltaU, deltaV);
    }

    private Vector2 UVToRectPosition(Vector2 uv)
    {
        if (!rect) rect = GetComponent<RectTransform>();

        return uv * rect.rect.size;
    }

    private Vector2 UVToRectDisplacement(Vector2 uvDisplacement)
    {
        if (!rect) rect = GetComponent<RectTransform>();

        return uvDisplacement * rect.rect.size;
    }

    private Vector2 RectPositionToUV(Vector2 rectPosition)
    {
        if (!rect) rect = GetComponent<RectTransform>();

        return (rectPosition / rect.rect.size) + 0.5f * Vector2.one;
    }

    private Vector2 CoordinateToRectPosition(Vector2 coordinatePosition)
    {
        Vector2 uv = CoordinatePositionToUV(coordinatePosition);
        return UVToRectPosition(uv);
    }

    private Vector2 CoordinateToRectDisplacement(Vector2 coordinateDisplacement)
    {
        Vector2 deltaUV = CoordinateDisplacementToUV(coordinateDisplacement);
        return UVToRectDisplacement(deltaUV);
    }
}
