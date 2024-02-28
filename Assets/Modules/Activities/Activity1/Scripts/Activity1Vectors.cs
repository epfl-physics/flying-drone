// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;

public class Activity1Vectors : MonoBehaviour
{
    [SerializeField] private ActivityCustomVector velocityVector;
    [SerializeField] private Vector omegaVector;
    [SerializeField] private DroneSimulationState simState;

    [Header("Colors")]
    [SerializeField] private Color colorUnknown;
    [SerializeField] private Color colorAbsolute;
    [SerializeField] private Color colorPlatform;
    [SerializeField] private Color colorRelative;
    [SerializeField] private Color colorTangential;

    private delegate Vector3 GetVelocityComponents();
    private GetVelocityComponents getVelocityComponents;

    private void OnEnable()
    {
        DroneSimulationState.OnRedrawVectors += HandleRedrawVectors;
        Activity1Controller.OnLoadScenario += HandleLoadScenario;
        Activity1Controller.OnAnswerIsCorrect += HandleAnswerIsCorrect;
    }

    private void OnDisable()
    {
        DroneSimulationState.OnRedrawVectors -= HandleRedrawVectors;
        Activity1Controller.OnLoadScenario -= HandleLoadScenario;
        Activity1Controller.OnAnswerIsCorrect -= HandleAnswerIsCorrect;
    }

    private void Start()
    {
        ResetColor();
    }

    public void HandleLoadScenario(int answerIndex)
    {
        switch (answerIndex)
        {
            case 0:
                getVelocityComponents = GetAbsoluteComponents;
                break;
            case 1:
                getVelocityComponents = GetPlatformComponents;
                break;
            case 2:
                getVelocityComponents = GetRelativeComponents;
                break;
            case 3:
                getVelocityComponents = GetTangentialComponents;
                break;
            default:
                break;
        }
    }

    public void HandleRedrawVectors()
    {
        if (!velocityVector || !simState) return;

        velocityVector.transform.position = simState.origin + simState.dronePositionAbsolute;
        velocityVector.components = getVelocityComponents();
        velocityVector.Redraw();

        if (simState.omega.magnitude != 0)
        {
            if (!omegaVector.gameObject.activeInHierarchy)
            {
                omegaVector.gameObject.SetActive(true);
            }
            omegaVector.components = simState.omega;
            omegaVector.Redraw();
        }
        else if (omegaVector.gameObject.activeInHierarchy)
        {
            omegaVector.gameObject.SetActive(false);
        }
    }

    public void SetVectorColor(Color color)
    {
        if (velocityVector)
        {
            velocityVector.color = color;
            velocityVector.SetColor();
        }
    }

    public void HandleAnswerIsCorrect(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                SetVectorColor(colorAbsolute);
                break;
            case 1:
                SetVectorColor(colorPlatform);
                break;
            case 2:
                SetVectorColor(colorRelative);
                break;
            case 3:
                SetVectorColor(colorTangential);
                break;
            default:
                break;
        }
    }

    public void ResetColor()
    {
        SetVectorColor(colorUnknown);
    }

    private Vector3 GetAbsoluteComponents()
    {
        Vector3 components = Vector3.zero;
        if (simState) components = simState.droneVelocityAbsolute;
        return components;
    }

    private Vector3 GetPlatformComponents()
    {
        Vector3 components = Vector3.zero;
        if (simState) components = simState.platformVelocity;
        return components;
    }

    private Vector3 GetRelativeComponents()
    {
        Vector3 components = Vector3.zero;
        if (simState) components = simState.droneVelocityRelative;
        return components;
    }

    private Vector3 GetTangentialComponents()
    {
        Vector3 components = Vector3.zero;
        if (simState) components = simState.tangentialVelocity;
        return components;
    }
}
