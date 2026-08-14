using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

//게임의 진행에 맞는 대화, 화면을 재생합니다.
public class BossDialogTest : MonoBehaviour
{
    [SerializeField]
    private BossDialogSystem dialogSystem01;
    [SerializeField]
    private TextMeshProUGUI textCountdown;
    [SerializeField]
    private BossDialogSystem dialogSystemWin;
    [SerializeField]
    private BossDialogSystem dialogSystemFail;

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private BossHealth bossHealth;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject endingPanel;

    //대화 진행 중에는 게임을 멈추고 대화를 이어나갑니다.
    private IEnumerator Start()
    {
        BossGameManager.Instance.IsDialogPlaying = true;
        Time.timeScale = 0f;

        textCountdown.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(3);

        // 첫 대사
        yield return new WaitUntil(() => dialogSystem01.UpdateDialog());


        // 전투 시작 전 카운트다운
        textCountdown.gameObject.SetActive(true);

        int count = 3;

        while (count > 0)
        {

            textCountdown.text = count.ToString();
            count--;

            yield return new WaitForSecondsRealtime(1);
        }

        textCountdown.gameObject.SetActive(false);


        // 진짜 전투 시작
        BossGameManager.Instance.IsDialogPlaying = false;
        Time.timeScale = 1f;

        StartCoroutine(CheckGameEnd());
        //이제 전투 진행        
    }
    //플레이어와 보스의 체력을 받아 Win,Fail등의 대화를 출력합니다.
    private IEnumerator CheckGameEnd()
    {
        while (true)
        {
            if (playerHealth.IsDead)
            {
                BossGameManager.Instance.IsDialogPlaying = true;
                Time.timeScale = 0f;

                yield return new WaitUntil(() => dialogSystemFail.UpdateDialog());

                ShowGameOverPanel();

                yield break;
            }

            if (bossHealth.IsDead)
            {
                BossGameManager.Instance.IsDialogPlaying = true;
                Time.timeScale = 0f;

                yield return new WaitUntil(() => dialogSystemWin.UpdateDialog());

                ShowEndingPanel();

                yield break;
            }

            yield return null;
        }
    }
    //게임오버 시 게임을 멈추고 게임오버 화면을 재생합니다.
    void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        BossGameManager.Instance.IsPaused = true;
        Time.timeScale = 0f;
    }

    //보스 처치 시 게임을 멈추고 엔딩 화면을 재생합니다.
    void ShowEndingPanel()
    {
        endingPanel.SetActive(true);
        BossGameManager.Instance.IsPaused = true;
        Time.timeScale = 0f;
    }
    //다시 시작하는 함수입니다.
    public void PayAgain()
    {
        //보스방부터 다시 시작
        SceneManager.LoadScene("BossScene");
    }
}
