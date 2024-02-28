// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;

[CreateAssetMenu(fileName = "New Custom Cursor", menuName = "Custom Cursor", order = 30)]
public class CustomCursor : ScriptableObject
{
    public Texture2D texture = null;
    public Vector2 hotspot = default;
}
