using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] float time;
    // Start is called before the first frame update
    void Awake()
    {
        Destroy(this.gameObject, time);
    }

}
