using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip[] footstepSounds;  // Array of footstep audio clips
    public AudioSource audioSource;     // Reference to the AudioSource component

    public bool isPlaying;

    private void Start()
    {
        // Ensure you have an AudioSource component attached to the same GameObject
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found!");
            }
        }
    }

    private void PlayRandomFootstepSound()
    {
        // Check if there are audio clips assigned
        if (footstepSounds.Length > 0)
        {
            // Randomly select a footstep sound from the array
            AudioClip randomFootstepSound = footstepSounds[Random.Range(0, footstepSounds.Length)];

            // Play the selected footstep sound

            audioSource.PlayOneShot(randomFootstepSound);
        }
    }

    // Call this method whenever you want to play a footstep sound (e.g., in the player's movement script)
    public void PlayFootstep()
    {
        PlayRandomFootstepSound();
    }

    public bool IsPlaying() { return audioSource.isPlaying; }
}
