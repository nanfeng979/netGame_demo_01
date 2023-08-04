using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReadManager : MonoBehaviour
{
    [SerializeField]
    private Image playerA;
    private Button buttonA;
    [SerializeField]
    private Image playerB;
    private Button buttonB;

    void Start()
    {
        buttonA = playerA.gameObject.AddComponent<Button>();
        buttonA.onClick.AddListener(OnClickA);
        buttonB = playerB.gameObject.AddComponent<Button>();
        buttonB.onClick.AddListener(OnClickB);
    }

    void Update()
    {
        
    }

    private void OnClickA()
    {
        SetCurrentPlayer("PlayerA");
        SceneManager.LoadScene("Playing");

    }

    private void OnClickB()
    {
        SetCurrentPlayer("PlayerB");
        SceneManager.LoadScene("Playing");
    }

    private void SetCurrentPlayer(string player) {
        PlayerManager.SetCurrentPlayerName(player);
        PlayerManager.playerList.Add(player);
        Debug.Log(PlayerManager.GetCurrentPlayerName());
    }
}
