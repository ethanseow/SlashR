using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


// change monobehaviorpun to monobehavior
public class Camera : MonoBehaviourPun
{
    private float refVelocX;
    private float refVelocY;
    private float amtRotatedY = 0;
    private float rotX = 0;
    private float rotY;
    private float smoothCursorSpeed;

    public float mouseSensitivity;


    void Start()
    {
        smoothCursorSpeed = SingletonInfo.Instance.cursorSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        /*
        if (photonView.IsMine == false)
        {
            this.enabled = false;
        }
        */
    }

    void FixedUpdate()
    {
        rotX = Mathf.SmoothDamp(rotX,Input.GetAxis("Mouse X")  * mouseSensitivity * Time.deltaTime, ref refVelocX, smoothCursorSpeed);
        rotY = Mathf.SmoothDamp(rotY,- Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime, ref refVelocY, smoothCursorSpeed);
        amtRotatedY += rotY;
        if(amtRotatedY >= 90)
        {
            ClampYRot(90);
            rotY = 0;
            amtRotatedY = 90;
        }
        else if(amtRotatedY <= -90)
        {
            ClampYRot(-90);
            rotY = 0;
            amtRotatedY = -90;
        }
        transform.Rotate(new Vector3(rotY, 0, 0));
        transform.parent.Rotate(new Vector3(0, rotX, 0));
    }

    private void ClampYRot(float rotClampValue)
    {
        transform.eulerAngles = new Vector3(rotClampValue, transform.eulerAngles.y, transform.eulerAngles.z);
        //armsPivotTransform.eulerAngles = new Vector3(rotClampValue, armsPivotTransform.eulerAngles.y, armsPivotTransform.eulerAngles.z);
    }
}
