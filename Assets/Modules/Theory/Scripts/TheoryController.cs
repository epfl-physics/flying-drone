using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TheoryController : MonoBehaviour
{
    public DroneSimulation sim;
    public DroneSimulationState simState;
    public float droneAxisOffset = 1;
    private bool droneIsOnAxis = true;
    private bool droneIsAtRestInR = true;
    private bool droneIsAtRestInRPrime = false;
    private float defaultTranslationAmplitude = 0.7f;
    private float defaultTranslationPeriod = 4f;
    private bool translationIsZero = false;
    private bool translationIsConstant = false;
    private bool translationIsVariable = true;
    private float defaultRotationRate = 0.1f;
    private float rotationDirection = 1f;
    private bool rotationIsZero = true;
    private bool rotationIsConstant = false;
    private bool rotationIsVariable = false;

    [Header("Toggle Groups")]
    public ToggleGroup dronePositionTG;
    public ToggleGroup droneMotionTG;
    public ToggleGroup platformDirectionTG;

    [Header("Dropdowns")]
    public TMP_Dropdown platformTranslationDropdown;
    public TMP_Dropdown platformRotationDropdown;

    [Header("Trails")]
    public TrailRenderer droneTrail;
    public TrailRenderer pointMassTrail;

    public static event System.Action OnChangeSimulationParameters;

    public void Start()
    {
        // int translationTypeIndex = GetToggleGroupActiveIndex(platformTranslationTG);
        int translationTypeIndex = GetDropdownActiveIndex(platformTranslationDropdown);
        SetPlatformTranslationType(translationTypeIndex, false);

        // int rotationTypeIndex = GetToggleGroupActiveIndex(platformRotationTG);
        int rotationTypeIndex = GetDropdownActiveIndex(platformRotationDropdown);
        SetPlatformRotationType(rotationTypeIndex, false);

        int directionIndex = GetToggleGroupActiveIndex(platformDirectionTG);
        SetPlatformRotationDirection(directionIndex, false);

        int axisIndex = GetToggleGroupActiveIndex(dronePositionTG);
        SetDroneAxisOffset(axisIndex, false);

        int droneMotionIndex = GetToggleGroupActiveIndex(droneMotionTG);
        SetDroneMotionType(droneMotionIndex, false);

        SetSimulationData();
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

    private void SetPlatformTranslationType(int typeIndex, bool setSimData)
    {
        if (typeIndex < 0) return;

        translationIsZero = false;
        translationIsConstant = false;
        translationIsVariable = false;

        if (typeIndex == 0)
        {
            translationIsZero = true;
        }
        else if (typeIndex == 1)
        {
            translationIsConstant = true;
        }
        else if (typeIndex == 2)
        {
            translationIsVariable = true;
        }

        if (setSimData) SetSimulationData();
    }

    private void SetPlatformRotationDirection(int directionIndex, bool setSimData)
    {
        if (directionIndex < 0) return;

        rotationDirection = directionIndex == 1 ? 1 : -1;

        if (setSimData) SetSimulationData();
    }

    private void SetPlatformRotationToggleInteractivity(bool interactable)
    {
        if (!platformDirectionTG) return;

        if (platformDirectionTG.TryGetComponent(out CanvasGroup cg))
        {
            cg.interactable = interactable;
        }

        if (platformDirectionTG.TryGetComponent(out CursorHoverUI cursorHover))
        {
            cursorHover.enabled = interactable;
        }

        foreach (TextMeshProUGUI tmp in platformDirectionTG.GetComponentsInChildren<TextMeshProUGUI>())
        {
            Color color = tmp.color;
            color.a = interactable ? 1 : 0.3f;
            tmp.color = color;
        }
    }

    private void SetPlatformRotationType(int typeIndex, bool setSimData)
    {
        if (typeIndex < 0) return;

        rotationIsZero = false;
        rotationIsConstant = false;
        rotationIsVariable = false;

        if (typeIndex == 0)
        {
            rotationIsZero = true;
            SetPlatformRotationToggleInteractivity(false);
        }
        else if (typeIndex == 1)
        {
            rotationIsConstant = true;
            SetPlatformRotationToggleInteractivity(true);

        }
        else if (typeIndex == 2)
        {
            rotationIsVariable = true;
            SetPlatformRotationToggleInteractivity(false);
        }

        if (setSimData) SetSimulationData();
    }

    private void SetDroneAxisOffset(int axisIndex, bool setSimData)
    {
        if (axisIndex < 0) return;

        droneIsOnAxis = axisIndex == 1;

        if (setSimData) SetSimulationData(true);
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
        }

        if (setSimData) SetSimulationData();
    }

    public void OnPlatformTranslationOptionChanged(int value)
    {
        SetPlatformTranslationType(value, true);
    }

    public void OnPlatformDirectionOptionChanged(bool triggerNewDirection)
    {
        if (!triggerNewDirection) return;

        int activeIndex = GetToggleGroupActiveIndex(platformDirectionTG);
        SetPlatformRotationDirection(activeIndex, true);
    }

    public void OnPlatformRotationOptionChanged(int value)
    {
        SetPlatformRotationType(value, true);
    }

    public void OnDroneAxisOffsetChanged(bool triggerNewOffset)
    {
        if (!triggerNewOffset) return;

        int activeIndex = GetToggleGroupActiveIndex(dronePositionTG);
        SetDroneAxisOffset(activeIndex, true);
    }

    public void OnDroneMotionOptionChanged(bool triggerNewMotion)
    {
        if (!triggerNewMotion) return;

        int activeIndex = GetToggleGroupActiveIndex(droneMotionTG);
        SetDroneMotionType(activeIndex, true);
    }

    private void SetSimulationData(bool clearTrail = false)
    {
        if (!sim) return;

        // Debug.Log("TheoryController > SetSimulationData");

        // First deal with the platform
        sim.platformData.translationAmplitude = defaultTranslationAmplitude;
        sim.platformData.translationPeriod = defaultTranslationPeriod;
        if (translationIsZero)
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

        sim.platformData.rotationFrequency = rotationDirection * defaultRotationRate;
        if (rotationIsZero)
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

        sim.droneData.origin = 4 * Vector3.right;
        sim.droneData.verticalPeriod = sim.platformData.translationPeriod;
        sim.droneData.verticalAmplitude = sim.platformData.translationAmplitude;
        sim.droneData.circularFrequency = sim.platformData.rotationFrequency;
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
                if (rotationIsZero)
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
        else
        {
            // Not at rest in R or R' !!
            if (droneIsOnAxis)
            {
                sim.droneData.restPosition = 4 * Vector3.up;
                sim.droneData.circularRadius = 0;
                sim.droneData.circularMotionType = Drone.CircularMotionType.Constant;
                sim.droneData.verticalMotionType = Drone.VerticalMotionType.Linear;
            }
            else
            {
                sim.droneData.restPosition = 4 * Vector3.up;
                sim.droneData.circularRadius = droneAxisOffset;
                sim.droneData.circularFrequency = -sim.platformData.rotationFrequency;
                sim.droneData.circularMotionType = Drone.CircularMotionType.Constant;
                sim.droneData.verticalMotionType = Drone.VerticalMotionType.None;
            }
        }

        sim.ApplyDroneData(true, true);

        // Assign hidden variables
        if (simState)
        {
            simState.droneIsOnAxis = droneIsOnAxis;
            simState.droneIsAtRestInR = droneIsAtRestInR;
            simState.droneIsAtRestInRPrime = droneIsAtRestInRPrime;
            simState.translationIsZero = translationIsZero;
            simState.translationIsConstant = translationIsConstant;
            simState.translationIsVariable = translationIsVariable;
            simState.rotationIsZero = rotationIsZero;
            simState.rotationIsConstant = rotationIsConstant;
            simState.rotationIsVariable = rotationIsVariable;
        }

        if (clearTrail)
        {
            if (droneTrail) droneTrail.Clear();
            if (pointMassTrail) pointMassTrail.Clear();
        }

        OnChangeSimulationParameters?.Invoke();
    }
}
