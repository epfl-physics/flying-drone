using UnityEngine;

[CreateAssetMenu(fileName = "New Drone Simulation State", menuName = "Drone Simulation State", order = 56)]
public class DroneSimulationState : ScriptableObject
{
    public float droneTime;
    public float platformTime;

    public enum Display { Physical, Mathematical }
    public Display display = Display.Physical;

    public static event System.Action<Display> OnChangeDisplay;

    public void SetDisplay(Display display)
    {
        this.display = display;
        OnChangeDisplay?.Invoke(display);
    }
}
