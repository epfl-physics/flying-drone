// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Sprite playIcon;
    [SerializeField] private Sprite pauseIcon;

    private bool isPaused = true;

    public void Play()
    {
        isPaused = false;
        if (icon) icon.sprite = pauseIcon;
    }

    public void Pause()
    {
        isPaused = true;
        if (icon) icon.sprite = playIcon;
    }

    public void TogglePlayPause()
    {
        if (isPaused)
        {
            Play();
        }
        else
        {
            Pause();
        }
    }
}
