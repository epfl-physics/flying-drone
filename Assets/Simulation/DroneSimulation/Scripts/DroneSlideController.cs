using UnityEngine;
using UnityEngine.UI;

public class DroneSlideController : Slides.SimulationSlideController
{
    [Header("Simulation")]
    public float droneVerticalAmplitude = 0.5f;
    public float droneVerticalFrequency = 0.3f;
    public float droneCircularFrequency = 0;
    public Drone.CircularMotionType droneCircularMotionType;
    public GameObject platform;
    public bool platformIsVisible;
    public float platformTranslationAmplitude = 0.5f;
    public float platformTranslationFrequency = 0.3f;
    public float platformRotationFrequency = 0f;
    public MovingPlatform.TranslationType platformTranslationType;
    public MovingPlatform.RotationType platformRotationType;

    [Header("Environment")]
    public GameObject tree;
    public bool treeIsVisible;
    public GameObject ground;
    public bool groundIsVisible;

    [Header("Vectors")]
    public VectorManager vectorManager;
    public bool showDronePositionAbsolute;
    public bool showDronePositionRelative;
    public bool showPlatformPosition;

    [Header("UI")]
    public Toggle[] checkboxesOff;
    public Toggle[] checkboxesOn;

    public override void InitializeSlide()
    {
        DroneSimulation sim = (DroneSimulation)simulation;

        sim.platformData.translationAmplitude = platformTranslationAmplitude;
        sim.platformData.translationFrequency = platformTranslationFrequency;
        sim.platformData.translationType = platformTranslationType;
        sim.platformData.rotationFrequency = platformRotationFrequency;
        sim.platformData.rotationType = platformRotationType;
        sim.ApplyPlatformData();

        sim.droneData.circularFrequency = droneCircularFrequency;
        sim.droneData.verticalAmplitude = droneVerticalAmplitude;
        sim.droneData.verticalFrequency = droneVerticalFrequency;
        sim.droneData.circularMotionType = droneCircularMotionType;
        sim.ApplyDroneData();

        // Object visibility
        if (platform) platform.SetActive(platformIsVisible);
        if (tree) tree.SetActive(treeIsVisible);
        if (ground) ground.SetActive(groundIsVisible);

        // Vector visibility
        if (vectorManager)
        {
            vectorManager.showDronePositionAbsolute = showDronePositionAbsolute;
            vectorManager.showDronePositionRelative = showDronePositionRelative;
            vectorManager.showPlatformPosition = showPlatformPosition;
        }

        foreach (Toggle checkbox in checkboxesOff)
        {
            checkbox.isOn = false;
        }

        foreach (Toggle checkbox in checkboxesOn)
        {
            checkbox.isOn = true;
        }
    }
}
