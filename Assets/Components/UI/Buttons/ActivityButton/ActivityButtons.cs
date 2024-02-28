// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivityButtons : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        // if (SceneManager.GetSceneByName(sceneName).IsValid())
        // {
        //     SceneManager.LoadScene(sceneName);
        // }
        SceneManager.LoadScene(sceneName);
    }
}
