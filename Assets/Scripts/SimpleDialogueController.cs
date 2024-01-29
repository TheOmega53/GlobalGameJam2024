using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI[] dialogueTexts; // Array of Text elements representing different dialogues
    public float typingSpeed = 0.05f; // Speed at which each character is displayed
    public UnityEvent endOfDialogueEvent; //What happens after Dialogue ends
    public GameObject currentDialoguePanel; //Where the dialogue is being shown

    private string currentMessage = ""; // Complete message of the current dialogue
    private int currentDialogueIndex = 0; // Index of the current dialogue
    private bool isTyping = false; // Flag to check if the typing coroutine is currently active


    public string[] jokeBank;

    // Start is called before the first frame update
    void Start()
    {
        currentDialoguePanel = dialogueTexts[currentDialogueIndex].transform.parent.gameObject;
        dialogueTexts[0].text = jokeBank[UnityEngine.Random.Range(0, jokeBank.Length)];
        ShowCurrentDialogue();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID
// Check for user input to progress to the next dialogue
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {

            if (!isTyping)
            {

                // Hide previous dialoguebox if not same
                currentDialoguePanel = dialogueTexts[currentDialogueIndex].transform.parent.gameObject;
                
                currentDialoguePanel.SetActive(false);


                // If typing coroutine is not active, go to the next dialogue
                currentDialogueIndex++;

                // Check if there are more dialogues
                if (currentDialogueIndex < dialogueTexts.Length)
                {
                    ShowCurrentDialogue();
                }
                else
                {
                    // All dialogues displayed, you can add additional logic here
                    endOfDialogueEvent.Invoke();
                }
            }
            else
            {
                // If typing coroutine is active, complete it instantly
                StopAllCoroutines();
                isTyping = false;
                dialogueTexts[currentDialogueIndex].text = currentMessage;
            }
        }
#else
        // Check for user input to progress to the next dialogue
        if (Input.GetKeyDown(KeyCode.Return))
        {

            if (!isTyping)
            {

                // Hide previous dialoguebox if not same
                currentDialoguePanel = dialogueTexts[currentDialogueIndex].transform.parent.gameObject;
                
                currentDialoguePanel.SetActive(false);


                // If typing coroutine is not active, go to the next dialogue
                currentDialogueIndex++;

                // Check if there are more dialogues
                if (currentDialogueIndex < dialogueTexts.Length)
                {
                    ShowCurrentDialogue();
                }
                else
                {
                    // All dialogues displayed, you can add additional logic here
                    endOfDialogueEvent.Invoke();
                }
            }
            else
            {
                // If typing coroutine is active, complete it instantly
                StopAllCoroutines();
                isTyping = false;
                dialogueTexts[currentDialogueIndex].text = currentMessage;
            }
        }
#endif
    }

    // Coroutine to gradually display the message
    IEnumerator TypeMessage(string message)
    {
        isTyping = true;
        dialogueTexts[currentDialogueIndex].text = ""; // Clear existing text

        foreach (char letter in message)
        {
            dialogueTexts[currentDialogueIndex].text += letter; // Append each character to the text
            yield return new WaitForSeconds(typingSpeed); // Wait for the specified time before displaying the next character
        }

        isTyping = false;
    }

    // Display the current dialogue
    void ShowCurrentDialogue()
    {
        // Enable the dialogue's parent (if not enabled)
        currentDialoguePanel = dialogueTexts[currentDialogueIndex].transform.parent.gameObject;
        currentDialoguePanel.SetActive(true);

        currentMessage = dialogueTexts[currentDialogueIndex].text; // Store the complete message        
        dialogueTexts[currentDialogueIndex].text = ""; // Clear existing text
        StartCoroutine(TypeMessage(currentMessage));
    }

    // Get the complete message instantly
    string GetCompleteMessage()
    {
        return dialogueTexts[currentDialogueIndex].text;
    }
}
