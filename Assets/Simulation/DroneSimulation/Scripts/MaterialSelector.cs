using UnityEngine;

public class MaterialSelector : MonoBehaviour
{
    public Material[] materials;
    public int currentIndex;

    public void OnValidate()
    {
        if (materials != null)
        {
            int maxInt = Mathf.Max(0, materials.Length - 1);
            currentIndex = Mathf.Clamp(currentIndex, 0, maxInt);
        }
    }

    public void SetMaterial(int index)
    {
        if (materials == null) return;

        if (index < 0 || index >= materials.Length) return;

        if (TryGetComponent(out MeshRenderer renderer))
        {
            renderer.sharedMaterial = materials[index];
        }
    }

    public void SetMaterial(bool isSecond)
    {
        SetMaterial(isSecond ? 1 : 0);
    }
}
