// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;

public class LabeledVector : Vector
{
    public Transform label;
    public Vector3 labelOffset;

    public override void Redraw()
    {
        base.Redraw();

        if (label)
        {
            label.localPosition = 0.5f * components + labelOffset;
        }
    }
}
