using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using System;

public class ShockWave : MonoBehaviour
{
    public float timePeriod=1.5f;
    [SerializeField] GameObject Shockwave;
    List<GameObject> objsToDestroy = new List<GameObject>();
    GameplayManager gm;
    [SerializeField] float multiplyScale, range;
    [SerializeField] Vector3 vfxTargetScale;
    [SerializeField] float heightOffset;
    private async void Awake()
    {
        gm = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        DoAnimateRight();
        DoAnimateLeft();
    }

    /*
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
    */
    async Task DoAnimateRight()
    {
        for (int i = 1; i < 4; i++)
        {

            var obj = Instantiate(Shockwave, new Vector3(gm.player.transform.position.x + i * range, gm.player.transform.position.y + heightOffset, 1), Quaternion.identity);
            objsToDestroy.Add(obj);
            obj.transform.SetParent(gm.vfxCanvas.transform);
            obj.transform.localScale = vfxTargetScale;
            obj.transform.localScale = new Vector3(obj.transform.localScale.x * (multiplyScale - (Mathf.Abs(i) * 0.1f * 2)), obj.transform.localScale.y * (multiplyScale - (Mathf.Abs(i) * 0.1f) * 2), 1);
            await Task.Delay(500);
        }
        await Task.Delay(1500);
        foreach (GameObject obj in objsToDestroy)
        {
            Destroy(obj);
        }
        Destroy(this.gameObject);
    }
    async Task DoAnimateLeft()
    {
        for (int i = -1; i > -4; i--)
        {

            var obj = Instantiate(Shockwave, new Vector3(gm.player.transform.position.x + i * range, gm.player.transform.position.y +heightOffset, 1), Quaternion.identity);
            //
            objsToDestroy.Add(obj);
            obj.transform.SetParent(gm.vfxCanvas.transform);
            obj.transform.localScale = vfxTargetScale;
            obj.transform.localScale = new Vector3(obj.transform.localScale.x *(multiplyScale-(Mathf.Abs(i)*0.1f * 2)), obj.transform.localScale.y * (multiplyScale - (Mathf.Abs(i) * 0.1f * 2)), 1);
            await Task.Delay(500);

        }
        await Task.Delay(1500);
        foreach (GameObject obj in objsToDestroy)
        {
            Destroy(obj);
        }
        Destroy(this.gameObject);
    }
}
