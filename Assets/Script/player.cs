using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private CharacterController cc;

    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("중력 설정")]
    [SerializeField] private float gravity = -20f;
    // 음수인 이유: 아래 방향(-y)으로 계속 당겨야 하기 때문
    // -9.8이 현실 중력이지만 게임은 더 강해야 자연스러움

    private Vector2 moveInput;

    // velocity = 현재 속도
    // 주로 y축(중력/점프) 계산에 사용
    private Vector3 velocity;

    void Start()
    {
        cc = GetComponent<CharacterController>();

        if (cc == null)
        {
            Debug.LogError("CharacterController가 없어요!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        GetInput();
        Move();
        ApplyGravity(); // 중력 추가
    }

    void GetInput()
    {
        float moveX = 0f;
        if (Keyboard.current.aKey.isPressed) moveX = -1f;
        if (Keyboard.current.dKey.isPressed) moveX =  1f;

        float moveY = 0f;
        if (Keyboard.current.sKey.isPressed) moveY = -1f;
        if (Keyboard.current.wKey.isPressed) moveY =  1f;

        moveInput = new Vector2(moveX, moveY);
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);
        cc.Move(moveDir * moveSpeed * Time.deltaTime);
    }

    void ApplyGravity()
    {
        // 바닥에 닿아있으면 중력 누적 초기화
        // cc.isGrounded = CharacterController가 자동으로 바닥 감지해주는 값
        if (cc.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
            // 0f가 아닌 -2f인 이유:
            // 완전히 0으로 만들면 isGrounded가 간헐적으로 false로 인식하는 버그 있음
            // 살짝 음수 유지 = "확실히 바닥에 붙어있음"을 보장하는 트릭
        }

        // 매 프레임마다 중력을 누적 (점점 빠르게 떨어지는 효과)
        velocity.y += gravity * Time.deltaTime;

        // 계산된 속도로 실제 이동 적용
        cc.Move(velocity * Time.deltaTime);
    }
}