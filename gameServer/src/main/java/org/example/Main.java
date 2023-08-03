package org.example;

import com.fasterxml.jackson.databind.ObjectMapper;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.*;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.TimeUnit;

public class Main {
    private static final int PORT = 8888;

    public static Map<Socket, PlayerData> PlayerList;

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
                    PlayerList.put(clientSocket, new PlayerData());
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
        for (Map.Entry<Socket, PlayerData> entry : PlayerList.entrySet()) {
            System.out.print(entry.getValue().getName() + "__");
        }
        System.out.println();
        System.out.println("--------------------");
    }
}
