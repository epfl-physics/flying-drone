using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SandboxController : MonoBehaviour
{
    public DroneSimulation sim;

    // Drone parameters
    private float droneAxisOffset = 0;
    private bool droneIsOnAxis = true;
    private bool droneIsAtRestInR = true;
    private bool droneIsAtRestInRPrime = false;

    // Platform parameters
    private float translationAmplitude = 0.7f;
    private float translationSpeed = 0;
    private bool translationIsZero = true;
    private bool translationIsConstant = false;
    private bool translationIsVariable = false;
    private float angularFrequency = 0;
    private float angularFrequencyVariable = 0.15f;
    private bool rotationIsZero = true;
    private bool rotationIsConstant = false;
    private bool rotationIsVariable = false;

    [Header("Toggle Groups")]
    public ToggleGroup droneMotionTG;

    [Header("Sliders")]
    public CustomSlider axisOffsetSlider;
    public CustomSlider platformTranslationSlider;
    public CenteredSlider platformRotationSlider;
    private bool hasUpdatedFillArea;

    [Header("Dropdowns")]
    public TMP_Dropdown platformTranslationDropdown;
    public TMP_Dropdown platformRotationDropdown;

    [Header("Time")]
    public float slowMotionTimeScale = 0.25f;

    [Header("Trails")]
    public TrailRenderer droneTrail;
    public TrailRenderer pointMassTrail;
    private bool needToClearDroneTrail;
    private bool needToClearPointMassTrail;

    // public static event System.Action OnChangeSimulationParameters;

    private void OnEnable()
    {
        SandboxSlider.OnMouseDown += HandleSliderMouseDown;
        SandboxSlider.OnMouseUp += HandleSliderMouseUp;
    }

    private void OnDisable()
    {
        SandboxSlider.OnMouseDown -= HandleSliderMouseDown;
        SandboxSlider.OnMouseUp -= HandleSliderMouseUp;

        SetTimeScale(false);
    }

    private void HandleSliderMouseDown()
    {
        if (sim) sim.Pause();
    }

    private void HandleSliderMouseUp()
    {
        if (sim) sim.Resume();
    }

    private void Start()
    {
        // Platform translation
        int translationTypeIndex = GetDropdownActiveIndex(platformTranslationDropdown);
        SetPlatformTranslationType(translationTypeIndex, false);

        if (platformTranslationSlider) SetPlatformTranslationSpeed(platformTranslationSlider.value);

        // Platform rotation
        int rotationTypeIndex = GetDropdownActiveIndex(platformRotationDropdown);
        SetPlatformRotationType(rotationTypeIndex, false);

        if (platformRotationSlider) SetPlatformAngularFrequency(platformRotationSlider.value);

        // Drone
        int droneMotionIndex = GetToggleGroupActiveIndex(droneMotionTG);
        SetDroneMotionType(droneMotionIndex, false);

        if (axisOffsetSlider) SetDroneAxisOffset(axisOffsetSlider.value);

        ApplySimulationData();
    }

    private void LateUpdate()
    {
        // Clear trails late to avoid visual discontinuities

        if (needToClearDroneTrail)
        {
            if (droneTrail) droneTrail.Clear();
            needToClearDroneTrail = false;
        }

        if (needToClearPointMassTrail)
        {
            if (pointMassTrail) pointMassTrail.Clear();
            needToClearPointMassTrail = false;
        }
    }

    private int GetToggleGroupActiveIndex(ToggleGroup toggleGroup)
    {
        int index = -1;

        if (toggleGroup)
        {
            foreach (var toggle in toggleGroup.ActiveToggles())
            {
                index = toggle.transform.GetSiblingIndex();
            }
        }

        return index;
    }

    private int GetDropdownActiveIndex(TMP_Dropdown dropdown)
    {
        return dropdown ? dropdown.value : -1;
    }

    private void SetPlatformTranslationType(int typeIndex, bool applySimData)
    {
        if (typeIndex < 0) return;

        translationIsZero = true;
        translationIsConstant = false;
        translationIsVariable = false;

        switch (typeIndex)
        {
            case 0:
                translationIsConstant = true;
                break;
            case 1:
                translationIsVariable = true;
                break;
            default:
                break;
        }

        if (applySimData) ApplySimulationData(true);
    }

    private void SetPlatformRotationType(int typeIndex, bool setSimData)
    {
        if (typeIndex < 0) return;

        rotationIsZero = true;
        rotationIsConstant = false;
        rotationIsVariable = false;

        switch (typeIndex)
        {
            case 0:
                rotationIsConstant = true;
                break;
            case 1:
                rotationIsVariable = true;
                break;
            default:
                break;
        }

        if (setSimData) ApplySimulationData(true);
    }

    public void SetDroneAxisOffsetAndApply()
    {
        // Note: We need to avoid getting the value from what is passed through 
        // OnValueChanged(float value), since it will not have potential rounding applied
        // This is what is done here.
        if (axisOffsetSlider)
        {
            SetDroneAxisOffset(axisOffsetSlider.value);
            ApplySimulationData(true);
        }
    }

    private void SetDroneAxisOffset(float value)
    {
        // Debug.Log("SandboxController > SetDroneAxisOffset");
        droneAxisOffset = value;
        droneIsOnAxis = droneAxisOffset == 0;
    }

    private void SetDroneMotionType(int motionIndex, bool setSimData)
    {
        if (motionIndex < 0) return;

        droneIsAtRestInR = false;
        droneIsAtRestInRPrime = false;

        if (motionIndex == 0)
        {
            droneIsAtRestInR = true;
        }
        else if (motionIndex == 1)
        {
            droneIsAtRestInRPrime = true;
            sim.SynchronizeDroneClocksWithPlatform();
        }

        if (setSimData) ApplySimulationData(true);
    }

    public void OnPlatformTranslationOptionChanged(int value)
    {
        SetPlatformTranslationType(value, true);
    }

    public void SetPlatformTranslationSpeedAndApply()
    {
        // Note: We need to avoid getting the value from what is passed through 
        // OnValueChanged(float value), since it will not have potential rounding applied
        // This is what is done here.
        if (platformTranslationSlider)
        {
            SetPlatformTranslationSpeed(platformTranslationSlider.value);
            ApplySimulationData(true);
        }
    }

    private void SetPlatformTranslationSpeed(float value)
    {
        translationSpeed = value;
        translationIsZero = translationSpeed == 0;
    }

    public void SetPlatformAngularFrequencyAndApply()
    {
        // Note: We need to avoid getting the value from what is passed through 
        // OnValueChanged(float value), since it will not have potential rounding applied
        // This is what is done here.
        if (platformRotationSlider)
        {
            SetPlatformAngularFrequency(platformRotationSlider.value);
            ApplySimulationData(true);
        }
    }

    private void SetPlatformAngularFrequency(float value)
    {
        angularFrequency = value;
        rotationIsZero = angularFrequency == 0;
    }

    public void OnPlatformRotationOptionChanged(int value)
    {
        SetPlatformRotationType(value, true);
    }

    public void OnDroneMotionOptionChanged(bool triggerNewMotion)
    {
        if (!triggerNewMotion) return;

        int activeIndex = GetToggleGroupActiveIndex(droneMotionTG);
        SetDroneMotionType(activeIndex, true);
    }

    private void ApplySimulationData(bool clearTrail = false)
    {
        if (!sim) return;

        // Debug.Log("SandboxController > ApplySimulationData");

        // First deal with the platform
        sim.platformData.translationAmplitude = translationAmplitude;
        sim.platformData.translationFrequency = translationSpeed / translationAmplitude;
        if (translationIsConstant)
        {
            sim.platformData.translationType = MovingPlatform.TranslationType.Linear;
        }
        else if (translationIsVariable)
        {
            sim.platformData.translationType = MovingPlatform.TranslationType.Sinusoidal;
        }

        // Platform rotation frequency is Hz, while angular frequency here is rad / s.
        sim.platformData.rotationFrequency = angularFrequency / (2 * Mathf.PI);
        sim.platformData.rotationFrequencyVariable = angularFrequencyVariable;
        if (rotationIsConstant)
        {
            sim.platformData.rotationType = MovingPlatform.RotationType.Constant;
        }
        else if (rotationIsVariable)
        {
            sim.platformData.rotationType = MovingPlatform.RotationType.Sinusoidal;
        }

        sim.ApplyPlatformData();

        // Next the drone
        sim.droneData.origin = 4 * Vector3.right;
        sim.droneData.restPosition = 4 * Vector3.up + droneAxisOffset * Vector3.right;
        sim.droneData.verticalAmplitude = translationAmplitude;
        sim.droneData.circularRadius = droneAxisOffset;
        if (droneIsAtRestInR)
        {
            sim.droneData.verticalFrequency = 0;
            sim.droneData.circularFrequency = 0;
            sim.droneData.circularFrequencyVariable = 0;
        }
        else if (droneIsAtRestInRPrime)
        {
            sim.droneData.verticalFrequency = sim.platformData.translationFrequency;
            sim.droneData.circularFrequency = sim.platformData.rotationFrequency; // Recall the factor of 2pi
            sim.droneData.circularFrequencyVariable = angularFrequencyVariable;
            if (translationIsConstant)
            {
                sim.droneData.verticalMotionType = Drone.VerticalMotionType.Linear;
            }
            else if (translationIsVariable)
            {
                sim.droneData.verticalMotionType = Drone.VerticalMotionType.Sinusoidal;
            }

            if (rotationIsConstant)
            {
                sim.droneData.circularMotionType = Drone.CircularMotionType.Constant;
            }
            else if (rotationIsVariable)
            {
                sim.droneData.circularMotionType = Drone.CircularMotionType.Sinusoidal;
            }
        }

        sim.ApplyDroneData();

        // Assign hidden variables
        // if (simState)
        // {
        //     simState.droneIsOnAxis = droneIsOnAxis;
        //     simState.droneIsAtRestInR = droneIsAtRestInR;
        //     simState.droneIsAtRestInRPrime = droneIsAtRestInRPrime;
        //     simState.translationIsZero = translationIsZero;
        //     simState.translationIsConstant = translationIsConstant;
        //     simState.translationIsVariable = translationIsVariable;
        //     simState.rotationIsZero = rotationIsZero;
        //     simState.rotationIsConstant = rotationIsConstant;
        //     simState.rotationIsVariable = rotationIsVariable;
        // }

        if (clearTrail)
        {
            needToClearDroneTrail = true;
            needToClearPointMassTrail = true;
        }

        // OnChangeSimulationParameters?.Invoke();
    }

    public void TogglePointMass(bool showPointMass)
    {
        if (sim)
        {
            sim.drone.gameObject.SetActive(!showPointMass);
            sim.pointMass.gameObject.SetActive(showPointMass);
        }
    }

    public void SetTimeScale(bool isSlowMotion)
    {
        Time.timeScale = isSlowMotion ? slowMotionTimeScale : 1f;
    }
}
