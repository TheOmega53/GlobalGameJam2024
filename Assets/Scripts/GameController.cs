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
        if (Input.GetKeyDown(KeyCode.Return))
        {

        }
    }


    public void StartGame()
    {
        Started = true;
        player.enabled = true;

        Menu.gameObject.SetActive(false);
    }

    public void CameraFollow(bool enabled)
    {
        cameraFollow.enabled = enabled;
    }
}
