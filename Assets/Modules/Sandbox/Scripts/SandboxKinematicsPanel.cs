// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;

public class SandboxKinematicsPanel : MonoBehaviour
{
    private void Start()
    {
        ActivateTab(0);
    }

    public void ActivateTab(int tabIndex)
    {
        // Debug.Log("Activating tab " + tabIndex);
        if (tabIndex >= 0 && tabIndex < transform.childCount)
        {
            transform.GetChild(tabIndex).GetComponent<Toggle>().isOn = true;
        }
    }
}
