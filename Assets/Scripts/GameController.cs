using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Static reference to the instance of the GameController
    private static GameController instance;

    // Public property to access the GameController instance
    public static GameController Instance
    {
        get
        {
            if (instance == null)
            {
                // If instance is null, try to find an existing GameController in the scene
                instance = FindObjectOfType<GameController>();

                // If there is no existing GameController, create a new one
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameController");
                    instance = obj.AddComponent<GameController>();
                }
            }

            return instance;
        }
    }

    public bool Started;
    public PlayerController player;
    public CameraFollow cameraFollow;
    public Canvas Menu;


    public AudioClip IntroMusic;
    public AudioClip LevelMusic;
    public AudioSource audioSource;

    private bool isPaused = false;

    private void Awake()
    {
        // Ensure there is only one instance of the GameController
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void Update()
    {
        // Check for the Escape key to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

    }

    void TogglePause()
    {
        // Toggle the isPaused flag
        isPaused = !isPaused;

        // Pause or unpause the game based on the isPaused flag
        if (isPaused)
        {
            Pause();
        }
        else
        {
            Unpause();
        }
    }

    void Pause()
    {
        // Pause the game by setting the time scale to 0
        Time.timeScale = 0f;

        // You can also add additional logic for pausing game elements or showing a pause menu
        Debug.Log("Game Paused");
    }

    void Unpause()
    {
        // Unpause the game by setting the time scale back to 1
        Time.timeScale = 1f;

        // You can also add additional logic for unpausing game elements or hiding a pause menu
        Debug.Log("Game Unpaused");
    }


    private void Start()
    {
        audioSource.PlayOneShot(IntroMusic);
    }

    public void StartGame()
    {
        Started = true;
        player.enabled = true;

        Menu.gameObject.SetActive(false);

        audioSource.PlayOneShot(LevelMusic);

        //player.animator.SetTrigger("Run");
    }

    public void CameraFollow(bool enabled)
    {
        cameraFollow.enabled = enabled;
    }
}
