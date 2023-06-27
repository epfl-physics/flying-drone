using UnityEngine;

public class CameraTagger : MonoBehaviour
{
    public static event System.Action OnMainCameraChanged;

    public void SetCameraAsMain(bool isMain)
    {
        gameObject.tag = isMain ? "MainCamera" : "Untagged";

        if (isMain)
        {
            // Debug.Log("CameraTagger > " + transform.name + " is main");
            gameObject.SetActive(true);
            OnMainCameraChanged?.Invoke();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
