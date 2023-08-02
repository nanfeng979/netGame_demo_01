public enum MessageType {
    ADD_PLAYER,
    REMOVE_PLAYER
}

public class Message {
    public string content;
    public MessageType type;
    public string sender;

    public Message(string content, MessageType type, string sender) {
        this.content = content;
        this.type = type;
        this.sender = sender;
    }

    public string getContent() {
        return content;
    }

    public MessageType getType() {
        return type;
    }

    public string getSender() {
        return sender;
    }

    public string toJsonString() {
        return "Message{" +
                "content='" + content + '\'' +
                ", type=" + type +
                ", sender='" + sender + '\'' +
                '}';
    }
}