using UnityEngine;

public class VectorManager : MonoBehaviour
{
    public LabeledVector platformPositionVector;
    public LabeledVector dronePositionVector;
    public LabeledVector relativePositionVector;

    public void Redraw(Vector3 platformPosition, Vector3 dronePosition)
    {
        if (platformPositionVector)
        {
            platformPositionVector.transform.position = transform.parent.localPosition;
            platformPositionVector.components = platformPosition;
            platformPositionVector.Redraw();
        }

        if (dronePositionVector)
        {
            dronePositionVector.transform.position = transform.parent.localPosition;
            dronePositionVector.components = dronePosition;
            dronePositionVector.Redraw();
        }

        if (relativePositionVector)
        {
            relativePositionVector.transform.position = transform.parent.localPosition + platformPosition;
            relativePositionVector.components = dronePosition - platformPosition;
            relativePositionVector.Redraw();
        }
    }
}
