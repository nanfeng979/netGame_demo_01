package org.example;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;

public class Main {
    private static final int PORT = 8888;

    public static void main(String[] args) {
        try {
            ServerSocket serverSocket = new ServerSocket(PORT);
            System.out.println("服务器启动，正在监听端口 " + PORT);

            while (true) {
                Socket clientSocket = serverSocket.accept();
                System.out.println("客户端连接成功，IP: " + clientSocket.getInetAddress() + ", 端口: " + clientSocket.getPort());

                // 创建一个新线程处理客户端连接
                ClientHandler clientHandler = new ClientHandler(clientSocket);
                Thread thread = new Thread(clientHandler);
                thread.start();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}

class ClientHandler implements Runnable {
    private final Socket clientSocket;

    public ClientHandler(Socket clientSocket) {
        this.clientSocket = clientSocket;
    }

    @Override
    public void run() {
        try {
            // 处理客户端连接，你可以在这里添加游戏逻辑
            PrintWriter out = new PrintWriter(clientSocket.getOutputStream(), true);
            out.println("连接成功，欢迎进入游戏！");

            // 这里可以添加更多的游戏逻辑和交互
            // ...
            BufferedReader in = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
            String clientMessage;
            while((clientMessage = in.readLine()) != null){
                System.out.println("收到客户端消息:" + clientMessage);

                // 处理客户端消息
                // ...
            }

//            clientSocket.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
