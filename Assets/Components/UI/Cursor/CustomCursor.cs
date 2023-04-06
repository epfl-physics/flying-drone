using UnityEngine;

[CreateAssetMenu(fileName = "New Custom Cursor", menuName = "Custom Cursor", order = 30)]
public class CustomCursor : ScriptableObject
{
    public Texture2D texture = null;
    public Vector2 hotspot = default;
}
