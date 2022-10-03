using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class ShieldBuddy : Card
{
    public int armor;
    [SerializeField] private GameObject shieldBuddyVFX,shieldProjectile,particleVFX;
    [SerializeField] private Vector3  spawnOffset;
    [SerializeField] private float shakeStrength,shakeTime;
    [SerializeField] int shakeVib;
    List<GameObject> vfxses = new List<GameObject>();
    GameObject buddy,particles;
    int getArmor;

    public override async void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {



            player.manaText.text = player.mana.ToString();
                gameplayManager.OnCardPlayedDetail -= applyEffect;
            base.OnDrop();
            await OnPlayAnim();
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }
    private void OnEnable()
    {
        DoAnim();
        gameplayManager.OnCardPlayedDetail += applyEffect;
    }
    async Task OnPlayAnim()
    {
        var getRect=buddy.GetComponent<RectTransform>();
        getRect.DOAnchorPos(player.myOriginalPosition, 0.7f).OnComplete(()=>
        {
            player.GetArmor(armor);
        });
    }
    async Task DoAnim()
    {
        buddy = Instantiate(shieldBuddyVFX, this.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform).gameObject;
        var buddyRect=buddy.GetComponent<RectTransform>();
        buddyRect.DOAnchorPos(player.GetComponent<RectTransform>().anchoredPosition3D + spawnOffset,1);
        var wingTrans1 = buddy.transform.GetChild(0).GetComponent<Transform>();
        var wingTrans2 = buddy.transform.GetChild(1).GetComponent<Transform>();
        wingTrans1.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -20));
        wingTrans2.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 20));
        wingTrans1.DORotate(new Vector3(0, 0, 20), 0.4f, RotateMode.Fast).SetLoops(-1, LoopType.Yoyo);
        wingTrans2.DORotate(new Vector3(0, 0, -20), 0.4f, RotateMode.Fast).SetLoops(-1, LoopType.Yoyo);
        await Task.Delay(1000);
        particles = Instantiate(particleVFX, buddy.gameObject.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        buddyRect.DOShakeAnchorPos(shakeTime, shakeStrength,shakeVib).SetLoops(-1, LoopType.Yoyo);


    }
    void applyEffect(Card card,float dam,int c)
    {
        getArmor =  c + 1;
        //oncardplay
        var proj = Instantiate(shieldProjectile, buddy.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var rect=proj.GetComponent<RectTransform>();
        rect.DOAnchorPos(player.GetComponent<RectTransform>().anchoredPosition,1f).OnComplete(()=>
        {
            //add armor
            player.GetArmor(getArmor);
            Destroy(proj);
        });
                    
    }
    private void OnDestroy()
    {
        Debug.Log(buddy.name);
        Destroy(buddy.gameObject);
        Destroy(particles.gameObject);
        if(gameplayManager.OnCardPlayedDetail!=null)
        gameplayManager.OnCardPlayedDetail -= applyEffect;

    }

}
