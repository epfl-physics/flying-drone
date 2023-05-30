using UnityEngine;
using UnityEngine.UI;

public class TheoryController : MonoBehaviour
{
    public DroneSimulation sim;
    public bool droneIsOnAxis = true;
    public bool droneIsAtRestInR = true;
    public bool droneIsAtRestInRPrime = false;
    public float defaultRotationRate = 0.1f;
    public float rotationDirection = 1f;
    public bool rotationIsConstant = false;
    public bool rotationIsVariable = false;

    [Header("Toggle Groups")]
    public ToggleGroup dronePositionTG;
    public ToggleGroup droneMotionTG;
    public ToggleGroup platformDirectionTG;
    public ToggleGroup platformRotationTG;

    public void Start()
    {
        int directionIndex = GetToggleGroupActiveIndex(platformDirectionTG);
        SetPlatformRotationDirection(directionIndex, false);

        int rotationTypeIndex = GetToggleGroupActiveIndex(platformRotationTG);
        SetPlatformRotationType(rotationTypeIndex, false);

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

    private void SetPlatformRotationDirection(int directionIndex, bool setSimData)
    {
        if (directionIndex < 0) return;

        rotationDirection = directionIndex == 1 ? 1 : -1;

        if (setSimData) SetSimulationData();
    }

    private void SetPlatformRotationType(int typeIndex, bool setSimData)
    {
        if (typeIndex < 0) return;

        rotationIsConstant = false;
        rotationIsVariable = false;

        if (typeIndex == 3)
        {
            rotationIsConstant = true;
        }
        else if (typeIndex == 4)
        {
            rotationIsVariable = true;
        }

        if (setSimData) SetSimulationData();
    }

    private void SetDroneAxisOffset(int axisIndex, bool setSimData)
    {
        if (axisIndex < 0) return;

        droneIsOnAxis = axisIndex == 1;

        if (setSimData) SetSimulationData();
    }

    private void SetDroneMotionType(int motionIndex, bool setSimData)
    {
        if (motionIndex < 0) return;

        droneIsAtRestInR = false;
        droneIsAtRestInRPrime = false;

        if (motionIndex == 3)
        {
            droneIsAtRestInR = true;
        }
        else if (motionIndex == 4)
        {
            droneIsAtRestInRPrime = true;
        }

        if (setSimData) SetSimulationData();
    }

    public void OnPlatformDirectionOptionChanged(bool triggerNewDirection)
    {
        if (!triggerNewDirection) return;

        int activeIndex = GetToggleGroupActiveIndex(platformDirectionTG);
        SetPlatformRotationDirection(activeIndex, true);
    }

    public void OnPlatformRotationOptionChanged(bool triggerNewRotationType)
    {
        if (!triggerNewRotationType) return;

        int activeIndex = GetToggleGroupActiveIndex(platformRotationTG);
        SetPlatformRotationType(activeIndex, true);
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

    private void SetSimulationData()
    {
        if (!sim) return;

        // Debug.Log("TheoryController > SetSimulationData");

        // First deal with the platform
        sim.platformData.rotationFrequency = rotationDirection * defaultRotationRate;
        if (rotationIsVariable)
        {
            sim.platformData.rotationType = MovingPlatform.RotationType.Sinusoidal;
        }
        else if (rotationIsConstant)
        {
            sim.platformData.rotationType = MovingPlatform.RotationType.Constant;
        }
        else
        {
            sim.platformData.rotationType = MovingPlatform.RotationType.None;
        }

        sim.ApplyPlatformData(false);

        sim.droneData.origin = 4 * Vector3.right;
        sim.droneData.verticalPeriod = sim.platformData.translationPeriod;
        sim.droneData.verticalAmplitude = sim.platformData.amplitude;
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
                sim.droneData.restPosition = 4 * Vector3.up + Vector3.right;
            }
        }
        else if (droneIsAtRestInRPrime)
        {
            sim.droneData.verticalMotionType = Drone.VerticalMotionType.Sinusoidal;
            if (droneIsOnAxis)
            {
                sim.droneData.restPosition = 4 * Vector3.up;
                sim.droneData.circularRadius = 0;
                sim.droneData.circularFrequency = 0;
                sim.droneData.circularMotionType = Drone.CircularMotionType.None;
            }
            else
            {
                if (sim.platformData.rotationType == MovingPlatform.RotationType.None)
                {
                    sim.droneData.restPosition = 4 * Vector3.up + Vector3.right;
                    sim.droneData.circularRadius = 0;
                    sim.droneData.circularFrequency = 0;
                    sim.droneData.circularMotionType = Drone.CircularMotionType.None;
                }
                else
                {
                    sim.droneData.restPosition = 4 * Vector3.up;
                    sim.droneData.circularRadius = 1;
                    sim.droneData.circularFrequency = sim.platformData.rotationFrequency;
                    if (sim.platformData.rotationType == MovingPlatform.RotationType.Constant)
                    {
                        sim.droneData.circularMotionType = Drone.CircularMotionType.Constant;
                    }
                    else
                    {
                        sim.droneData.circularMotionType = Drone.CircularMotionType.Sinusoidal;
                    }
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
                sim.droneData.circularRadius = 1;
                sim.droneData.circularFrequency = -sim.platformData.rotationFrequency;
                sim.droneData.circularMotionType = Drone.CircularMotionType.Constant;
                sim.droneData.verticalMotionType = Drone.VerticalMotionType.None;
            }
        }

        sim.ApplyDroneData(true);
    }
}
