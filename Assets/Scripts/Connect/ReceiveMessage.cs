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
            string[] substrings = Regex.Split(receivedMessage, "&&");
            for (int i = 0; i < substrings.Length - 1; i++) {
                Debug.Log($"substrings[{i}]" + substrings[i]);
                Message message = JsonConvert.DeserializeObject<Message>(substrings[i]);

                if (message.doing == DoingType.UPDATE_PLAYER) {
                    if (!PlayerManager.playerList.Contains(message.playerData.name)) {
                        GameObject tempObj = Instantiate(playerPrefab, new Vector3(message.playerData.position.x, message.playerData.position.y, message.playerData.position.z), Quaternion.identity);
                        tempObj.name = message.playerData.name;
                        PlayerManager.playerList.Add(message.playerData.name);
                    }
                }
                else if (message.doing == DoingType.UPDATE_DATA) {
                    for (int j = 0; j < PlayerManager.playerList.Count; j++) {
                        GameObject.Find(message.playerData.name).transform.position = new Vector3(message.playerData.position.x, message.playerData.position.y, message.playerData.position.z);
                    }
                }
            }
            receivedMessage = null;
        }
    }
}
