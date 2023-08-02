package org.example;

enum MessageType {
    ADD_PLAYER,
    REMOVE_PLAYER
}

public class Message {
    private String content;
    private MessageType type;
    private String sender;

    public Message(String content, MessageType type, String sender) {
        this.content = content;
        this.type = type;
        this.sender = sender;
    }

    public String getContent() {
        return content;
    }

    public MessageType getType() {
        return type;
    }

    public String getSender() {
        return sender;
    }

    @Override
    public String toString() {
        return "Message{" +
                "content='" + content + '\'' +
                ", type=" + type +
                ", sender='" + sender + '\'' +
                '}';
    }
}