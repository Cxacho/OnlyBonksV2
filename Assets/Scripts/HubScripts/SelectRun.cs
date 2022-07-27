using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectRun : MonoBehaviour
{
    public void ChooseRun(int runNumber)
    {
        switch (runNumber)
        {
            case 0:
                SceneManager.LoadScene(1);
                break;
            case 1:
                SceneManager.LoadScene(1);
                break;
            case 2:
                SceneManager.LoadScene(1);
                break;
            default:
                break;
        }
    }
}
