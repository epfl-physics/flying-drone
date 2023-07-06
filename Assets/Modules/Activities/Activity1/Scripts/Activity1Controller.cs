using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Activity1Controller : MonoBehaviour
{
    [SerializeField] private DroneSimulation sim;
    [SerializeField] private OptionSet options;
    [SerializeField] private MultilingualDropdown difficultyDropdown;
    [SerializeField] private Toggle treeFrameToggle;

    [Range(0, 2)] public int difficulty;
    private List<int> answerIndex;
    private int previousActivityIndex = -1;

    [Header("Cameras")]
    [SerializeField] private Transform absoluteCamera;
    // [SerializeField] private Transform relativeCamera;
    // [SerializeField] private bool autoFrame;

    [Header("Scenarios")]
    [SerializeField] private ActivityScenario[] beginnerScenarios;
    [SerializeField] private ActivityScenario[] intermediateScenarios;
    [SerializeField] private ActivityScenario[] advancedScenarios;

    public static event System.Action<int> OnLoadScenario;
    public static event System.Action<int> OnAnswerIsCorrect;

    private void Start()
    {
        if (difficultyDropdown) difficulty = difficultyDropdown.startIndex;
        LoadRandomQuestion();
    }

    public void LoadRandomQuestion()
    {
        // Choose a scenario randomly with the given difficulty level
        ActivityScenario[] scenarios;
        if (difficulty == 0)
        {
            scenarios = beginnerScenarios;
        }
        else if (difficulty == 1)
        {
            scenarios = intermediateScenarios;
        }
        else if (difficulty == 2)
        {
            scenarios = advancedScenarios;
        }
        else
        {
            scenarios = new ActivityScenario[0];
        }

        int activityIndex = Random.Range(0, scenarios.Length);
        if (scenarios.Length > 1)
        {
            // Make sure to pick a new scenario
            while (activityIndex == previousActivityIndex)
            {
                activityIndex = Random.Range(0, scenarios.Length);
            }
            previousActivityIndex = activityIndex;
        }

        // Assign the scenario parameters to the simulation
        ActivityScenario scenario = scenarios[activityIndex];
        LoadScenario(scenario);

        // Store the correct answer(s)
        answerIndex = new List<int>();
        foreach (ActivityScenario.VelocityTerm answer in scenario.answers)
        {
            answerIndex.Add((int)answer);
        }
    }

    private void LoadScenario(ActivityScenario scenario)
    {
        if (!sim) return;

        // First deal with the platform
        sim.platformData = scenario.platformData;
        sim.ApplyPlatformData(false);

        sim.droneData = scenario.droneData;
        sim.ApplyDroneData(true, true);

        // Deal with camera frame
        // if (autoFrame)
        // {
        //     SetCameraAsMain(absoluteCamera, scenario.frameIsInertial);
        //     SetCameraAsMain(relativeCamera, !scenario.frameIsInertial);
        // }

        OnLoadScenario?.Invoke((int)scenario.answers[0]);
    }

    public void CheckAnswer()
    {
        if (!options) return;

        int selectedIndex = options.SelectedIndex;
        if (answerIndex.Contains(selectedIndex))
        {
            // Debug.Log("Correct ! The answer is " + answerIndex);
            options.SelectionIsCorrect();
            OnAnswerIsCorrect?.Invoke(selectedIndex);
        }
        else
        {
            // Debug.Log(selectedIndex + " is not correct :(");
            options.SelectionIsIncorrect(answerIndex);
        }
    }

    private void SetCameraAsMain(Transform camera, bool isMain)
    {
        if (camera)
        {
            camera.gameObject.SetActive(isMain);
            if (camera.TryGetComponent(out CameraTagger tagger))
            {
                tagger.SetCameraAsMain(isMain);
            }
        }
    }

    public void ResetCamera()
    {
        // SetCameraAsMain(absoluteCamera, true);
        // SetCameraAsMain(relativeCamera, false);
        // if (absoluteCamera)
        // {
        //     if (absoluteCamera.TryGetComponent(out CameraController cameraController))
        //     {
        //         cameraController.SetCameraImmediately();
        //     }
        // }

        if (treeFrameToggle) treeFrameToggle.isOn = true;

        // Reset absolute camera to original position and rotation
        if (absoluteCamera)
        {
            if (absoluteCamera.TryGetComponent(out CameraController cameraController))
            {
                cameraController.SetCameraImmediately();
            }
        }
    }

    public void SetDifficulty(int index)
    {
        difficulty = Mathf.Clamp(index, 0, 2);
        LoadRandomQuestion();
        ResetCamera();
    }
}
