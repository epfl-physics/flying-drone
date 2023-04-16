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
