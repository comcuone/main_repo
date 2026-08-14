using UnityEngine;
using UnityEngine.SceneManagement;

public class SungShin_LeftWallTrigger : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("LeftWall 충돌 발생 : " + collision.gameObject.name);
        if (!collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player 태그가 아닙니다");
            return;
        }
        
        Debug.Log("현재 : currentID = " + Sungshin_DialogueManager.Instance.currentID);
        if ( Sungshin_DialogueManager.Instance.currentID == 1500)
        {
            Debug.Log("1500 확인 씬 이동");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}