using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class nieruszactegoskryptu : MonoBehaviour, IPointerEnterHandler
{
    CardAlign cAlign;
    public void OnPointerEnter(PointerEventData eventData)
    {
        cAlign.pointerHandler = 100;
    }
    private void Awake()
    {
        cAlign = GameObject.Find("PlayerHand").GetComponent<CardAlign>();
    }


}
