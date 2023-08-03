using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

public class ReceiveMessage : MonoBehaviour
{
    public static string receivedMessage;

    [SerializeField]
    private GameObject playerPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        if (receivedMessage != null) {
            Debug.Log(receivedMessage);
            string[] substrings = Regex.Split(receivedMessage, "&&");
            for (int i = 0; i < substrings.Length - 1; i++) {
                Message message = JsonConvert.DeserializeObject<Message>(substrings[i]);

                if (message.doing == DoingType.UPDATE_PLAYER) {
                    if (!GameManager.instance.playerList.Contains(message.playerData.name)) {
                        Debug.Log("开始生成");
                        Instantiate(playerPrefab, new Vector3(message.playerData.position.x, message.playerData.position.y, message.playerData.position.z), Quaternion.identity);
                        GameManager.instance.playerList.Add(message.playerData.name);
                    }
                }
            }
            
            receivedMessage = null;
        }
    }
}
