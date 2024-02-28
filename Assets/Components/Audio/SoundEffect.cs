// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;

[CreateAssetMenu(menuName = "Sound Effect", fileName = "New Sound Effect", order = 60)]
public class SoundEffect : ScriptableObject
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField, Min(0)] private int clipIndex;

    [SerializeField, Range(0, 2)] private float pitch = 1;
    [SerializeField] private float pitchDelta = 0.1f;
    [SerializeField, Min(0)] private float volume = 1;

    public float Duration => audioClips[clipIndex].length;

    public void Play(AudioSource audioSource)
    {
        if (audioClips.Length == 0) return;

        if (audioSource.isPlaying) audioSource.Stop();

        // audioSource.pitch = pitch;
        audioSource.pitch = Random.Range(pitch - 0.5f * pitchDelta, pitch + 0.5f * pitchDelta);
        audioSource.volume = volume;
        audioSource.PlayOneShot(audioClips[Mathf.Clamp(clipIndex, 0, audioClips.Length - 1)]);
    }
}
