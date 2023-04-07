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
