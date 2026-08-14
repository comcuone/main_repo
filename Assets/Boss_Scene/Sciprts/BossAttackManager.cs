using System.Collections;
using UnityEngine;
using TMPro;

public class BossAttackManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private BossHealth bossHealth;
    [Header("UI")]
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private GameObject defensePanel;
    [SerializeField] private TMP_Text attackOrderText;
    [SerializeField] private TMP_Text defenseTypeText;
    [SerializeField] private TMP_Text enterText;
    [SerializeField] private TMP_Text defenseListText;
    [SerializeField] private GameObject textPanel;

    [Header("Defense")]
    [SerializeField] private DefenseWindow defenseWindow;

    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text resultText;
    //쿨타임
    [Header("Attack")]
    [SerializeField] private float attackCooldown = 50f;



    // 방어창 종류
    private string[] defenseTypes =
    {
        "스택",
        "큐"
    };

    private string[] currentOrder;
    private string currentDefense;

    public string[] CurrentOrder => currentOrder;
    public string CurrentDefense => currentDefense;

    void Start()
    {
        defensePanel.SetActive(false);
        warningPanel.SetActive(false);
        attackText.gameObject.SetActive(false);
        resultText.gameObject.SetActive(false);
        textPanel.SetActive(false);

        StartCoroutine(BattleLoop());
    }

   IEnumerator BattleLoop()
    {
        // 대화가 끝날 때까지 기다림
        while (BossGameManager.Instance.IsDialogPlaying)
            yield return null;


        while (true)
        {
            // 사망 체크
            if (playerHealth.IsDead || bossHealth.IsDead)
            {
                defensePanel.SetActive(false);
                warningPanel.SetActive(false);
                attackText.gameObject.SetActive(false);
                resultText.gameObject.SetActive(false);

                yield break;
            }

            // ★ 다음 공격까지 대기 (Inspector에서 설정한 시간)
            yield return new WaitForSecondsRealtime(attackCooldown);

            GenerateWarning();
            defensePanel.SetActive(true);
            warningPanel.SetActive(true);

            Time.timeScale = 0f;

            enterText.text = "1-뿌리, 2-잎, 3-열매, Backspace를 눌러 되돌리기/Enter를 눌러 완료하기";

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                if (playerHealth.IsDead || bossHealth.IsDead)
                {
                    warningPanel.SetActive(false);
                    defensePanel.SetActive(false);
                    yield break;
                }
                // Backspace를 누르면 마지막 방어막 되돌리기
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    defenseWindow.RemoveLastShield();
                }

                yield return null;
            }

            Time.timeScale = 1f;

            warningPanel.SetActive(false);
            defensePanel.SetActive(false);

            yield return StartCoroutine(AttackRoutine());
        }
    }
    void GenerateWarning()
    {
        currentOrder = GetRandomOrder();

        currentDefense = defenseTypes[Random.Range(0, defenseTypes.Length)];

        if (currentDefense == "스택")
            defenseWindow.SetDefenseType(DefenseWindow.DefenseType.Stack);
        else
            defenseWindow.SetDefenseType(DefenseWindow.DefenseType.Queue);

        attackOrderText.text =
            currentOrder[0] + " → " + currentOrder[1] + " → " +currentOrder[2];

        defenseTypeText.text =
            "방어창의 종류: " + currentDefense;
    }

    string[] GetRandomOrder()
    {
        string[] temp =
        {
            "뿌리",
            "열매",
            "잎"
        };

        for (int i = 0; i < temp.Length; i++)
        {
            int rand = Random.Range(i, temp.Length);

            string save = temp[i];
            temp[i] = temp[rand];
            temp[rand] = save;
        }

        return temp;
    }
    IEnumerator AttackRoutine()
    {
        //얘필요없대?
        // isAttacking = true;

        for (int i = 0; i < currentOrder.Length; i++)
        {
            if (playerHealth.IsDead || bossHealth.IsDead)
            {
                attackText.gameObject.SetActive(false);
                resultText.gameObject.SetActive(false);

                yield break;
            }
            if (defenseWindow.ShieldCount() == 0)
            {
                Debug.Log("보호막 부족 - 자동 방어 실패");
                playerHealth.TakeDamage(50);

                yield return new WaitForSeconds(1f);

                continue;
            }

            string shield = defenseWindow.RemoveShield();

            textPanel.SetActive(true);
            attackText.gameObject.SetActive(true);
            attackText.text = "보스 공격 : " + currentOrder[i];

            yield return new WaitForSeconds(0.7f);

            attackText.gameObject.SetActive(false);
            textPanel.SetActive(false);

            if (shield == currentOrder[i])
            {
                bossHealth.TakeDamage(20);
                textPanel.SetActive(true);
                resultText.gameObject.SetActive(true);
                resultText.text = "방어 성공!\n보스 HP -20";
            }
            else
            {
                playerHealth.TakeDamage(50);
                textPanel.SetActive(true);
                resultText.gameObject.SetActive(true);
                resultText.color = Color.red;
                resultText.text = "방어 실패!\n플레이어 HP -50";
            }

            yield return new WaitForSeconds(0.8f);

            resultText.gameObject.SetActive(false);
            textPanel.SetActive(false);
        }
        defenseWindow.ClearShields();

        // isAttacking = false;
    }
}