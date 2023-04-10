using UnityEngine;
using UnityEngine.SceneManagement;

namespace Slides
{
    public class Header : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
