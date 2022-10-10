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
        Tooltip = GameObject.FindGameObjectWithTag("ToolTip");
        tooltip_RectTransoform = Tooltip.GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        delay = LeanTween.delayedCall(0.8f, () =>
        {
            
            switch (this.gameObject.name)
            {
                case "Shop_icon" :
                    tooltip_RectTransoform.anchoredPosition = new Vector2(-710f, 418);
                    break;
                case "hp_icon" :
                    tooltip_RectTransoform.anchoredPosition = new Vector2(-470.0f, 431f);
                    break;
                case "roomCount":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(-226, 430);
                    break;
                case "roomImage":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(-226, 430);
                    break;
                case "EndTurn_Button":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(495, -467);
                    break;case "endImage":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(495, -467);
                    break;
                case "Deck_icon":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(681.6f, 431f);
                    break;
                case "Map_icon":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(791, 431f);
                    break;
                case "Settings_icon":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(851.68f, 431f);
                    break;
                case "settingsImage":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(851.68f, 431f);
                    break;
                case "Mana":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(-804, -177);
                    break;
                case "Discard_Deck_Button":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(495, -467);
                    break;
                case "Draw_Deck_Button":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(-482, -468);
                    break;
                case "Swap_Weapon_Button":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(-482, -468);
                    break;
                case "StrengthBuffIndcator(Clone)":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(-527, -25);
                    break;
                case "DexterityIndicator(Clone)":
                    tooltip_RectTransoform.anchoredPosition = new Vector2(-527, -25);
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
