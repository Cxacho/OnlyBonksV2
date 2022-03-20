using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static LTDescr delay;
    public string content;
    public string header;
    public GameObject Tooltip;

    RectTransform tooltip_RectTransoform;

    void Awake()
    {
        tooltip_RectTransoform = Tooltip.GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        delay = LeanTween.delayedCall(0.8f, () =>
        {
            //Debug.Log(this.gameObject.name);
            switch (this.gameObject.name)
            {
                case "Shop_icon" :
                    tooltip_RectTransoform.anchoredPosition = new Vector2(-960f, 376.5f);
                    break;
                case "hp_icon" :
                    tooltip_RectTransoform.anchoredPosition = new Vector2(-621f, 389f);
                    break;
                case "room_count_icon":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(-434.4f, 389f);
                    break;
                case "EndTurnButton":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(-72.3f, 389f);
                    break;
                case "Deck_icon":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(529f, 389f);
                    break;
                case "Map_icon":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(699f, 389f);
                    break;
                case "Settings_icon":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(809.8f, 400.2998f);
                    break;
                case "Mana_icon":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(-876f, -119f);
                    break;
                case "DiscardDeckButton":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(725.7f, -334.9f);
                    break;
                case "DrewDeckButton":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(-960, -334.9f);
                    break;
                default:
                    Debug.Log("error");
                    break;
            }
            TooltipSystem.Show(content, header);
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delay.uniqueId);
        TooltipSystem.Hide();
    }



}
