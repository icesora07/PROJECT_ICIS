using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        // 마우스 커서를 중앙에 잠그고 가린다.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}