using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static string currentPlayerName;

    public static List<string> playerList = new List<string>();

    public static Vector3 currentPlayerPosition;
    public static Vector3 currentPlayerRotation;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public static void SetCurrentPlayerName(string name)
    {
        currentPlayerName = name;
    }

    public static string GetCurrentPlayerName()
    {
        return currentPlayerName;
    }
}
