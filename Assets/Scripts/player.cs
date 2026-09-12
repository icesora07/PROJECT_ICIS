using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    private CharacterController cc;

    // [Header("추적 대상")]
    // [SerializeField] private Transform target;

    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("달리기 설정")]
    [SerializeField] private float runSpeed = 8f;

    [Header("점프 설정")]
    [SerializeField] private float jumpSpeed = 10f;

    [Header("중력 설정")]
    [SerializeField] private float gravity = -25f;
    private Vector3 moveInput;

    // velocity = 현재 속도
    // 주로 y축(중력/점프) 계산에 사용
    private Vector3 velocity;

    void Start() {
        cc = GetComponent<CharacterController>();
    }

    void Update() {
        GetInput();
        Move();
    }

    void GetInput() {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();
        Debug.DrawRay(cc.transform.position, new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z) * 7f, Color.blue);
        Debug.DrawRay(cc.transform.position, cc.transform.forward * 5f, Color.red);
        Debug.Log(cc.transform.forward);

        float moveX = 0f;
        float moveZ = 0f;
        if (Keyboard.current.wKey.isPressed) { moveX += cameraForward.x; moveZ += cameraForward.z; }
        if (Keyboard.current.sKey.isPressed) { moveX -= cameraForward.x; moveZ -= cameraForward.z; }
        if (Keyboard.current.dKey.isPressed) { moveX += cameraRight.x; moveZ += cameraRight.z; }
        if (Keyboard.current.aKey.isPressed) { moveX -= cameraRight.x; moveZ -= cameraRight.z; }

        if (cc.isGrounded && velocity.y <= 0f && Keyboard.current.spaceKey.isPressed) velocity.y = jumpSpeed;

        moveInput = new Vector3(moveX, 0f, moveZ);
        moveInput.Normalize();
    }

    void Move() {
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.z);
        velocity.x = moveDir.x;
        velocity.z = moveDir.z;

        float currentSpeed;

        if (Keyboard.current.shiftKey.isPressed) currentSpeed = runSpeed;
        else currentSpeed = moveSpeed;

        if (moveDir.sqrMagnitude > 0.01f) {
            velocity.x = moveDir.x * currentSpeed;
            velocity.z = moveDir.z * currentSpeed;
            
            if (cc.transform.forward != new Vector3(velocity.x, 0f, velocity.z)) {
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                cc.transform.rotation = Quaternion.RotateTowards(cc.transform.rotation, targetRotation, 1000f * Time.deltaTime);
            }
        }
        else {
            velocity.x = 0f;
            velocity.z = 0f;
        }

        if (cc.isGrounded && velocity.y <= 0f) velocity.y = -1f;
        velocity.y += gravity * Time.deltaTime;


        cc.Move(velocity * Time.deltaTime);
    }
}