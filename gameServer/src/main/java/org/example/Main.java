package org.example;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.*;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.TimeUnit;

import com.fasterxml.jackson.databind.ObjectMapper;

import javax.security.auth.callback.Callback;

public class Main {
    private static final int PORT = 8888;

    public static Map<Socket, String> PlayerList;

    public static void main(String[] args) {
        PlayerList = new HashMap<>();

        try {
            ServerSocket serverSocket = new ServerSocket(PORT);
            System.out.println("服务器启动，正在监听端口 " + PORT);

            // 输出当前PlayerList列表
            TimedReply(2, () -> {
                printPlayerList();
            });

            while (true) {
                Socket clientSocket = serverSocket.accept();
                System.out.println("客户端连接成功，IP: " + clientSocket.getInetAddress() + ", 端口: " + clientSocket.getPort());
                if (!PlayerList.containsKey(clientSocket)) {
                    PlayerList.put(clientSocket, "");
                }

                // 创建一个新线程处理客户端连接
                ClientHandler clientHandler = new ClientHandler(clientSocket);
                Thread thread = new Thread(clientHandler);
                thread.start();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    interface TimedReplyCalllback {
        void onCall();
    }

    private static void TimedReply(int time, TimedReplyCalllback timedReplyCalllback) {
        // 定时任务
        ScheduledExecutorService scheduler = Executors.newScheduledThreadPool(1);
        scheduler.scheduleAtFixedRate(() -> {
            timedReplyCalllback.onCall();
        }, 0, time, TimeUnit.SECONDS); // 每隔x秒执行一次
    }

    private static void printPlayerList() {
        System.out.println("当前玩家列表：");
        for (Map.Entry<Socket, String> entry : PlayerList.entrySet()) {
            System.out.print(entry.getValue() + "__");
        }
        System.out.println();
        System.out.println("--------------------");
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
        } else if (message.getType() == MessageType.REMOVE_PLAYER) {
            Main.PlayerList.remove(socket);
        }
        System.out.println("来自客户端: " + name);
    }
}
