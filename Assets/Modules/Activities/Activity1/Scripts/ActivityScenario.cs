using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Activity Scenario", menuName = "Drone Simulation/Activity Scenario", order = 1)]
public class ActivityScenario : ScriptableObject
{
    public PlatformData platformData;
    public DroneData droneData;
    public bool frameIsInertial = true;

    public enum VelocityTerm { Absolute, Platform, Relative, Tangential }
    public List<VelocityTerm> answers;
}