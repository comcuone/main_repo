using UnityEngine;
using UnityEngine.SceneManagement;

public class nan_LeftWallSceneChange : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어가 아닌 경우 무시
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // 마지막 대화가 끝난 경우에만 씬 이동
        if (nan_DialogueManager.Instance.LastDialogueFinished)
        {
            Debug.Log("마지막 대화 완료 → 다음 씬으로 이동");

            SceneManager.LoadScene(nextSceneName);
        }
    }
}