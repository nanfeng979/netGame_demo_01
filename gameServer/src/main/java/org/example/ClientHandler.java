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
            // 客户端连接时，返回确认连接的响应消息
//            sendMessage(clientSocket, "连接成功，欢迎进入游戏！");

            // 接收客户端消息
            BufferedReader reader = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
            String clientMessage;
            while((clientMessage = reader.readLine()) != null){
                // 处理客户端消息
                // 将消息反序列化
                ObjectMapper jsonObject = new ObjectMapper();
                Message message = jsonObject.readValue(clientMessage, Message.class);

                // 管理PlayerList中玩家的接入与移出
                playerListManager(clientSocket, message, clientMessage);
            }

//            clientSocket.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private static void playerListManager(Socket socket, Message message, String jsonMessage) {
        String name = message.getSender();
        if (message.getType() == MessageType.Broadcast) {
            if (message.getDoing() == DoingType.ADD_PLAYER) {
                Main.PlayerList.put(socket, message.getPlayerData());
//            broadcastMessage(name + "加入游戏");
//                // 当有新玩家加入时，广播给其他人
//                broadcastMessage(jsonMessage);

            } else if (message.getDoing() == DoingType.UPDATE_PLAYER) {
                // 新加入后会由系统通知目前总共有多少人
                // 初始化ObjectMapper对象
                ObjectMapper objectMapper = new ObjectMapper();

                Message sendAllPlayerData = new Message();
                sendAllPlayerData.SetType(MessageType.Single);
                sendAllPlayerData.SetDoing(DoingType.UPDATE_PLAYER);
                sendAllPlayerData.SetSender("System");

                String jsonString = "";
                for (Map.Entry<Socket, PlayerData> entry : Main.PlayerList.entrySet()) {
                    sendAllPlayerData.SetPlayerData(entry.getValue());
                    try {
                        // 将Person对象序列化为JSON字符串
                        jsonString += objectMapper.writeValueAsString(sendAllPlayerData) + "&&";

                    } catch (Exception e) {
                        e.printStackTrace();
                    }
                }

                sendMessage(socket, jsonString);
            }

            else if (message.getDoing() == DoingType.REMOVE_PLAYER) {
                Main.PlayerList.remove(socket);
//            broadcastMessage(name + "离开游戏");
//            broadcastMessage(message.toString());
            }
        } else if (message.getType() == MessageType.Single) {
            // 目前默认单播仅有将所有角色信息发送给某一用户
            System.out.println("来自客户端: " + jsonMessage);
        }
    }

    // 广播消息给所有客户端
    public static void broadcastMessage(String message) {
        for (Map.Entry<Socket, PlayerData> entry : Main.PlayerList.entrySet()) {
            try {
                PrintWriter out = new PrintWriter(entry.getKey().getOutputStream(), true);
                out.println(message); // todo: 前面组装type
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }

    // 单播消息给某个客户端
    public static void sendMessage(Socket socket, String message) {
        try {
            PrintWriter out = new PrintWriter(socket.getOutputStream(), true);
            out.println(message);
        }
        catch (IOException e) {
            e.printStackTrace();
        }
    }


}