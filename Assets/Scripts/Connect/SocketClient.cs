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
    private static Socket clientSocket;
    private readonly byte[] receiveBuffer = new byte[1024];
    private string currentPlayerName;

    private void Start()
    {
        currentPlayerName = PlayerManager.GetCurrentPlayerName();

        ConnectToServer();

        InvokeRepeating(nameof(Update_player), 1.0f, 1.0f);
    }

    private void Update()
    {
        
    }

    private void Update_player()
    {
        PlayerData playerData = new PlayerData(PlayerDataType.ADD_PLAYER, currentPlayerName, null, null);
        Message message = new Message(MessageType.Broadcast, DoingType.UPDATE_PLAYER, playerData, currentPlayerName, 0);
        SendDataToServer(message);
    }

    private void OnDestroy() {
        // 发送断开消息
        PlayerData playerData = new PlayerData(PlayerDataType.REMOVE_PLAYER, currentPlayerName, null, null);
        Message message = new Message(MessageType.Broadcast, DoingType.REMOVE_PLAYER, playerData, currentPlayerName, 0);
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
            Debug.LogError("连接到服务器失败: " + e.Message);
        }
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            Debug.Log("已连接到服务器.");

            // 发送连接消息
            // 需放在此处，紧接着开始异步接收数据
            PlayerData playerData = new PlayerData(PlayerDataType.ADD_PLAYER, PlayerManager.GetCurrentPlayerName(), new myVector3(PlayerManager.currentPlayerPosition), new myVector3(PlayerManager.currentPlayerRotation));
            Message message = new Message(MessageType.Broadcast, DoingType.ADD_PLAYER, playerData, PlayerManager.GetCurrentPlayerName(), 0);
            Debug.Log(JsonConvert.SerializeObject(message));
            SendDataToServer(message);

            // 开始异步接收数据
            socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ReceiveCallback, socket);
        }
        catch (Exception e)
        {
            Debug.LogError("调用连接回调函数失败: " + e.Message);
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
            Debug.LogError("调用接收回调函数失败: " + e.Message);
        }
    }

    public static void SendDataToServer(Message data)
    {
        try
        {
            byte[] sendData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data) + '\n');
            clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, clientSocket);
        }
        catch (Exception e)
        {
            Debug.LogError("发送数据失败: " + e.Message);
        }
    }

    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);
        }
        catch (Exception e)
        {
            Debug.LogError("调用发送回调函数失败: " + e.Message);
        }
    }
}
