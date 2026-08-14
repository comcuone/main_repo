using UnityEngine;

public class TriggerPoint : MonoBehaviour
{
    public int eventNumber;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        DialogueManager.Instance.StartEvent(eventNumber);

        gameObject.SetActive(false);
    }
}