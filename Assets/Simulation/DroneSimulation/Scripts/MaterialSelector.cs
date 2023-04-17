using UnityEngine;

public class MaterialSelector : MonoBehaviour
{
    public Material[] materials;

    public void SetMaterial(int index)
    {
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
