using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [Header("Movement:")]
    [SerializeField, Range(1f, 25f)] private float Player_Speed;
    float HorMove;
    float VerMove;

    [Header("Camera controller:")]
    [SerializeField] private Transform _cam;
    [SerializeField, Range(0.5f, 4f)] private float SensX;
    [SerializeField, Range(0.5f, 4f)] private float SensY;
    [SerializeField] private float verticalRotation;
    [SerializeField] private float MinLookY;
    [SerializeField] private float MaxLookY;
    float mouseX;
    float mouseY;

    [Header("Jump and gravity:")]
    [SerializeField, Range(1f, 20f)] private float Jump_Force;
    [SerializeField, Range(-1f, -20f)] private float Gravity;
    private Vector3 velocity;

    [Header("Ground Check:")]
    [SerializeField, Range(0.1f, 0.50f)] private float Ground_Distance;

    [Header("Character controller:")]
    [SerializeField] private CharacterController _characterController;

    private Vector3 moveDirection;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        // блокировка и невидемость екосора в центре экрана
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MyInput();
        Jump();

        if (_cam != null)
        {
            // ввод с компьтерной мыши, присваивание осей
            mouseX = Input.GetAxisRaw("Mouse X") * SensX;
            mouseY = Input.GetAxisRaw("Mouse Y") * SensY;

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, MinLookY, MaxLookY);

            _cam.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
            transform.Rotate(Vector3.up * mouseX);
        }
    }

    private void FixedUpdate()
    {
        DirectionAndMove();
    }

    private void MyInput()
    {
        HorMove = Input.GetAxisRaw("Horizontal");
        VerMove = Input.GetAxisRaw("Vertical");
    }

    private void DirectionAndMove()
    {
        moveDirection = transform.forward * VerMove + transform.right * HorMove;
        _characterController.Move(moveDirection * Player_Speed * Time.deltaTime);
        moveDirection.Normalize();   
    }

    private void Jump()
    {
        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            velocity.y = Jump_Force;
        }

        else
        {
            velocity.y += Gravity * Time.deltaTime;
        }

        _characterController.Move(velocity * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Ground_Distance))
        {
            return true;
        }
        return false;
    }
}
