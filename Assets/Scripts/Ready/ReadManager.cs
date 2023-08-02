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
        GameManager.instance.SetPlayerName("playerA");
        SceneManager.LoadScene("Playing");

    }

    private void OnClickB()
    {
        GameManager.instance.SetPlayerName("playerB");
        SceneManager.LoadScene("Playing");
    }
}
