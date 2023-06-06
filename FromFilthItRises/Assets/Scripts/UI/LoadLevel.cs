using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public GameObject panel;
    public int SceneIndex;

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("wah");
            LoadScene(SceneIndex);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("wah");
            LoadScene(SceneIndex);
        }
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadSceneAsync(index);
    }

    //Load from menu
    public void LoadSceneAdditive(int index)
    {
        SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        panel.SetActive(false);
    }
}
