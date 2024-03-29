// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Activity1 Scenario", menuName = "Activities/Activity1 Scenario", order = 1)]
public class ActivityScenario : ScriptableObject
{
    public PlatformData platformData;
    public DroneData droneData;
    public bool frameIsInertial = true;

    public enum VelocityTerm { Absolute, Platform, Relative, Tangential }
    public List<VelocityTerm> answers;
}
