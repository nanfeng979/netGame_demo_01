package org.example;

enum PlayerDataType {
    ADD_PLAYER,
    REMOVE_PLAYER,
    UPDATE_DATA,
    UPDATE_POSITION,
}

public class PlayerData {
    private PlayerDataType type;
    private String name;
    private Vector3 position;

    public PlayerData() {

    }

    public PlayerData(PlayerDataType type, String name, Vector3 position) {
        this.type = type;
        this.name = name;
        this.position = position;
    }

    public PlayerDataType getType() {
        return type;
    }

    public void SetType(PlayerDataType type) {
        this.type = type;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public Vector3 getPosition() {
        return position;
    }

    public void setPosition(Vector3 position) {
        this.position = position;
    }
}
