using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Activity2 Scenario", menuName = "Activities/Activity2 Scenario", order = 1)]
public class Activity2Scenario : ScriptableObject
{
    public PlatformData platformData;
    public DroneData droneData;

    public enum AccelerationTerm { Absolute, Platform, Relative, Centripetal, Coriolis, Euler }
    public List<AccelerationTerm> answers;
}
