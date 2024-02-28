// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;

[CreateAssetMenu(fileName = "New Cube Simulation State", menuName = "Cube Simulation State", order = 55)]
public class CubeSimulationState : ScriptableObject
{
    public Quaternion rotation;
    public float time;
}
