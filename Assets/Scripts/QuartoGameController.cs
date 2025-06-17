using UnityEngine;

public class QuartoGameController : MonoBehaviour
{
    private void HandleRightMouseDrag()
    {
        if (Input.GetMouseButton(1)) // 右クリックが押されている間
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // 回転の感度を大幅に上げる（3.0fに変更）
            float rotationSpeed = 20.0f;
            
            // マウスの移動量に応じて回転
            transform.Rotate(Vector3.up, -mouseX * rotationSpeed, Space.World);
            transform.Rotate(Vector3.right, mouseY * rotationSpeed, Space.World);
        }
    }
} 