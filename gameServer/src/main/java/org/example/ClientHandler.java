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

    // 初始化ObjectMapper对象
    private static ObjectMapper objectMapper = new ObjectMapper();
    private static Message message;

    public ClientHandler(Socket clientSocket) {
        this.clientSocket = clientSocket;
    }

    @Override
    public void run() {
        try {
            // 接收客户端消息
            BufferedReader reader = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
            // 处理客户端消息
            String clientMessage;
            while((clientMessage = reader.readLine()) != null){
                // 将消息反序列化
                ObjectMapper jsonObject = new ObjectMapper();
                message = jsonObject.readValue(clientMessage, Message.class);
                // 管理接收到的Message
                messageManager(clientSocket, message, clientMessage);
            }
//            clientSocket.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private static void messageManager(Socket socket, Message message, String jsonMessage) {
        // 收到广播消息
        if (message.getType() == MessageType.Broadcast) {
            // 玩家加入
            if (message.getDoing() == DoingType.ADD_PLAYER) {
                // 将该玩家添加到PlayerList
                Main.PlayerList.put(socket, message.getPlayerData());
            }
            // 向发送方单播，发送所有的玩家信息给该发送方
            else if (message.getDoing() == DoingType.UPDATE_PLAYER) {
                // 向某一方更新所有的玩家信息
                Update_Player_Single(socket);
            }
            // 玩家退出
            else if (message.getDoing() == DoingType.REMOVE_PLAYER) {
                // 将该玩家从PlayerList移出
                Main.PlayerList.remove(socket);
            }
        }
        // 收到单播消息
        else if (message.getType() == MessageType.Single) {

        }
    }

    // 向某一方更新所有的玩家信息
    private static void Update_Player_Single(Socket socket) {
        // 组装所有玩家基础信息
        Message playerDataMessage = new Message();
        playerDataMessage.SetType(MessageType.Single);
        playerDataMessage.SetDoing(DoingType.UPDATE_PLAYER);
        playerDataMessage.SetSender("System");

        String allPlayerDataMessage = "";
        for (Map.Entry<Socket, PlayerData> entry : Main.PlayerList.entrySet()) {
            // 增加各个玩家的playerData
            playerDataMessage.SetPlayerData(entry.getValue());
            try {
                // todo：分开发送
                allPlayerDataMessage += objectMapper.writeValueAsString(playerDataMessage) + "&&";
            } catch (Exception e) {
                e.printStackTrace();
            }
        }

        // 单播消息
        sendMessage(socket, allPlayerDataMessage);
    }

    // 广播消息给所有客户端
    private static void broadcastMessage(String message) {
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
    private static void sendMessage(Socket socket, String message) {
        try {
            PrintWriter out = new PrintWriter(socket.getOutputStream(), true);
            out.println(message);
        }
        catch (IOException e) {
            e.printStackTrace();
        }
    }
}