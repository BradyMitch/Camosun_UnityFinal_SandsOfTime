using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseXAndY,
        MouseX,
        MouseY
    }

    void Awake()
    {
        Messenger.AddListener(GameEvent.GAME_ACTIVE, this.OnActive);
        Messenger.AddListener(GameEvent.GAME_INACTIVE, this.OnInActive);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.GAME_ACTIVE, this.OnActive);
        Messenger.RemoveListener(GameEvent.GAME_INACTIVE, this.OnInActive);
    }

    public RotationAxes axes = RotationAxes.MouseXAndY;

    [SerializeField] private float sensitivity = 9f;

    public float minVert = -45f;
    public float maxVert = 45f;

    private float rotationX = 0f;

    private bool isActive = true;

    // Update is called once per frame
    void Update()
    {
        if (axes == RotationAxes.MouseX && isActive)
        {
            // horizontal rotation
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * sensitivity);
        }
        else if (axes == RotationAxes.MouseY && isActive)
        {
            // vertical rotation
            rotationX -= Input.GetAxis("Mouse Y") * sensitivity;
            rotationX = Mathf.Clamp(rotationX, minVert, maxVert);

            transform.localEulerAngles = new Vector3(rotationX, 0, 0);
        }
        else
        {
            if (isActive)
            {
                // both horizontal and vertical rotation
                rotationX -= Input.GetAxis("Mouse Y") * sensitivity;
                rotationX = Mathf.Clamp(rotationX, minVert, maxVert);

                float deltaHoriz = Input.GetAxis("Mouse X") * sensitivity;
                float rotationY = transform.localEulerAngles.y + deltaHoriz;

                transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
            }
        }
    }

    private void OnActive()
    {
        Debug.Log("MouseLook: isActive true");
        isActive = true;
    }

    private void OnInActive()
    {
        Debug.Log("MouseLook: isActive false");
        isActive = false;
    }
}
