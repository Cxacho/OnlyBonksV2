using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class ButtonsController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    RectTransform rect;

    private float startpozX;
    private float startpozY;

    Tweener myTwen;

    public void Start()
    {
        rect = this.gameObject.GetComponent<RectTransform>();
        startpozX = rect.anchoredPosition.x;
        startpozY = rect.anchoredPosition.y;

        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.gameObject.GetComponent<TextMeshProUGUI>().enableVertexGradient = true;
        
        myTwen = rect.DOAnchorPos(new Vector2(startpozX + 50f, startpozY), 0.1f);

        

    }

    public void OnPointerExit(PointerEventData eventData)
    {

        this.gameObject.GetComponent<TextMeshProUGUI>().enableVertexGradient = false;
        
        
        
        rect.DOAnchorPos(new Vector2(startpozX, startpozY), 0.1f);

        

    }
    
}
