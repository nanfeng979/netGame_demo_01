package org.example;

enum DoingType {
    ADD_PLAYER,
    REMOVE_PLAYER,
    UPDATE_DATA,
    UPDATE_PLAYER
}

enum MessageType {
    Broadcast,
    Single,
}

public class Message {
    private MessageType type;
    private DoingType doing;
    private PlayerData playerData;
    private String sender;

    private int flag;

    public Message() {

    }

    public Message(MessageType type, DoingType doing, PlayerData playerData, String sender, int flag) {
        this.type = type;
        this.doing = doing;
        this.playerData = playerData;
        this.sender = sender;
        this.flag = flag;
    }

    public MessageType getType() {
        return type;
    }

    public void SetType(MessageType type) {
        this.type = type;
    }

    public DoingType getDoing() { return doing; }

    public void SetDoing(DoingType doing) { this.doing = doing; }

    public PlayerData getPlayerData() { return playerData; }

    public void SetPlayerData(PlayerData playerData) {
        this.playerData = playerData;
    }

    public String getSender() {
        return sender;
    }

    public void SetSender(String sender) {
        this.sender = sender;
    }

    public int getFlag() {
        return flag;
    }

    public void SetFlag(int flag) {
        this.flag = flag;
    }
}