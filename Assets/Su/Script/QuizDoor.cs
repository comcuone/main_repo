using UnityEngine;

public class QuizDoor : MonoBehaviour
{
    [Header("Door")]
    public bool isLeftDoor;

    [Header("Highlight")]
    public GameObject border;

    private bool playerInRange = false;

    private void Start()
    {
        border.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TwoChoiceRoomManager.Instance.SelectDoor(isLeftDoor);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = true;
        border.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = false;
        border.SetActive(false);
    }
}