using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class Sanic : Card
{
    [SerializeField] AnimationCurve anCurve;
    [SerializeField] GameObject trail;
    [SerializeField] GameObject sanicRings;
    List<GameObject> distances = new List<GameObject>();
    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {
        //if (player.transform.position.dista)
        if (gameplayManager.state != BattleState.PLAYERTURN)
            return;
        distances.Clear();
        for (int i = 0; i < gameplayManager.enemyBattleStation.Length; i++)
            if (gameplayManager.enemyBattleStation[i].transform.childCount == 0) continue;
            else
            {
                distances.Add(gameplayManager.enemyBattleStation[i].gameObject);
            }
        for (int j = 0; j < distances.Count; j++)
            if (Vector3.Distance(distances[j].transform.position, player.transform.position) < 8)
                Instantiate(sanicRings, player.transform.position, Quaternion.identity);


        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage to every enemy";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage to every enemy";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage to every enemy";
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }

    public override void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
           
            StartCoroutine(ExecuteAfterTime());
            //enable trail
            var sanTrail = Instantiate(trail, player.transform.position, Quaternion.identity);
            sanTrail.transform.SetParent(player.transform);
            player.transform.DOMove(new Vector3(80, player.transform.position.y, player.transform.position.z), 0.4f).SetEase(anCurve).OnComplete(() =>
           {
               foreach (Enemy en in _enemies)
               {
                   en.RecieveDamage(attack, this);

               }
               player.transform.position = new Vector3(-80, player.transform.position.y, player.transform.position.z);
               player.Walk(player.myOriginalPosition);
               sanTrail.transform.SetParent(gameplayManager.canvas.transform);
               Destroy(sanTrail,1f);
               //disbaletrail
           });


            base.OnDrop();

            
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }
    }

    IEnumerator ExecuteAfterTime()
    {
        yield return new WaitForSeconds(0);
       // Instantiate(sanicRings, player.transform.position, Quaternion.identity);
        //yield return new WaitForSeconds(0.2f);
        //Instantiate(sanicRings, player.transform.position, Quaternion.identity);

       // yield return new WaitForSeconds(0.5f);
       // Instantiate(sanicRings, player.transform.position, Quaternion.identity);
        player.manaText.text = player.mana.ToString();





    }

}
