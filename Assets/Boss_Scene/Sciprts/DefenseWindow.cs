using System.Collections.Generic;
using TMPro;
using UnityEngine;

//방어창에 방어막을 비우고 추가하거나 갱신합니다.
public class DefenseWindow : MonoBehaviour
{
    public enum DefenseType
    {
        Stack,
        Queue
    }

    [Header("UI")]
    [SerializeField] private TMP_Text defenseTypeText;
    [SerializeField] private TMP_Text defenseListText;

    private DefenseType currentType;

    private Stack<string> stack = new Stack<string>();
    private Queue<string> queue = new Queue<string>();
    [SerializeField] private int maxShieldCount = 3;

    //방어창의 종류를 띄웁니다.
    public void SetDefenseType(DefenseType type)
    {
        currentType = type;

        stack.Clear();
        queue.Clear();

        defenseTypeText.text = "방어창 : " + currentType;

        UpdateUI();
    }

    //방어창의 종류에 맞게 방어막을 순서대로 추가합니다.
    public void AddShield(string shieldName)
    {
        if (ShieldCount() >= maxShieldCount)
        {
            Debug.Log("방어창이 가득 찼습니다.");

            return;
        }

        if (currentType == DefenseType.Stack)
            stack.Push(shieldName);
        else
            queue.Enqueue(shieldName);

        UpdateUI();
    }

    //방어창의 종류에 따라 방어막 하나를 제거하고 반환합니다.
    public string RemoveShield()
    {
        if (currentType == DefenseType.Stack)
        {
            if (stack.Count == 0)
                return null;

            string shield = stack.Pop();

            UpdateUI();

            return shield;
        }
        else
        {
            if (queue.Count == 0)
                return null;

            string shield = queue.Dequeue();

            UpdateUI();

            return shield;
        }
    }

    //현재 나아있는 방어막 목록을 알려줍니다.
    void UpdateUI()
    {
         defenseListText.text = "";

        if (currentType == DefenseType.Stack)
        {
            foreach (string shield in stack)
            {
                if (defenseListText.text != "")
                    defenseListText.text += " > ";

                defenseListText.text += shield;
            }
        }
        else
        {
            foreach (string shield in queue)
            {
                if (defenseListText.text != "")
                    defenseListText.text += " > ";

                defenseListText.text += shield;
            }
        }
    }

    //플레이어가 입력한 키에 맞게 방어창에 방어막을 추가합니다.
    void Update()
    {
        if (BossGameManager.Instance.IsDialogPlaying)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            AddShield("뿌리");

        if (Input.GetKeyDown(KeyCode.Alpha2))
            AddShield("잎");

        if (Input.GetKeyDown(KeyCode.Alpha3))
            AddShield("열매");
    }
    //저장된 방어막의 개수를 반환합니다.
    public int ShieldCount()
    {
        if (currentType == DefenseType.Stack)
            return stack.Count;
        else
            return queue.Count;
    }
    //방어막을 비웁니다.
    public void ClearShields()
    {
        stack.Clear();
        queue.Clear();

        UpdateUI();
    }
    //마지막에 추가한 방어막을 제거합니다.
    public void RemoveLastShield()
    {
        if (ShieldCount() == 0)
            return;

        if (currentType == DefenseType.Stack)
        {
            stack.Pop();
        }
        else
        {
            // Queue는 마지막 요소를 직접 삭제할 수 없으므로
            // 기존 요소들을 임시로 옮긴 뒤 마지막 요소만 제외
            Queue<string> temp = new Queue<string>();

            while (queue.Count > 1)
            {
                temp.Enqueue(queue.Dequeue());
            }

            queue.Dequeue();
            queue = temp;
        }

        UpdateUI();
    }
}