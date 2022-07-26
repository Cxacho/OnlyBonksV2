using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink Json")]
    [SerializeField] private TextAsset inkJson;

    
    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        if(playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            
            visualCue.SetActive(true);

            if(Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Wait());

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    
                    if(hit.transform.tag == "NPCDialogue")
                    {
                        StartCoroutine(Wait());
                        DialogueManager.GetInstance().EnterDialogueMode(inkJson);
                    }
                }
            }
        }
        else
        {
            visualCue.SetActive(false);
            
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
