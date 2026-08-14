using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int storyStep = 0;

    public GameObject LocationPoint;

    void Awake()
    {
        Instance = this;
    }






}