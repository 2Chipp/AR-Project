using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMov : MonoBehaviour
{
    [SerializeField] private Transform rotTransform;
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;

    private float rotSpeed;

    private Vector2 touchStartPos;
    private Vector2 touchDir;


    private void FixedUpdate()
    {
        Rotate();
    }

    public void SetRotSpeed(float rotSpeed)
    {
        this.rotSpeed = rotSpeed;
    }

    private Vector3 ClampRot()
    {
        Vector3 tempRot = rotTransform.localEulerAngles;
        float x = Mathf.Clamp(tempRot.x, minAngle, maxAngle);
        tempRot = new Vector3(x, tempRot.y, tempRot.z);
        return tempRot;
    }

#if UNITY_EDITOR
    private void Rotate()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("in rot");

            float y = Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
            float x = Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;

            rotTransform.Rotate(0, y, 0, Space.World);
            rotTransform.Rotate(x, 0, 0, Space.Self);

            rotTransform.localRotation = Quaternion.Euler(ClampRot());
        }
    }

#elif UNITY_ANDROID

    private void Rotate()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchDir = Input.GetTouch(0).deltaPosition;
            touchDir.Normalize();

            float y = touchDir.x * rotSpeed/10 * Time.deltaTime;
            float x = touchDir.y * rotSpeed/10 * Time.deltaTime;

            rotTransform.Rotate(0, y, 0, Space.World);
            rotTransform.Rotate(x, 0, 0, Space.Self);

            rotTransform.localRotation = Quaternion.Euler(ClampRot());
        }
    }

#endif
}
