using System.Collections;
using UnityEngine;

namespace Slides
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Slide : MonoBehaviour
    {
        private CanvasGroup canvasGroup;
        private CameraController cameraController;
        private SimulationSlideController simSlideController;

        private void Awake()
        {
            TryGetComponent(out canvasGroup);
            TryGetComponent(out cameraController);
            TryGetComponent(out simSlideController);
        }

        public void Deactivate()
        {
            // Hide and disable all UI elements using the CanvasGroup
            if (canvasGroup)
            {
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }

            // Deactivate the associated simulations
            if (simSlideController)
            {
                simSlideController.DeactivateSimulation();
            }
        }

        // public void Activate()
        // {
        //     // Show and activate the UI of the starting slide
        //     if (canvasGroup)
        //     {
        //         canvasGroup.alpha = 1;
        //         canvasGroup.interactable = true;
        //         canvasGroup.blocksRaycasts = true;
        //     }

        //     // Activate the simulation for this slide
        //     if (simSlideController)
        //     {
        //         simSlideController.ActivateSimulation();
        //         // simSlideController.enabled = true;
        //     }

        //     // Move the camera to the correct position
        //     if (cameraController)
        //     {
        //         cameraController.TriggerCameraMovement();
        //         cameraController.SetCameraImmediately();
        //     }
        // }

        public void FadeIn(float fadeTime, float startDelay = 0, bool useCameraState = false)
        {
            if (canvasGroup)
            {
                StopAllCoroutines();
                StartCoroutine(Fade(1, fadeTime, startDelay));
            }

            if (simSlideController)
            {
                simSlideController.ActivateSimulation();
            }

            if (cameraController)
            {
                cameraController.TriggerCameraMovement(useCameraState);
            }
        }

        public void FadeOut(float fadeTime, float startDelay = 0)
        {
            if (canvasGroup)
            {
                StopAllCoroutines();
                StartCoroutine(Fade(0, fadeTime, startDelay));
            }

            if (simSlideController)
            {
                simSlideController.DeactivateSimulation();
            }

            // Stop the camera movement initiated by this slide
            if (cameraController)
            {
                cameraController.StopAllCoroutines();
            }
        }

        private IEnumerator Fade(float targetAlpha, float fadeTime, float startDelay = 0)
        {
            yield return new WaitForSeconds(startDelay);

            float time = 0;
            float startAlpha = canvasGroup.alpha;

            while (time < fadeTime)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeTime);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
        }
    }
}
