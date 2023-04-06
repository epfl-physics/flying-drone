using UnityEngine;

[CreateAssetMenu(fileName = "New Cube Simulation State", menuName = "Cube Simulation State", order = 55)]
public class CubeSimulationState : ScriptableObject
{
    public Quaternion rotation;
    public float time;
}
