using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    public float horizontalSpeed = 100.0f;
    public float verticalSpeed = 80.0f;
    public float camerraDampValue = 0.05f;

    private IUserInput pi;
    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float tempEulerX;
    private GameObject model;
    private GameObject _camera;

    private Vector3 cameraDampVElocity;

    private void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 20;
        //�ýű�ִ��˳����ActorController֮��(Default Time�Ժ�)
        ActorController ac = playerHandle.GetComponent<ActorController>();
        model = ac.model;
        pi = ac.pi;
        _camera = Camera.main.gameObject;

        Cursor.lockState = CursorLockMode.Locked;
    }


    private void FixedUpdate()
    {
        //ǰ���ƶ�
        //Vector3 newPos = transform.localPosition;
        //newPos.z += verticalSpeed * pi.JForward * Time.fixedDeltaTime;
        //transform.localPosition = newPos;

        Vector3 tempModelEuler = model.transform.eulerAngles;

        //��y����ת ˮƽ����
        playerHandle.transform.Rotate(Vector3.up, pi.JRight * horizontalSpeed * Time.fixedDeltaTime);
        //��z����ת ��ֱ����
        tempEulerX -= pi.Jup * verticalSpeed * Time.fixedDeltaTime;
        tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);

        cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);

        //ģ�ͽǶȲ���
        model.transform.eulerAngles = tempModelEuler;

        //_camera.transform.position = transform.position;
        //����ƶ����
        _camera.transform.position = Vector3.SmoothDamp(_camera.transform.position, transform.position,
            ref cameraDampVElocity,
            camerraDampValue);
        //_camera.transform.eulerAngles = transform.eulerAngles;
        _camera.transform.LookAt(cameraHandle.transform);



    }
}
