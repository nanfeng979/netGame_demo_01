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

    private void Start()
    {
        ConnectToServer();
    }

    private void OnDestroy() {
        // 发送断开消息
        Message message = new Message("我已关闭", MessageType.REMOVE_PLAYER, GameManager.instance.GetPlayerName());
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
            Message message = new Message("我已连接", MessageType.ADD_PLAYER, GameManager.instance.GetPlayerName());
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
                Debug.Log("Received from server: " + receivedMessage);

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
