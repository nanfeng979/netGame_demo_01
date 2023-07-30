using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private TcpClient client;
    private StreamReader reader;
    private StreamWriter writer;

    private void Start()
    {
        client = new TcpClient("localhost", 1234); // 服务器的IP地址和端口号
        NetworkStream stream = client.GetStream();
        reader = new StreamReader(stream);
        writer = new StreamWriter(stream);

        StartCoroutine(ReceivePlayerUpdates());
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        transform.Translate(moveHorizontal * moveSpeed * Time.deltaTime, 0f, moveVertical * moveSpeed * Time.deltaTime);

        SendPlayerPosition(transform.position.x, transform.position.z);
    }

    private IEnumerator ReceivePlayerUpdates()
    {
        while (true)
        {
            try
            {
                string message = reader.ReadLine();
                if (!string.IsNullOrEmpty(message))
                {
                    string[] playerData = message.Split(',');

                    if (playerData.Length >= 3)
                    {
                        string colorString = playerData[0];
                        float posX = float.Parse(playerData[1]);
                        float posZ = float.Parse(playerData[2]);

                        UpdateOtherPlayerPosition(colorString, posX, posZ);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error receiving player updates: " + e.Message);
                break;
            }

            yield return null;
        }
    }

    private void SendPlayerPosition(float posX, float posZ)
    {
        string message = $"{posX},{posZ}";
        writer.WriteLine(message);
        writer.Flush();
    }

    private void UpdateOtherPlayerPosition(string colorString, float posX, float posZ)
    {
        // 在此处更新其他玩家的位置和颜色
        // ...
    }

    private void OnDestroy()
    {
        writer.Close();
        reader.Close();
        client.Close();
    }
}
