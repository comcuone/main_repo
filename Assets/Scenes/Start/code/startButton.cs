using UnityEngine;
using UnityEngine.SceneManagement;

public class startButton : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("jan");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
}
