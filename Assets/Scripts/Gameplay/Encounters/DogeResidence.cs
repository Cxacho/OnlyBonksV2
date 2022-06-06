using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogeResidence : MonoBehaviour
{
    [SerializeField] GameplayManager gm;
    [SerializeField] Player pl;
    [SerializeField] int value = 60;
    [SerializeField] GameObject mysteryPanel;
    [SerializeField] GameObject relicHolder;
    [SerializeField] EncountersHandler eh;
    // Start is called before the first frame update
    private void Awake()
    {
        gm = FindObjectOfType<GameplayManager>();
        pl = FindObjectOfType<Player>();
        eh = FindObjectOfType<EncountersHandler>();
        relicHolder = GameObject.Find("RelicHolder");
        mysteryPanel = GameObject.Find("MysteryPanel");
    }
    private void Update()
    {
        //naczas dema to pozostaje w updejcie, potem to trzeba do awake wjebac i usunac relic z listy relikow
        if (gm.gold < value)
            gameObject.transform.GetChild(1).GetComponent<Button>().interactable = false;
    }
    public void pay(int amount)
    {
        gm.gold = gm.gold - amount;
        //update text

        gm.treasurePanel.SetActive(true);
        if (gm.treasurePanel.transform.childCount > 0)
        {
            var checkChildCount = gm.treasurePanel.transform.childCount;
            for (int i = 0; i < checkChildCount - 1; i++)
            {
                Destroy(gm.treasurePanel.transform.GetChild(i).gameObject);
            }
            Instantiate(gm.allRelicsList[0], gm.treasurePanel.transform);
            //allRelicsList.RemoveAt(random);
            gm.treasurePanelButton.transform.SetAsLastSibling();
            StartCoroutine(eh.DestroyMe());  
        }
    }
    public void fight()
    {
        
        StartCoroutine(gm.SetupBattle());
        StartCoroutine(eh.DestroyMe());

    }
    public void leave()
    {
        StartCoroutine(eh.DestroyMe());
        gm.gogo();


        
    }
    /*
    IEnumerator DisablePanel()
    {
        yield return new WaitForSeconds(0.5f);
        
        mysteryPanel.SetActive(false);
    }
    */
}
