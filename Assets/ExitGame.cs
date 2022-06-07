using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ExitGame : MonoBehaviour
{
    public void BackToMenu()
    {
        //Save player prefs
        SceneManager.LoadScene(0);
    }
}
