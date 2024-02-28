// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;

[CreateAssetMenu(menuName = "Activities/Vector Colors", fileName = "New Vector Colors", order = 0)]
public class VectorColors : ScriptableObject
{
    public Color defaultColor = Color.black;
    public VectorColor[] colors;

    public Color GetColorFromIndex(int index)
    {
        Color color = defaultColor;

        if (index >= 0 && index < colors.Length)
        {
            color = colors[index].color;
        }

        return color;
    }
}

[System.Serializable]
public class VectorColor
{
    public string name;
    public Color color = Color.black;
}
