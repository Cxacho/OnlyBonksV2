using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Threading.Tasks;
public class Backstab : Card
{
    private TextMeshPro textMeshPro;
    [SerializeField] Vector3 offset;
    [SerializeField] private GameObject smoke;
    [SerializeField] private GameObject bleed;
    GameObject smokeHolder;
    GameObject bleedHolder;
    [SerializeField] private float vfxScale;
    List<Transform> characters = new List<Transform>();

    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage, if enemy has their back exposed, deal double damage";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;


    }
    private void OnEnable()
    {
        foreach (Transform obj in gameplayManager.characterCanvas.transform)
        {
            characters.Add(obj);
        }
    }

    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage, if enemy has their back exposed, deal double damage";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage, if enemy has their back exposed, deal double damage";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage, if enemy has their back exposed, deal double damage";
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    public override async void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            base.OnDrop();

            StartCoroutine(ExecuteAfterTime(1f));
            foreach (Enemy en in _enemies)
            {
                if (en.targeted == true)
                {
                    //gameplayManager.OnEnemyKilled += AddMeSomeMana;
                    //dociagnonc mechanike "back exposed " np gdy zabijemy przeciwnika na srodku, zrobic to porprzz sprawdzenie czy istnieje nastepny enemy spawner pos, i czy ma dzieci
                    var getEnemyIndex = en.transform.parent.GetSiblingIndex();
                    /*
                if (en.gameObject == gameplayManager.enemies[gameplayManager.enemies.Count - 1])
                {
                    await DoAnim(en);
                    en.RecieveDamage(attack * 2, this);
                }
                */
                    if (characters[getEnemyIndex + 1].gameObject == gameplayManager.player.gameObject || characters[getEnemyIndex + 1].childCount == 0)
                    {

                        await DoAnim(en);
                        en.RecieveDamage(attack * 2, this);
                    }
                    else
                    {
                        await DoAnim(en);
                        en.RecieveDamage(attack, this);
                    }

                    en.targeted = false;
                }
            }
            resetTargetting();
        }

        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }
    void AddMeSomeMana(object sender, EventArgs e)
    {
        player.mana += 4;
    }
    private void OnDestroy()
    {
        // gameplayManager.OnEnemyKilled -= AddMeSomeMana;   
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        player.manaText.text = player.mana.ToString();

    }
     async Task DoAnim(Enemy _enemy)
    {
        var smokeVFX = Instantiate(smoke, gameplayManager.player.transform.position, Quaternion.identity);
        smokeVFX.transform.SetParent(gameplayManager.vfxCanvas.transform);
        smokeVFX.transform.localScale = new Vector3(vfxScale,vfxScale,vfxScale);
        await Task.Delay(1000);
        var smokeVFX2 = Instantiate(smoke, gameplayManager.player.transform.position, Quaternion.identity);
        smokeVFX2.transform.SetParent(gameplayManager.vfxCanvas.transform);
        smokeVFX2.transform.localScale = new Vector3(vfxScale, vfxScale, vfxScale);
        await Task.Delay(100);
        var oldPos = gameplayManager.player.transform.position;
        gameplayManager.player.transform.position = _enemy.transform.position + offset;
        var rect = gameplayManager.player.GetComponent<RectTransform>();
        var oldRot = rect.rotation.eulerAngles;
        rect.DORotate(Vector3.zero, 0.2f, RotateMode.Fast);
        
        await Task.Delay(400);
        var smokeVFX3 = Instantiate(smoke, gameplayManager.player.transform.position, Quaternion.identity);
        smokeVFX3.transform.SetParent(gameplayManager.vfxCanvas.transform);
        smokeVFX3.transform.localScale = new Vector3(vfxScale, vfxScale, vfxScale);
        await Task.Delay(100);
        gameplayManager.player.transform.position = oldPos;
        rect.DORotate(oldRot, 0.2f, RotateMode.Fast);
        Destroy(smokeVFX,1);
        Destroy(smokeVFX2, 1);
        Destroy(smokeVFX3, 1);
        //characters[getEnemyIndex+1].gameObject!=gameplayManager.player.gameplayManager
        Debug.Log(characters.Count);
    }


}
