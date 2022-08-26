using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cut : MonoBehaviour
{
    RectTransform rect;
    [SerializeField] Vector2 offset = new Vector2(-125, -125);
    [SerializeField] Vector2 moveOffset = new Vector2(125, 125);
    [SerializeField] float animTime = 1;
    // Start is called before the first frame update
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x + offset.x, rect.anchoredPosition.y + offset.y);
        rect.DOAnchorPos(new Vector2(moveOffset.x + rect.anchoredPosition.x, moveOffset.y + rect.anchoredPosition.y), animTime);
    }

}
