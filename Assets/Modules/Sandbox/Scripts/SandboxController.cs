using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SandboxController : MonoBehaviour
{
    public DroneSimulation sim;
    // public DroneSimulationState simState;

    // Drone parameters
    private float droneAxisOffset = 0;
    private bool droneIsOnAxis = true;
    private bool droneIsAtRestInR = true;
    private bool droneIsAtRestInRPrime = false;

    // Platform parameters
    private float defaultTranslationAmplitude = 0.7f;
    private float platformTranslationSpeed = 0;
    // private bool translationIsZero = true;
    private bool translationIsConstant = true;
    private bool translationIsVariable = false;
    private float platformAngularFrequency = 0;
    // private bool rotationIsZero = true;
    private bool rotationIsConstant = false;
    private bool rotationIsVariable = false;

    [Header("Toggle Groups")]
    // public ToggleGroup dronePositionTG;
    public ToggleGroup droneMotionTG;
    // public ToggleGroup platformDirectionTG;

    [Header("Sliders")]
    public Slider axisOffsetSlider;
    public Slider platformTranslationSlider;
    public SandboxSlider platformRotationSlider;

    [Header("Dropdowns")]
    public TMP_Dropdown platformTranslationDropdown;
    public TMP_Dropdown platformRotationDropdown;

    [Header("Trails")]
    public TrailRenderer droneTrail;
    public TrailRenderer pointMassTrail;

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
    }

    private void HandleSliderMouseDown()
    {
        if (sim) sim.Pause();
    }

    private void HandleSliderMouseUp()
    {
        if (sim) sim.Resume();
    }

    public void Start()
    {
        // Let variable rotation be independent from the translation rate
        if (sim) sim.UnlinkSinusoidalRotationFromTranslation();

        int translationTypeIndex = GetDropdownActiveIndex(platformTranslationDropdown);
        SetPlatformTranslationType(translationTypeIndex, false);

        int rotationTypeIndex = GetDropdownActiveIndex(platformRotationDropdown);
        SetPlatformRotationType(rotationTypeIndex, false);

        if (platformTranslationSlider) SetPlatformTranslationSpeed(platformTranslationSlider.value);
        if (platformRotationSlider) SetPlatformAngularFrequency(platformRotationSlider.value);

        int droneMotionIndex = GetToggleGroupActiveIndex(droneMotionTG);
        SetDroneMotionType(droneMotionIndex, false);

        if (axisOffsetSlider) SetDroneAxisOffset(axisOffsetSlider.value);

        ApplySimulationData(true);
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

        // translationIsZero = false;
        translationIsConstant = false;
        translationIsVariable = false;

        if (typeIndex == 0)
        {
            translationIsConstant = true;
        }
        else if (typeIndex == 1)
        {
            translationIsVariable = true;
        }

        if (applySimData) ApplySimulationData(true);
    }

    // private void SetPlatformRotationDirection(int directionIndex, bool setSimData)
    // {
    //     if (directionIndex < 0) return;

    //     rotationDirection = directionIndex == 1 ? 1 : -1;

    //     if (setSimData) SetSimulationData();
    // }

    // private void SetPlatformRotationToggleInteractivity(bool interactable)
    // {
    //     if (!platformDirectionTG) return;

    //     if (platformDirectionTG.TryGetComponent(out CanvasGroup cg))
    //     {
    //         cg.interactable = interactable;
    //     }

    //     if (platformDirectionTG.TryGetComponent(out CursorHoverUI cursorHover))
    //     {
    //         cursorHover.enabled = interactable;
    //     }

    //     foreach (TextMeshProUGUI tmp in platformDirectionTG.GetComponentsInChildren<TextMeshProUGUI>())
    //     {
    //         Color color = tmp.color;
    //         color.a = interactable ? 1 : 0.3f;
    //         tmp.color = color;
    //     }
    // }

    private void SetPlatformRotationType(int typeIndex, bool setSimData)
    {
        if (typeIndex < 0) return;

        // rotationIsZero = false;
        rotationIsConstant = false;
        rotationIsVariable = false;

        if (typeIndex == 0)
        {
            rotationIsConstant = true;
            // SetPlatformRotationToggleInteractivity(true);

        }
        else if (typeIndex == 1)
        {
            rotationIsVariable = true;
            // SetPlatformRotationToggleInteractivity(false);
        }

        if (setSimData) ApplySimulationData(true);
    }

    // private void SetDroneAxisOffset(int axisIndex, bool setSimData)
    // {
    //     Debug.Log(axisIndex + ", " + setSimData);
    //     if (axisIndex < 0) return;

    //     droneIsOnAxis = axisIndex == 1;

    //     if (setSimData) ApplySimulationData(true);
    // }

    public void SetDroneAxisOffsetAndApply()
    {
        // Note: Need to avoid getting the value from what is passed through OnValueChanged(float value)
        // since it will not have potential rounding applied
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
        // Debug.Log("SandboxController > SetDroneMotionType");
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
        }

        if (setSimData) ApplySimulationData(true);
    }

    public void OnPlatformTranslationOptionChanged(int value)
    {
        SetPlatformTranslationType(value, true);
    }

    // public void OnPlatformDirectionOptionChanged(bool triggerNewDirection)
    // {
    //     if (!triggerNewDirection) return;

    //     int activeIndex = GetToggleGroupActiveIndex(platformDirectionTG);
    //     SetPlatformRotationDirection(activeIndex, true);
    // }

    public void SetPlatformTranslationSpeedAndApply()
    {
        // Note: Need to avoid getting the value from what is passed through OnValueChanged(float value)
        // since it will not have potential rounding applied
        if (platformTranslationSlider)
        {
            SetPlatformTranslationSpeed(platformTranslationSlider.value);
            ApplySimulationData(true);
        }
    }

    private void SetPlatformTranslationSpeed(float value)
    {
        // Debug.Log("SandboxController > SetPlatformTranslationSpeed");
        platformTranslationSpeed = value;
        // translationIsZero = value == 0;
    }

    public void SetPlatformAngularFrequencyAndApply()
    {
        // Note: Need to avoid getting the value from what is passed through OnValueChanged(float value)
        // since it will not have potential rounding applied
        if (platformRotationSlider)
        {
            SetPlatformAngularFrequency(platformRotationSlider.value);
            ApplySimulationData(true);
        }
    }

    private void SetPlatformAngularFrequency(float value)
    {
        // Debug.Log("SandboxController > SetPlatformAngularFrequency > " + value);
        platformAngularFrequency = value;
        // rotationIsConstant = value != 0;
    }

    public void OnPlatformRotationOptionChanged(int value)
    {
        SetPlatformRotationType(value, true);
    }

    // public void OnDroneAxisOffsetChanged(bool triggerNewOffset)
    // {
    //     if (!triggerNewOffset) return;

    //     int activeIndex = GetToggleGroupActiveIndex(dronePositionTG);
    //     SetDroneAxisOffset(activeIndex, true);
    // }

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
        sim.platformData.translationAmplitude = defaultTranslationAmplitude;
        sim.platformData.translationPeriod = defaultTranslationAmplitude / platformTranslationSpeed;
        if (platformTranslationSpeed == 0)
        {
            sim.platformData.translationType = MovingPlatform.TranslationType.None;
        }
        else if (translationIsConstant)
        {
            sim.platformData.translationType = MovingPlatform.TranslationType.Linear;
        }
        else if (translationIsVariable)
        {
            sim.platformData.translationType = MovingPlatform.TranslationType.Sinusoidal;
        }

        Debug.Log("Translation is constant ? " + translationIsConstant);

        // Platform rotation frequency is Hz, while angular frequency here is rad / s.
        sim.platformData.rotationFrequency = platformAngularFrequency / (2 * Mathf.PI);
        if (platformAngularFrequency == 0)
        {
            sim.platformData.rotationType = MovingPlatform.RotationType.None;
        }
        else if (rotationIsConstant)
        {
            sim.platformData.rotationType = MovingPlatform.RotationType.Constant;
        }
        else if (rotationIsVariable)
        {
            sim.platformData.rotationType = MovingPlatform.RotationType.Sinusoidal;
        }

        sim.ApplyPlatformData(false);

        // Next deal with the drone
        sim.droneData.origin = 4 * Vector3.right;
        sim.droneData.verticalPeriod = sim.platformData.translationPeriod;
        sim.droneData.verticalAmplitude = sim.platformData.translationAmplitude;
        sim.droneData.circularFrequency = sim.platformData.rotationFrequency;

        sim.droneData.restPosition = 4 * Vector3.up + droneAxisOffset * Vector3.right;
        if (droneIsAtRestInR)
        {
            sim.droneData.circularRadius = 0;
            sim.droneData.verticalMotionType = Drone.VerticalMotionType.None;
            sim.droneData.circularMotionType = Drone.CircularMotionType.None;
            if (droneIsOnAxis)
            {
                sim.droneData.restPosition = 4 * Vector3.up;
            }
            else
            {
                sim.droneData.restPosition = 4 * Vector3.up + droneAxisOffset * Vector3.right;
            }
        }
        else if (droneIsAtRestInRPrime)
        {
            if (sim.platformData.translationType == MovingPlatform.TranslationType.None)
            {
                sim.droneData.verticalMotionType = Drone.VerticalMotionType.None;
            }
            else if (sim.platformData.translationType == MovingPlatform.TranslationType.Linear)
            {
                sim.droneData.verticalMotionType = Drone.VerticalMotionType.Linear;
            }
            else if (sim.platformData.translationType == MovingPlatform.TranslationType.Sinusoidal)
            {
                sim.droneData.verticalMotionType = Drone.VerticalMotionType.Sinusoidal;
            }

            if (droneIsOnAxis)
            {
                sim.droneData.restPosition = 4 * Vector3.up;
                sim.droneData.circularRadius = 0;
                sim.droneData.circularFrequency = 0;
                sim.droneData.circularMotionType = Drone.CircularMotionType.None;
            }
            else
            {
                if (platformAngularFrequency == 0)
                {
                    sim.droneData.restPosition = 4 * Vector3.up + droneAxisOffset * Vector3.right;
                    sim.droneData.circularRadius = droneAxisOffset;
                    sim.droneData.circularFrequency = 0;
                    sim.droneData.circularMotionType = Drone.CircularMotionType.None;
                }
                else if (rotationIsConstant)
                {
                    sim.droneData.restPosition = 4 * Vector3.up;
                    sim.droneData.circularRadius = droneAxisOffset;
                    sim.droneData.circularFrequency = sim.platformData.rotationFrequency;
                    sim.droneData.circularMotionType = Drone.CircularMotionType.Constant;
                }
                else if (rotationIsVariable)
                {
                    sim.droneData.restPosition = 4 * Vector3.up;
                    sim.droneData.circularRadius = droneAxisOffset;
                    sim.droneData.circularFrequency = sim.platformData.rotationFrequency;
                    sim.droneData.circularMotionType = Drone.CircularMotionType.Sinusoidal;
                }
            }
        }

        sim.ApplyDroneData(true, true);

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
            if (droneTrail) droneTrail.Clear();
            if (pointMassTrail) pointMassTrail.Clear();
        }

        // OnChangeSimulationParameters?.Invoke();
    }
}
