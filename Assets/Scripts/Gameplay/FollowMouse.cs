using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowMouse : MonoBehaviour
{
    public Vector3 mousePosition, worldPosition,viewPortPosition;
    public RectTransform rectPos;
    [SerializeField] CanvasScaler scaler;
    public Enemy en;
    public GameObject[] objs;
    [SerializeField] List<RectTransform> rect = new List<RectTransform>(); 
    // Start is called before the first frame update
    private void Awake()
    {
        objs = GameObject.FindGameObjectsWithTag("Test");
        foreach(GameObject obj in objs)
        {
           rect.Add(obj.GetComponent<RectTransform>());
            
        }
        rect[0].anchoredPosition = new Vector3(0, -400, 0);

    }
    // Update is called once per frame
    void Update()
    {
        float test = 1.0f / objs.Length;
        for (int i = 0; i < objs.Length; i++)
        {
            if(i!=0)
            rect[i].anchoredPosition = Vector3.SlerpUnclamped(rect[0].anchoredPosition, rect[objs.Length - 1].anchoredPosition, test * i);
        }
        
        viewPortPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        rectPos.anchoredPosition = new Vector2(Input.mousePosition.x * scaler.referenceResolution.x / Screen.width, Input.mousePosition.y * scaler.referenceResolution.y / Screen.height);
        rectPos.anchoredPosition -= new Vector2(960, 540);
        rect[rect.Count - 1].anchoredPosition = rectPos.anchoredPosition;
        //enemy.takeddmg
    }

}
