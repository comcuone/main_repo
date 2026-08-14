using UnityEngine;

//게임 전체를 관리하는 스크립트입니다.
public class BossGameManager : MonoBehaviour
{
    public static BossGameManager Instance;

    public bool IsDialogPlaying = false;
    public bool IsPaused = false;


    //GameManager이 없다면 현 오브젝트를 등록하고, 있다면 현 오브젝트를 삭제합니다.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
