using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }


    public void Pause()
    {
        isPaused = true;

        BossGameManager.Instance.IsPaused = true;

        pausePanel.SetActive(true);

        Time.timeScale = 0f;
    }


    public void Resume()
    {
        isPaused = false;

        BossGameManager.Instance.IsPaused = false;

        pausePanel.SetActive(false);

        Time.timeScale = 1f;
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}