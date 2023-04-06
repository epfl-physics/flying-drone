using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Sprite playIcon;
    [SerializeField] private Sprite pauseIcon;

    public void Play()
    {
        if (icon) icon.sprite = pauseIcon;
    }

    public void Pause()
    {
        if (icon) icon.sprite = playIcon;
    }
}
