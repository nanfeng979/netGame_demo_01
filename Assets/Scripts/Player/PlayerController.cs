using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float rotateSpeed = 5.0f;
    private float mouseX;

    public float moveSpeed = 5.0f;
    private float moveX;
    private float moveY;

    // 用于连接
    PlayerData playerData;
    Message message;

    private void Awake()
    {
        gameObject.name = PlayerManager.GetCurrentPlayerName();

        float RandomX = Random.Range(-3.0f, 3.0f);
        float RandomZ = Random.Range(-3.0f, 3.0f);
        transform.position = new Vector3(RandomX, 0, RandomZ);
        PlayerManager.currentPlayerPosition = transform.position;

        playerData = new PlayerData(PlayerDataType.UPDATE_DATA, PlayerManager.GetCurrentPlayerName(), new myVector3(PlayerManager.currentPlayerPosition));
        message = new Message(MessageType.Broadcast, DoingType.UPDATE_DATA, playerData, PlayerManager.GetCurrentPlayerName(), 0);
    }

    void Start()
    {
        
    }

    void Update()
    {
        Rotate();
        Move();
    }

    private void Rotate() {
        mouseX += Input.GetAxis("Mouse X") * rotateSpeed;
        transform.rotation = Quaternion.Euler(0, mouseX, 0);
    }

    private void Move() {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        if (moveX != 0 || moveY != 0) {
            PlayerManager.currentPlayerPosition += moveX * moveSpeed * Time.deltaTime * transform.right;
            PlayerManager.currentPlayerPosition += moveY * moveSpeed * Time.deltaTime * transform.forward;
            Connect();
        }
    }

    private void Connect() {
        message.playerData.SetType(PlayerDataType.UPDATE_POSITION);
        message.playerData.setPosition(new myVector3(PlayerManager.currentPlayerPosition));
        SocketClient.SendDataToServer(message);
    }
}
