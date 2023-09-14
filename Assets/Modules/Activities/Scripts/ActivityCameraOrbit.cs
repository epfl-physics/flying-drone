using UnityEngine;

public class ActivityCameraOrbit : CameraOrbit
{
    protected override void OnEnable()
    {
        base.OnEnable();

        CameraTagger.OnMainCameraChanged += HandleMainCameraChanged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        CameraTagger.OnMainCameraChanged -= HandleMainCameraChanged;
    }

    public void HandleMainCameraChanged()
    {
        // Debug.Log(transform.name + " CameraTagger.OnMainCameraChanged");
        Initialize();
    }
}
