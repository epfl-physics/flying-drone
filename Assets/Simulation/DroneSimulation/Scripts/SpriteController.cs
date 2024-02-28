// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    public void SetAlpha(float alpha)
    {
        if (TryGetComponent(out SpriteRenderer sprite))
        {
            Color color = sprite.color;
            color.a = alpha;
            sprite.color = color;
        }
    }

    public void SetSpriteVisibility(bool isHidden)
    {
        if (TryGetComponent(out SpriteRenderer sprite))
        {
            sprite.enabled = !isHidden;
        }
    }
}
