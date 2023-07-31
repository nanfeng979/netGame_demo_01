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
            transform.Translate(moveX * moveSpeed * Time.deltaTime, 0, moveY * moveSpeed * Time.deltaTime);
        }
    }
}
