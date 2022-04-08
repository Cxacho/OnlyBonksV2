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
    // Start is called before the first frame update
    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        viewPortPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        rectPos.anchoredPosition = new Vector2(Input.mousePosition.x * scaler.referenceResolution.x / Screen.width, Input.mousePosition.y * scaler.referenceResolution.y / Screen.height);
        rectPos.anchoredPosition -= new Vector2(960, 540); 
        //enemy.takeddmg
    }
}
