using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public enum PlayerDataType {
    ADD_PLAYER,
    REMOVE_PLAYER,
    UPDATE_DATA,
    UPDATE_POSITION,
}

public class PlayerData {
    [JsonConverter(typeof(StringEnumConverter))]
    public PlayerDataType type;
    public string name;
    public myVector3 position;

    public PlayerData() {

    }

    public PlayerData(PlayerDataType type, string name,myVector3 position) {
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

    public string getName() {
        return name;
    }

    public void setName(string name) {
        this.name = name;
    }

    public myVector3 getPosition() {
        return position;
    }

    public void setPosition(myVector3 position) {
        this.position = position;
    }

    public string toJson(){
        return "{\"type\":\"" + type + "\", \"name\":\"" + name + "\", \"position\":\"" + position.toJson() + "\"}";
    }
}
