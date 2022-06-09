using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
public class catJam : MonoBehaviour
{
    public void OpenUrl()
    {
        string url = "https://www.youtube.com/watch?v=2V9HkdBgq10";
        Process pro = new Process();
        pro.StartInfo.FileName = "chrome.exe";
        pro.StartInfo.Arguments = url;
        pro.Start();
    }

    
}
