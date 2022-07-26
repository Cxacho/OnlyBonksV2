using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;

    private Story currentStory;

    private bool canContinueToNextLine = false;

    private Coroutine displayLineCoroutine;

    public bool submitButtonPressedThisFrame = false;

    public bool dialogueIsPlaying { get; private set; }

    [Header("Choices UI")]

    [SerializeField] private GameObject[] choices;

    private TextMeshProUGUI[] choicesText;

    private const string SPEAKER_TAG = "speaker";

    private const string PORTRAIT_TAG = "portrait";

    private const string LAYOUT_TAG = "layout";

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance!!!");
        }
        instance = this;

        
    }
    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            submitButtonPressedThisFrame = true;
        }
        if (!dialogueIsPlaying)
        {
            return;
        }
        if (currentStory.currentChoices.Count == 0 
            && canContinueToNextLine
            && submitButtonPressedThisFrame)
        {
            submitButtonPressedThisFrame = false;
            ContinueStory();
        }
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
    }
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        StartCoroutine(Wait());
        submitButtonPressedThisFrame = true;
        Debug.Log("meh?");
        currentStory = new Story(inkJSON.text);
        
        dialogueIsPlaying = true;
        //displayingLine = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }

            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));

            

            HandleTags(currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        

        dialogueText.text = "";

        canContinueToNextLine = false;

        HideChoices();

        foreach(char letter in line.ToCharArray())
        {
            if (submitButtonPressedThisFrame)
            {
                submitButtonPressedThisFrame = false;
                dialogueText.text = line;
               
                break;
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        canContinueToNextLine = true;

        DisplayChoices();
    }

    private void HideChoices()
    {
        foreach(GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if(splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be parsed" + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();


            switch(tagKey)
            {
                case SPEAKER_TAG:
                    Debug.Log("speaker = " + tagValue);
                    displayNameText.text = tagValue;
                    break ;
                case PORTRAIT_TAG:
                    Debug.Log("portrait = " + tagValue);
                    break ;
                case LAYOUT_TAG:
                    Debug.Log("layout = " + tagValue);
                    break ;
                default:
                    Debug.LogWarning("Tag currently not supported " + tag);
                    break;
            }
        }
    }


    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if(currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support!!!");
        }

        int index = 0;
        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for(int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
       // EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }
    public void MakeChoice(int choiceIndex)
    {
        if(canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);

            ContinueStory();

        }

    }
}
