using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    [SerializeField]
    private Skill[] skills;

    private void Update()
    {
        if (BossGameManager.Instance.IsDialogPlaying)
            return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            skills[0].UseSkill();
        }
    }
}