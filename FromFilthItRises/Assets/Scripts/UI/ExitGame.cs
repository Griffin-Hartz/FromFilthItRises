using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void OnApplicationQuit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
