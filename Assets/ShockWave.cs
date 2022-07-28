using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShockWave : MonoBehaviour
{
    public float timePeriod=1.5f;
    [SerializeField] GameObject Shockwave;
    GameplayManager gm;
    int i = 2;
    private void Awake()
    {
        gm = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        StartCoroutine(SpawnWave());
    }
    IEnumerator SpawnWave()
    {
        var sh1 = Instantiate(Shockwave, new Vector3(gm.player.transform.position.x +i*10,gm.player.transform.position.y,1), Quaternion.identity);
        i++;
        yield return new WaitForSeconds(timePeriod);
        var sh2=Instantiate(Shockwave, new Vector3(gm.player.transform.position.x + i * 10, gm.player.transform.position.y, 1), Quaternion.identity);
        i++;
        //sh2.transform.DOMove(new Vector3(sh2.transform.position.x + 81, sh2.transform.position.y, 0), timePeriod);
        yield return new WaitForSeconds(timePeriod);
        var sh3 = Instantiate(Shockwave, new Vector3(gm.player.transform.position.x + i * 10, gm.player.transform.position.y, 1), Quaternion.identity);
        i++;
        yield return new WaitForSeconds(timePeriod);
        var sh4 = Instantiate(Shockwave, new Vector3(gm.player.transform.position.x + i * 10, gm.player.transform.position.y, 1), Quaternion.identity);
        i++;
        yield return new WaitForSeconds(timePeriod);
        var sh5 = Instantiate(Shockwave, new Vector3(gm.player.transform.position.x + i * 10, gm.player.transform.position.y, 1), Quaternion.identity);
        i++;
        //sh2.transform.DOMove(new Vector3(sh2.transform.position.x + 81, sh2.transform.position.y, 0), timePeriod);
        yield return new WaitForSeconds(timePeriod);
        var sh6 = Instantiate(Shockwave, new Vector3(gm.player.transform.position.x + i * 10, gm.player.transform.position.y, 1), Quaternion.identity);
        i++;
        //sh3.transform.DOMove(new Vector3(sh3.transform.position.x + 81, sh3.transform.position.y, 0), timePeriod);
        //Destroy(sh3, timePeriod);
        //sh3.transform.DOMove(new Vector3(sh3.transform.position.x - 80, sh3.transform.position.y, 0), 0.5f);
        Destroy(this.gameObject);
    }
}
