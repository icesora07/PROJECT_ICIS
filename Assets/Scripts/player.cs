using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private CharacterController cc;

    // [Header("추적 대상")]
    // [SerializeField] private Transform target;

    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("점프 설정")]
    [SerializeField] private float jumpSpeed = 10f;

    [Header("중력 설정")]
    [SerializeField] private float gravity = -25f;
    private Vector3 moveInput;

    // velocity = 현재 속도
    // 주로 y축(중력/점프) 계산에 사용
    private Vector3 velocity;

    void Start()
    {
        cc = GetComponent<CharacterController>();

        if (cc == null)
        {
            Debug.LogError("N.F CharacterController");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        GetInput();
        Move();
    }

    void GetInput()
    {
        float moveX = 0f;
        if (Keyboard.current.aKey.isPressed) moveX = -1f;
        if (Keyboard.current.dKey.isPressed) moveX =  1f;

        float moveZ = 0f;
        if (Keyboard.current.sKey.isPressed) moveZ = -1f;
        if (Keyboard.current.wKey.isPressed) moveZ =  1f;

        if (cc.isGrounded && velocity.y <= 0f && Keyboard.current.spaceKey.isPressed) velocity.y = jumpSpeed;

        moveInput = new Vector3(moveX, 0f, moveZ);
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.z);
        cc.Move(moveDir * moveSpeed * Time.deltaTime);

        // 바닥에 닿아있으면 중력 누적 초기화
        // cc.isGrounded = CharacterController가 자동으로 바닥 감지해주는 값
        // 살짝 음수 유지 = "확실히 바닥에 붙어있음"을 보장
        if (cc.isGrounded && velocity.y <= 0f) velocity.y = -1f;
        
        // 매 프레임마다 중력을 누적 (점점 빠르게 떨어지는 효과)
        velocity.y += gravity * Time.deltaTime;

        // 계산된 속도로 실제 이동 적용
        cc.Move(velocity * Time.deltaTime);
    }
}