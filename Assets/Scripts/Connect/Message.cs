using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public enum DoingType {
    ADD_PLAYER,
    REMOVE_PLAYER,
    UPDATE_DATA,
    UPDATE_PLAYER,
}

public enum MessageType {
    Broadcast,
    Single,
}

public class Message {
    [JsonConverter(typeof(StringEnumConverter))]
    public MessageType type;
    [JsonConverter(typeof(StringEnumConverter))]
    public DoingType doing;
    public PlayerData playerData;
    public string sender;

    public int flag;

    public Message() {

    }

    public Message(MessageType type, DoingType doing, PlayerData playerData, string sender, int flag) {
        this.type = type;
        this.doing = doing;
        this.playerData = playerData;
        this.sender = sender;
        this.flag = flag;
    }

}