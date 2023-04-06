using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class VectorClickZone : MonoBehaviour
{
    [HideInInspector] public bool interactable;
    [SerializeField] private Material defaultMaterial;

    [Header("Cursor")]
    [SerializeField] private CustomCursor customCursor;

    private MeshRenderer mesh;

    public static event Action<VectorClickZone> OnZoneMouseDown;
    public static event Action<VectorClickZone> OnZoneMouseUp;

    private bool mouseIsDown;

    private void Awake()
    {
        if (TryGetComponent(out mesh))
        {
            mesh.enabled = false;
        }
    }

    private void OnDisable()
    {
        RestoreDefaultCursor();
    }

    private void OnMouseEnter()
    {
        if (!interactable) return;

        // Display the cursor while hovering
        if (customCursor) Cursor.SetCursor(customCursor.texture, customCursor.hotspot, CursorMode.Auto);

        if (mesh) mesh.enabled = true;
    }

    private void OnMouseExit()
    {
        if (!interactable || mouseIsDown) return;

        RestoreDefaultCursor();
    }

    private void RestoreDefaultCursor()
    {
        // Restore the default cursor
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        if (mesh) mesh.enabled = false;
    }

    private void OnMouseDown()
    {
        if (!interactable) return;

        mouseIsDown = true;
        OnZoneMouseDown?.Invoke(this);
    }

    private void OnMouseUp()
    {
        if (!interactable) return;

        RestoreDefaultCursor();

        mouseIsDown = false;
        OnZoneMouseUp?.Invoke(this);
    }

    public void SetColor(Color color)
    {
        if (defaultMaterial)
        {
            Material material = new Material(defaultMaterial);
            material.color = color;
            material.name = "Copy of " + defaultMaterial;
            GetComponent<MeshRenderer>().sharedMaterial = material;
        }
    }
}
