using UnityEngine;

[ExecuteInEditMode]
public class DisplayController : MonoBehaviour
{
    [SerializeField] private Material opaque;
    [SerializeField] private Material transparent;

    private enum VisibleOn { Both, Physical, Mathematical }
    [SerializeField] VisibleOn visibleOn;

    public void OnEnable()
    {
        DroneSimulationState.OnChangeDisplayMode += HandleDisplayModeChanged;
    }

    public void OnDisable()
    {
        DroneSimulationState.OnChangeDisplayMode -= HandleDisplayModeChanged;
    }

    public void HandleDisplayModeChanged(DroneSimulationState.DisplayMode displayMode)
    {
        // Debug.Log(transform.name + " heard display is " + display);

        TryGetComponent(out MeshRenderer meshRenderer);

        if (meshRenderer)
        {
            if (displayMode == DroneSimulationState.DisplayMode.Physical)
            {
                if (opaque) meshRenderer.sharedMaterial = opaque;
            }
            else
            {
                if (transparent) meshRenderer.sharedMaterial = transparent;
            }

            switch (visibleOn)
            {
                case VisibleOn.Both:
                    meshRenderer.enabled = true;
                    break;
                case VisibleOn.Physical:
                    meshRenderer.enabled = displayMode == DroneSimulationState.DisplayMode.Physical;
                    break;
                case VisibleOn.Mathematical:
                    meshRenderer.enabled = displayMode == DroneSimulationState.DisplayMode.Mathematical;
                    break;
                default:
                    break;
            }
        }
        else
        {
            TryGetComponent(out SpriteRenderer spriteRenderer);

            if (spriteRenderer)
            {
                switch (visibleOn)
                {
                    case VisibleOn.Both:
                        spriteRenderer.enabled = true;
                        break;
                    case VisibleOn.Physical:
                        spriteRenderer.enabled = displayMode == DroneSimulationState.DisplayMode.Physical;
                        break;
                    case VisibleOn.Mathematical:
                        spriteRenderer.enabled = displayMode == DroneSimulationState.DisplayMode.Mathematical;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                foreach (Transform child in transform)
                {
                    switch (visibleOn)
                    {
                        case VisibleOn.Both:
                            child.gameObject.SetActive(true);
                            break;
                        case VisibleOn.Physical:
                            child.gameObject.SetActive(displayMode == DroneSimulationState.DisplayMode.Physical);
                            break;
                        case VisibleOn.Mathematical:
                            child.gameObject.SetActive(displayMode == DroneSimulationState.DisplayMode.Mathematical);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
