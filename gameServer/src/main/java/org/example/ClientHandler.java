package org.example;

import com.fasterxml.jackson.databind.ObjectMapper;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.Socket;
import java.util.Map;

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
            // 客户端连接时，返回确认连接的响应消息
            out.println("连接成功，欢迎进入游戏！");

            // 接收客户端消息
            BufferedReader reader = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
            String clientMessage;
            while((clientMessage = reader.readLine()) != null){
                // 处理客户端消息
                // 将消息反序列化
                ObjectMapper jsonObject = new ObjectMapper();
                Message message = jsonObject.readValue(clientMessage, Message.class);
                System.out.println("收到来自" +  message.getSender() + "的客户端消息:" + clientMessage);

                // 管理PlayerList中玩家的接入与移出
                playerListManager(clientSocket, message);
            }

//            clientSocket.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private static void playerListManager(Socket socket, Message message) {
        String name = message.getSender();
        if (message.getType() == MessageType.ADD_PLAYER) {
            Main.PlayerList.put(socket, message.getSender());
            broadcastMessage(name + "加入游戏");
        } else if (message.getType() == MessageType.REMOVE_PLAYER) {
            Main.PlayerList.remove(socket);
            broadcastMessage(name + "离开游戏");
        }
        System.out.println("来自客户端: " + name);
    }

    // 广播消息给所有客户端
    public static void broadcastMessage(String message) {
        for (Map.Entry<Socket, String> entry : Main.PlayerList.entrySet()) {
            try {
                PrintWriter out = new PrintWriter(entry.getKey().getOutputStream(), true);
                out.println(message);
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }
}