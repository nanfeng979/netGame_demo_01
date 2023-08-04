package org.example;

enum PlayerDataType {
    ADD_PLAYER,
    REMOVE_PLAYER,
    UPDATE_DATA,
    UPDATE_POSITION,
    UPDATE_ROTATION,
}

public class PlayerData {
    private PlayerDataType type;
    private String name;
    private Vector3 position;
    private Vector3 rotation;

    public PlayerData() {

    }

    public PlayerData(PlayerDataType type, String name, Vector3 position, Vector3 rotation) {
        this.type = type;
        this.name = name;
        this.position = position;
        this.rotation = rotation;
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

    public Vector3 getRotation() {
        return rotation;
    }

    public void setRotation(Vector3 rotation) {
        this.rotation = rotation;
    }
}
