// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
﻿using UnityEngine;

namespace Slides
{
    // Classes should inherit from SimulationSlideController and attach to a Slide
    // GameObject in order for the SlideManager to control the simulation's visibility
    public abstract class SimulationSlideController : MonoBehaviour
    {
        // [SerializeField] private bool autoPlay = true;
        [SerializeField] protected Simulation simulation;

        // Simulations are activated by SlideManager when changing slides
        public void ActivateSimulation()
        {
            if (!simulation) return;

            simulation.gameObject.SetActive(true);
            InitializeSlide();
        }

        // Simulations are deactivated by SlideManager when changing slides
        public void DeactivateSimulation()
        {
            if (simulation) simulation.gameObject.SetActive(false);
        }

        public virtual void InitializeSlide()
        {
            Debug.LogWarning(transform.name + " has not defined InitializeSlide()");
        }
    }
}
