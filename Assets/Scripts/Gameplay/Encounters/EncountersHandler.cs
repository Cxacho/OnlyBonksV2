using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncountersHandler : MonoBehaviour
{
public IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject.transform.GetChild(0).gameObject);
        gameObject.SetActive(false);
    }
}
