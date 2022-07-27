using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class nieruszactegoskryptu : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]private CardAlign cardAlign;
    public void OnPointerEnter(PointerEventData eventData)
    {
        cardAlign.pointerHandler = 100;
    }
    private void Awake()
    {
        //cardAlign = GameObject.Find("PlayerHand").GetComponent<CardAlign>();
    }


}
