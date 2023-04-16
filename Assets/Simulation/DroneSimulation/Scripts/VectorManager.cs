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
            platformPositionVector.transform.position = Vector3.zero;
            platformPositionVector.components = platformPosition;
            platformPositionVector.Redraw();
        }

        if (dronePositionVector)
        {
            dronePositionVector.transform.position = Vector3.zero;
            dronePositionVector.components = dronePosition;
            dronePositionVector.Redraw();
        }

        if (relativePositionVector)
        {
            relativePositionVector.transform.position = platformPosition;
            relativePositionVector.components = dronePosition - platformPosition;
            relativePositionVector.Redraw();
        }
    }
}
