using UnityEngine;

public class Level_Music : MonoBehaviour
{
    [SerializeField] private AudioClip musicClip; // Sound to play, set in the Inspector
    [SerializeField] private float volume = 1.0f; // Volume of the music, adjustable in the Inspector

    private AudioSource audioSource;

    private void Awake()
    {
        // Add an AudioSource component if not already present
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.volume = volume;
        audioSource.loop = true; // Enable looping
        audioSource.playOnAwake = true; // Play automatically on Awake
        audioSource.Play(); // Start playing the sound
    }
}
