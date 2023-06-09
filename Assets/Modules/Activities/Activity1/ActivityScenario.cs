using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Activity Scenario", fileName = "New Activity Scenario", order = 57)]
public class ActivityScenario : ScriptableObject
{
    public PlatformData platformData;
    public DroneData droneData;
}
