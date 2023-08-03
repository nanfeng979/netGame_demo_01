using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

public class SocketClient : MonoBehaviour
{
    private const string serverIP = "127.0.0.1"; // 服务端IP地址
    private const int serverPort = 8888;        // 服务端监听的端口号
    private Socket clientSocket;
    private byte[] receiveBuffer = new byte[1024];

    [SerializeField]
    private GameObject player;

    private float timer = 1.0f;
    private float cd = 0.0f;

    private void Start()
    {
        ConnectToServer();
    }

    private void Update()
    {
        // 1s执行一次Update_player()
        cd += Time.deltaTime;
        if(cd > timer) {
            cd = 0;
            Update_player();
        }
        // Invoke(nameof(Update_player), 3.0f);
    }

    private void Update_player()
    {
        PlayerData playerData = new PlayerData(PlayerDataType.ADD_PLAYER, "", null);
        Message message = new Message(MessageType.Broadcast, DoingType.UPDATE_PLAYER, playerData, GameManager.instance.GetPlayerName(), 0);
        SendDataToServer(message);
    }

    private void OnDestroy() {
        // 发送断开消息
        PlayerData playerData = new PlayerData(PlayerDataType.REMOVE_PLAYER, "", null);
        Message message = new Message(MessageType.Broadcast, DoingType.REMOVE_PLAYER, playerData, GameManager.instance.GetPlayerName(), 0);
        SendDataToServer(message);
        clientSocket.Close();
    }

    private void ConnectToServer()
    {
        try
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.BeginConnect(IPAddress.Parse(serverIP), serverPort, ConnectCallback, clientSocket);
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to server: " + e.Message);
        }
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            Debug.Log("Connected to server.");

            // 发送连接消息
            float positionX = 0;
            float positionY = 0;
            float positionZ = 0;
            PlayerData playerData = new PlayerData(PlayerDataType.ADD_PLAYER, GameManager.instance.GetPlayerName(), new myVector3(positionX, positionY, positionZ));
            Message message = new Message(MessageType.Broadcast, DoingType.ADD_PLAYER, playerData, GameManager.instance.GetPlayerName(), 0);
            Debug.Log(JsonConvert.SerializeObject(message));
            SendDataToServer(message);

            // 开始异步接收数据
            socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ReceiveCallback, socket);
        }
        catch (Exception e)
        {
            Debug.LogError("Error in ConnectCallback: " + e.Message);
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            int bytesRead = socket.EndReceive(ar);

            if (bytesRead > 0)
            {
                byte[] receivedData = new byte[bytesRead];
                Array.Copy(receiveBuffer, receivedData, bytesRead);

                // 处理收到的消息
                string receivedMessage = Encoding.UTF8.GetString(receivedData);
                ReceiveMessage.receivedMessage = receivedMessage;

                // 继续异步接收数据
                socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ReceiveCallback, socket);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error in ReceiveCallback: " + e.Message);
        }
    }

    public void SendDataToServer(Message data)
    {
        try
        {
            byte[] sendData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data) + '\n');
            clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, clientSocket);
        }
        catch (Exception e)
        {
            Debug.LogError("Error sending data to server: " + e.Message);
        }
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);
        }
        catch (Exception e)
        {
            Debug.LogError("Error in SendCallback: " + e.Message);
        }
    }
}
