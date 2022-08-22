using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Threading.Tasks;
using UnityEngine.VFX;
public class Spit : Card
{
    [SerializeField] private GameObject spitVFX;
    [SerializeField] private GameObject spitBurstVFX, splashVFX;
    private TextMeshPro textMeshPro;
    int height;
    VisualEffect VFX;

    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage";
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
                    await DoAnim(en);
                    //gameplayManager.OnEnemyKilled += AddMeSomeMana;
                    //set debuff??
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

    async Task DoAnim(Enemy en)
    {
        var spit=Instantiate(spitVFX, player.gameObject.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        VFX = spit.GetComponent<VisualEffect>();
        var index = en.transform.parent.GetSiblingIndex();
        if (index == 0)
            height = 5;
        else if (index == 1)
            height = 6;
        else
        {
            height = 7;
        }
        VFX.SetVector3("minVelocity", new Vector3(6, height, 0));
        VFX.SetVector3("maxVelocity", new Vector3(6, height-1, 0));
        Destroy(spit, 3f);
        await Task.Delay(1000);
        var spit1 = Instantiate(spitBurstVFX, en.transform.parent.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var splash = Instantiate(splashVFX, en.transform.parent.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
    }

}
