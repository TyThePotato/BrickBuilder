using UnityEngine;
using UnityEngine.InputSystem;

public class EditorCamera : MonoBehaviour
{
    [HideInInspector]
    public Vector2 _cameraRotation = new Vector2(315, 45);

    public float cameraSpeed;
    public float maxYAngle = 80f;
    public float movementSpeed;
    public float speedMulitplier;

    private bool fastSpeed;
    private float speed;

    private void Awake() {
        BBInputManager.Controls.Main.Speed.started += ctx => fastSpeed = true;
        BBInputManager.Controls.Main.Speed.canceled += ctx => fastSpeed = false;
        SettingsManager.SettingsChanged.AddListener(ApplySettings);
        ApplySettings();
    }

    public void ApplySettings () {
        cameraSpeed = SettingsManager.Settings.CameraSensitivity;
        movementSpeed = SettingsManager.Settings.CameraSpeed;
        GetComponent<Camera>().fieldOfView = SettingsManager.Settings.FOV;
    }

    public void ResetTransform() {
        UpdateCameraRotation(new Vector2(315, 45));
        transform.position = new Vector3(100, 125, -100);
    }

    public void UpdateCameraRotation(Vector2 rotation) {
        _cameraRotation = rotation;
        transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0);
    }

    private void Update() {
        if (Mouse.current.rightButton.isPressed) {
            // lock cursor once unless on linux editor because locking cursor on linux editor is scuffed
            if (Application.platform != RuntimePlatform.LinuxEditor) {
                if (Cursor.lockState != CursorLockMode.Locked) {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }

            _cameraRotation += BBInputManager.Controls.Main.CameraLook.ReadValue<Vector2>() * cameraSpeed;
            _cameraRotation.x = Mathf.Repeat(_cameraRotation.x, 360);
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y, -maxYAngle, maxYAngle);

            transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0);
        } else {
            // unlock cursor once
            if (Cursor.lockState != CursorLockMode.None)
                Cursor.lockState = CursorLockMode.None;
        }

        if (BBInputManager.IsCtrlDown()) return;

        speed = fastSpeed ? movementSpeed * speedMulitplier : movementSpeed;

        Vector2 hv = BBInputManager.Controls.Main.CameraXZ.ReadValue<Vector2>();
        float ud = BBInputManager.Controls.Main.CameraY.ReadValue<float>();
        Vector3 m = new Vector3(hv.x, ud, hv.y);

        transform.Translate(m.normalized * speed * Time.deltaTime, Space.Self);
    }
}
