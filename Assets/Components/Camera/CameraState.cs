using UnityEngine;

[CreateAssetMenu(fileName = "New Camera State", menuName = "Camera State", order = 50)]
public class CameraState : ScriptableObject
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public Color backgroundColor;

    public void SetState(Camera camera)
    {
        if (!camera) return;

        position = camera.transform.position;
        rotation = camera.transform.rotation;
        scale = camera.transform.localScale;
        backgroundColor = camera.backgroundColor;
    }
}
