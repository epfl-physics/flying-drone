using UnityEngine;

public class Activity2Vectors : MonoBehaviour
{
    [SerializeField] private ActivityCustomVector accelerationVector;
    [SerializeField] private Vector omegaVector;
    [SerializeField] private DroneSimulationState simState;

    [Header("Colors")]
    [SerializeField] private Color colorUnknown;
    [SerializeField] private Color colorAbsolute;
    [SerializeField] private Color colorPlatform;
    [SerializeField] private Color colorRelative;
    [SerializeField] private Color colorCentripetal;
    [SerializeField] private Color colorCoriolis;
    [SerializeField] private Color colorEuler;

    private delegate Vector3 GetAccelerationComponents();
    private GetAccelerationComponents getAccelerationComponents;

    private void OnEnable()
    {
        DroneSimulationState.OnRedrawVectors += HandleRedrawVectors;
        Activity2Controller.OnLoadScenario += HandleLoadScenario;
        Activity2Controller.OnAnswerIsCorrect += HandleAnswerIsCorrect;
    }

    private void OnDisable()
    {
        DroneSimulationState.OnRedrawVectors -= HandleRedrawVectors;
        Activity2Controller.OnLoadScenario -= HandleLoadScenario;
        Activity2Controller.OnAnswerIsCorrect -= HandleAnswerIsCorrect;
    }

    private void Start()
    {
        getAccelerationComponents = GetDefaultComponents;
        ResetColor();
    }

    public void HandleLoadScenario(int answerIndex)
    {
        switch (answerIndex)
        {
            case 0:
                getAccelerationComponents = GetAbsoluteComponents;
                break;
            case 1:
                getAccelerationComponents = GetPlatformComponents;
                break;
            case 2:
                getAccelerationComponents = GetRelativeComponents;
                break;
            case 3:
                getAccelerationComponents = GetCentripetalComponents;
                break;
            case 4:
                getAccelerationComponents = GetCoriolisComponents;
                break;
            case 5:
                getAccelerationComponents = GetEulerComponents;
                break;
            default:
                getAccelerationComponents = GetDefaultComponents;
                break;
        }
    }

    public void HandleRedrawVectors()
    {
        if (!accelerationVector || !simState) return;

        accelerationVector.transform.position = simState.origin + simState.dronePositionAbsolute;
        accelerationVector.components = getAccelerationComponents();
        accelerationVector.Redraw();

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
        if (accelerationVector)
        {
            accelerationVector.color = color;
            accelerationVector.SetColor();
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
                SetVectorColor(colorCentripetal);
                break;
            case 4:
                SetVectorColor(colorCoriolis);
                break;
            case 5:
                SetVectorColor(colorEuler);
                break;
            default:
                break;
        }
    }

    public void ResetColor()
    {
        SetVectorColor(colorUnknown);
    }

    private Vector3 GetDefaultComponents()
    {
        return Vector3.zero;
    }

    private Vector3 GetAbsoluteComponents()
    {
        Vector3 components = Vector3.zero;
        if (simState) components = simState.droneAccelerationAbsolute;
        return components;
    }

    private Vector3 GetPlatformComponents()
    {
        Vector3 components = Vector3.zero;
        if (simState) components = simState.platformAcceleration;
        return components;
    }

    private Vector3 GetRelativeComponents()
    {
        Vector3 components = Vector3.zero;
        if (simState) components = simState.droneAccelerationRelative;
        return components;
    }

    private Vector3 GetCentripetalComponents()
    {
        Vector3 components = Vector3.zero;
        if (simState) components = simState.centripetalAcceleration;
        return components;
    }

    private Vector3 GetCoriolisComponents()
    {
        Vector3 components = Vector3.zero;
        if (simState) components = simState.coriolisAcceleration;
        return components;
    }

    private Vector3 GetEulerComponents()
    {
        Vector3 components = Vector3.zero;
        if (simState) components = simState.eulerAcceleration;
        return components;
    }
}
