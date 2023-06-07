using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public bool paused = false;
    [SerializeField] StarterAssetsInputs StarterAssetInput;
    public Player player;

    private void Start()
    {
        StarterAssetInput = player.GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (paused)
                Resume();
            else
                Pause();
            paused = !paused;
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        StarterAssetInput.cursorLocked = false;
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        StarterAssetInput.cursorLocked = true;
    }
    public void Home(int sceneId)
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(sceneId);
    }
}