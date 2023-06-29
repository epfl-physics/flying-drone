using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity1Controller : MonoBehaviour
{
    [SerializeField] private DroneSimulation sim;
    [SerializeField] private OptionSet options;

    [Range(0, 2)] public int difficulty;
    [SerializeField] private int answerIndex = -1;

    public void LoadRandomQuestion(int difficulty)
    {
        // Choose a scenario randomly with the given difficulty level

        // Assign the scenario parameters to the simulation

        // Store the correct answer
        answerIndex = Random.Range(0, 4);
    }

    public void CheckAnswer()
    {
        if (!options) return;

        int selectedIndex = options.SelectedIndex;
        if (selectedIndex == answerIndex)
        {
            // Debug.Log("Correct ! The answer is " + answerIndex);
            options.SelectionIsCorrect();
        }
        else
        {
            // Debug.Log(selectedIndex + " is not correct :(");
            options.SelectionIsIncorrect(answerIndex);
        }
    }
}
