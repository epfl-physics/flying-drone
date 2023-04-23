using UnityEngine;
using UnityEngine.UI;

public class DroneSlideController : Slides.SimulationSlideController
{
    [Header("Simulation")]
    public float droneOrbitalFrequency = 0;
    public GameObject platform;
    public bool platformIsVisible;
    public float platformRotationFrequency = 0f;

    [Header("Environment")]
    public GameObject tree;
    public bool treeIsVisible;
    public GameObject ground;
    public bool groundIsVisible;

    [Header("UI")]
    public Toggle[] checkboxes;

    public override void InitializeSlide()
    {
        DroneSimulation sim = (DroneSimulation)simulation;

        // sim.droneOrbitalFrequency = droneOrbitalFrequency;
        // sim.platformRotationFrequency = platformRotationFrequency;

        if (platform) platform.SetActive(platformIsVisible);

        if (tree) tree.SetActive(treeIsVisible);
        if (ground) ground.SetActive(groundIsVisible);

        foreach (Toggle checkbox in checkboxes)
        {
            checkbox.isOn = false;
        }
    }
}
