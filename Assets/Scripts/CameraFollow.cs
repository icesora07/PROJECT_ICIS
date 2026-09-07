using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [Header("추적 대상")]
    [SerializeField] private Transform target; // 따라갈 플레이어 Transform

    [Header("카메라 오프셋 (거리 설정)")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -10f);

    [Header("추적 부드러움")]
    [SerializeField] private float smoothSpeed = 5f;

    // 플레이어의 이동 계산이 끝난 후 실행되도록 LateUpdate 사용
    void LateUpdate() {
        if (target == null) return;

        // 목표 위치 계산 (플레이어 위치 + 지정한 거리)
        Vector3 desiredPosition = target.position + offset;

        // 현재 위치에서 목표 위치로 부드럽게 이동 (선형 보간)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // 카메라 위치 업데이트
        transform.position = smoothedPosition;

        // 카메라가 플레이어를 상시 바라보도록 설정 (필요 시 주석 해제)
        // transform.LookAt(target);
    }
}