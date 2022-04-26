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
    [SerializeField] float multipiler;

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
    IEnumerable<Vector3> EvaluateSlerpPoints(Vector3 start,Vector3 end,float centerOffset)
    {
        var centerPivot = (start + end) * 0.5f;
        centerPivot -= new Vector3(0, -centerOffset);
        var startRelativeCenter = start - centerPivot;
        var endRelativeCenter = end - centerPivot;
        var f = 1f / 10;
        for(var i = 0f;i<1+f;i+=f)
        {
            yield return Vector3.Slerp(startRelativeCenter, endRelativeCenter, i) + centerPivot;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        float test = 1.0f / objs.Length;
        for (int i = 0; i < objs.Length; i++)
        {
            if(i!=0)
            rect[i].anchoredPosition = Vector3.LerpUnclamped(rect[0].anchoredPosition, rect[objs.Length - 1].anchoredPosition, test * i);
            rect[i].localScale = new Vector3(i * multipiler, i * multipiler, 1);
        }
        
        
        
        viewPortPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        rectPos.anchoredPosition = new Vector2(Input.mousePosition.x * scaler.referenceResolution.x / Screen.width, Input.mousePosition.y * scaler.referenceResolution.y / Screen.height);
        rectPos.anchoredPosition -= new Vector2(960, 540);
        rect[rect.Count - 1].anchoredPosition = rectPos.anchoredPosition;
        //enemy.takeddmg
    }

}
