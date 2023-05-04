using UnityEngine;

[CreateAssetMenu(fileName = "New Drone Simulation State", menuName = "Drone Simulation State", order = 56)]
public class DroneSimulationState : ScriptableObject
{
    public float droneTime;
    public float platformTime;

    [Header("Rotation")]
    public Vector3 omega;

    [Header("Position")]
    public Vector3 origin;
    public Vector3 dronePositionAbsolute;
    public Vector3 dronePositionRelative;
    public Vector3 platformPosition;

    [Header("Velocity")]
    public Vector3 droneVelocityAbsolute;
    public Vector3 droneVelocityRelative;
    public Vector3 platformVelocity;
    public Vector3 tangentialVelocity;

    [Header("Acceleration")]
    public Vector3 droneAccelerationAbsolute;
    public Vector3 droneAccelerationRelative;
    public Vector3 platformAcceleration;
    public Vector3 centrifugalAcceleration;
    public Vector3 coriolisAcceleration;
    public Vector3 eulerAcceleration;

    public static event System.Action OnRedrawVectors;

    public void RedrawVectors()
    {
        OnRedrawVectors?.Invoke();
    }
}
