using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSlideController : Slides.SimulationSlideController
{
    public bool droneIsMoving;

    public override void InitializeSlide()
    {
        DroneSimulation sim = (DroneSimulation)simulation;


    }
}
