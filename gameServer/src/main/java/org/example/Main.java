package org.example;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.ServerSocket;
import java.net.Socket;

public class Main {
    public static void main(String[] args) {
        int port = 12345; // 服务端监听的端口号
        try {
            ServerSocket serverSocket = new ServerSocket(port);
            System.out.println("服务器已启动，正在监听端口：" + port);

            while (true) {
                Socket clientSocket = serverSocket.accept();
                String clientIP = clientSocket.getInetAddress().getHostAddress();
                int clientPort = clientSocket.getPort();
                System.out.println("客户端连接成功，IP地址：" + clientIP + "，端口号：" + clientPort);

                // 可以在这里对客户端进行处理，例如启动一个新的线程来处理与客户端的交互

                // 为了简单起见，这里直接关闭与客户端的连接
                clientSocket.close();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
