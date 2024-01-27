using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip[] footstepSounds;  // Array of footstep audio clips
    public AudioClip jumpSound;         // Jumping audio clip
    public AudioClip landSound;         // Landing audio clip
    public AudioClip slideSound;        // Slide audio clip
    public AudioSource fooutstepsSource;     // Reference to the footsteps AudioSource component
    public AudioSource jumpSource;     // Reference to the jump AudioSource component

    private void Start()
    {
        // Ensure you have an AudioSource component attached to the same GameObject
        if (fooutstepsSource == null)
        {
            fooutstepsSource = GetComponent<AudioSource>();
            if (fooutstepsSource == null)
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

            fooutstepsSource.PlayOneShot(randomFootstepSound);
        }
    }

    // Call this method whenever you want to play a footstep sound (e.g., in the player's movement script)
    public void PlayFootstep()
    {
        PlayRandomFootstepSound();
    }

    public bool IsPlaying() { return fooutstepsSource.isPlaying; }


    public void PlayJump()
    {
        jumpSource.PlayOneShot(jumpSound);
    }

    public void PlayLand()
    {
        jumpSource.PlayOneShot(landSound);
    }

    public void PlaySlide()
    {
        jumpSource.PlayOneShot(slideSound);
    }
}
