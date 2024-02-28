// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;

[CreateAssetMenu(fileName = "New Camera State", menuName = "Camera State", order = 50)]
public class CameraState : ScriptableObject
{
    public Vector3 position = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;
    public Vector3 scale = Vector3.one;
    public Color backgroundColor = Color.white;

    public void SetState(Camera camera)
    {
        if (!camera) return;

        position = camera.transform.position;
        rotation = camera.transform.rotation;
        scale = camera.transform.localScale;
        backgroundColor = camera.backgroundColor;
    }
}
