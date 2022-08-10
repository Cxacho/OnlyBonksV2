using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FollowMouse : MonoBehaviour
{
    public Vector3 mousePosition, worldPosition,viewPortPosition;
    public RectTransform rectPos;
    [SerializeField] CanvasScaler scaler;
    public Enemy en;
    public List<GameObject> objs = new List<GameObject>();
    public List<RectTransform> rect = new List<RectTransform>();
    [SerializeField] float multipiler;
    public List<Vector3> spriteScale = new List<Vector3>();
    public float damping;
    public Card crd;
    bool onStart;
    private void Awake()
    {
        objs.AddRange(GameObject.FindGameObjectsWithTag("Indicator"));
        foreach (GameObject obj in objs)
            rect.Add(obj.GetComponent<RectTransform>());
        rect[0].anchoredPosition = new Vector3(0, -400, 0);
        onStart = true;
    }
    
    void Update()
    {
        float test = 1.0f / objs.Count;
        for (int i = 0; i < objs.Count; i++)
        {
            if (i != 0)
            {
                rect[i].anchoredPosition = Vector3.LerpUnclamped(rect[0].anchoredPosition, rect[objs.Count - 1].anchoredPosition, test * i);
            }
            if (onStart == true)
            {
                spriteScale.Add(new Vector3(i * multipiler, i * multipiler, 1));
                if(spriteScale.Count > objs.Count)
                onStart= false;
            }


        }
        viewPortPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        rectPos.anchoredPosition = new Vector2(Input.mousePosition.x * scaler.referenceResolution.x / Screen.width, Input.mousePosition.y * scaler.referenceResolution.y / Screen.height);
        rectPos.anchoredPosition -= new Vector2(960, 540);
        rect[rect.Count - 1].anchoredPosition = new Vector3(rectPos.anchoredPosition.x, rectPos.anchoredPosition.y, -100); 
    }
}
