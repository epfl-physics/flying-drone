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
        DroneSimulationState.OnChangeDisplay += HandleDisplayChanged;
    }

    public void OnDisable()
    {
        DroneSimulationState.OnChangeDisplay -= HandleDisplayChanged;
    }

    public void HandleDisplayChanged(DroneSimulationState.Display display)
    {
        // Debug.Log(transform.name + " heard display is " + display);

        TryGetComponent(out MeshRenderer meshRenderer);

        if (meshRenderer)
        {
            if (display == DroneSimulationState.Display.Physical)
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
                    meshRenderer.enabled = display == DroneSimulationState.Display.Physical;
                    break;
                case VisibleOn.Mathematical:
                    meshRenderer.enabled = display == DroneSimulationState.Display.Mathematical;
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
                        spriteRenderer.enabled = display == DroneSimulationState.Display.Physical;
                        break;
                    case VisibleOn.Mathematical:
                        spriteRenderer.enabled = display == DroneSimulationState.Display.Mathematical;
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
                            child.gameObject.SetActive(display == DroneSimulationState.Display.Physical);
                            break;
                        case VisibleOn.Mathematical:
                            child.gameObject.SetActive(display == DroneSimulationState.Display.Mathematical);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
