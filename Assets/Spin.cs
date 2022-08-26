using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spin : MonoBehaviour
{
    [SerializeField] float scaleValue,time;
    // Start is called before the first frame update
    private void Awake()
    {
        transform.DORotate(new Vector3(0, 0, 360), 1.2f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        
    }
    private void OnEnable()
    {
        //transform.DOScaleX(scaleValue, time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
