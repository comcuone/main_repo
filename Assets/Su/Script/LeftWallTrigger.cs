using UnityEngine;
using UnityEngine.SceneManagement;

public class LeftWallTrigger : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // storyStep이 5일 때만 씬 이동
        if (GameManager.Instance.storyStep == 5)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}