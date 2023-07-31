using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SocketClient : MonoBehaviour
{
    private Socket clientSocket;
    private Thread clientThread;
    private byte[] buffer = new byte[1024];

    public string serverIP = "127.0.0.1"; // 服务器IP地址
    public int serverPort = 12345; // 服务器端口号

    private void Start()
    {
        ConnectToServer();
    }

    private void OnDestroy()
    {
        DisconnectFromServer();
    }

    private void ConnectToServer()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse(serverIP), serverPort));
            Debug.Log("Connected to server: " + serverIP + ":" + serverPort);

            clientThread = new Thread(ReceiveData);
            clientThread.Start();
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to server: " + e.Message);
        }
    }

    private void ReceiveData()
    {
        while (clientSocket.Connected)
        {
            try
            {
                int bytesRead = clientSocket.Receive(buffer);
                if (bytesRead > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Debug.Log("Received from server: " + message);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error receiving data from server: " + e.Message);
                break;
            }
        }
    }

    private void DisconnectFromServer()
    {
        if (clientSocket != null && clientSocket.Connected)
        {
            clientSocket.Close();
            clientSocket = null;
            Debug.Log("Disconnected from server.");
        }

        if (clientThread != null && clientThread.IsAlive)
        {
            clientThread.Join();
            clientThread = null;
        }
    }

    // 发送数据到服务器
    public void SendDataToServer(string data)
    {
        if (clientSocket != null && clientSocket.Connected)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            clientSocket.Send(dataBytes);
        }
    }
}
