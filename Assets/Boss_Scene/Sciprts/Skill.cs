using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 부메랑의 쿨타임과 발동을 제어합니다.
public class Skill : MonoBehaviour
{
    [SerializeField] private string skillName;
    [SerializeField] private float maxCooldownTime = 3f;

    [SerializeField] private TextMeshProUGUI textCooldownTime;
    [SerializeField] private Image imageCooldownTime;

    [SerializeField] private Boomerang boomerang;

    private float currentCooldownTime;
    private bool isCooldown;

    //쿨타임 상태를 끕니다.
    private void Awake()
    {
        SetCooldownIs(false);
    }

    //대화가 진행 중일 때를 제외하고 E키를 입력받으면 스킬함수를 불러옵니다.
    private void Update()
    {
        if (BossGameManager.Instance.IsDialogPlaying)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            UseSkill();
        }
    }

    //쿨타임이 아니라면 부메랑을 날리고 쿨타임을 맥스로 설정합니다.
    public void UseSkill()
    {
        if (isCooldown)
        {
            return;
        }

        boomerang.Fire();

        StartCoroutine(OnCooldownTime(maxCooldownTime));
    }

    //쿨타임을 진행시키고 남은 시간을 띄웁니다.
    private IEnumerator OnCooldownTime(float cooldown)
    {
        currentCooldownTime = cooldown;

        SetCooldownIs(true);

        while (currentCooldownTime > 0)
        {
            currentCooldownTime -= Time.deltaTime;

            imageCooldownTime.fillAmount = currentCooldownTime / cooldown;
            textCooldownTime.text = currentCooldownTime.ToString("F1");

            yield return null;
        }

        SetCooldownIs(false);
    }

    //쿨타임 상태를 변경합니다.
    private void SetCooldownIs(bool value)
    {
        isCooldown = value;

        textCooldownTime.enabled = value;
        imageCooldownTime.enabled = value;
    }
}