// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class TheoryCanvasGroup : MonoBehaviour
{
    public void SetAlpha(bool isOne)
    {
        GetComponent<CanvasGroup>().alpha = isOne ? 1 : 0;
    }
}
