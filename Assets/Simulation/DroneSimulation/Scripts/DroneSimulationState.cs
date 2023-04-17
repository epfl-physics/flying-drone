using UnityEngine;

[CreateAssetMenu(fileName = "New Drone Simulation State", menuName = "Drone Simulation State", order = 56)]
public class DroneSimulationState : ScriptableObject
{
    public float droneTime;
    public float platformTime;

    public bool droneIsIndependent;

    public enum DisplayMode { Physical, Mathematical }
    public DisplayMode displayMode = DisplayMode.Physical;

    public static event System.Action<DisplayMode> OnChangeDisplayMode;

    public void SetDisplayMode(bool isPhysical)
    {
        SetDisplayMode(isPhysical ? DisplayMode.Physical : DisplayMode.Mathematical);
        Debug.Log("DroneSimulationState > SetDisplayMode(bool)");
    }

    public void SetDisplayMode(DisplayMode displayMode)
    {
        this.displayMode = displayMode;
        OnChangeDisplayMode?.Invoke(displayMode);
        Debug.Log("DroneSimulationState > SetDisplayMode(DisplayMode)");
    }
}
