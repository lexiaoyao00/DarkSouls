using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    
    public float horizontalSpeed = 100.0f;
    public float verticalSpeed = 80.0f;
    public float camerraDampValue = 0.05f;
    public Image lockDot;
    public bool lockState;

    private IUserInput pi;
    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float tempEulerX;
    private GameObject model;
    private GameObject _camera;
    private Vector3 cameraDampVElocity;
    [SerializeField]
    private LockTarget lockTarget;

    private void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 20;
        //该脚本执行顺序在ActorController之后(Default Time以后)
        ActorController ac = playerHandle.GetComponent<ActorController>();
        model = ac.model;
        pi = ac.pi;
        _camera = Camera.main.gameObject;
        lockDot.enabled = false;
        lockState = false;

        Cursor.lockState = CursorLockMode.Locked;
    }


    private void FixedUpdate()
    {
        //锁定敌人
        if (lockTarget == null)
        {
            //前后移动
            Vector3 newPos = transform.localPosition;
            newPos.z += verticalSpeed * pi.JForward * Time.fixedDeltaTime;
            transform.localPosition = newPos;

            Vector3 tempModelEuler = model.transform.eulerAngles;

            //绕y肘旋转 水平方向
            playerHandle.transform.Rotate(Vector3.up, pi.JRight * horizontalSpeed * Time.fixedDeltaTime);
            //绕z轴旋转 垂直方向
            tempEulerX -= pi.Jup * verticalSpeed * Time.fixedDeltaTime;
            tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);

            cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);

            //模型角度不变
            model.transform.eulerAngles = tempModelEuler;
        }
        else
        {
            Vector3 tempForward = lockTarget.obj.transform.position - model.transform.position;
            tempForward.y = 0;
            playerHandle.transform.forward = tempForward;
            cameraHandle.transform.LookAt(lockTarget.obj.transform);
        }
      

        //_camera.transform.position = transform.position;
        //柔和移动相机
        _camera.transform.position = Vector3.SmoothDamp(_camera.transform.position, transform.position,
            ref cameraDampVElocity,
            camerraDampValue);
        //_camera.transform.eulerAngles = transform.eulerAngles;
        _camera.transform.LookAt(cameraHandle.transform);



    }

    private void Update()
    {
        if(lockTarget != null)
        {
            lockDot.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position 
                + new Vector3(0, lockTarget.halfHeight, 0));
            if (Vector3.Distance(model.transform.position,lockTarget.obj.transform.position) > 10f)
            {
                lockTarget = null;
                lockDot.enabled = false;
                lockState = false;
            }
        }
    }

    public void LockUnlock()
    {
        Vector3 modelOrigin1 = model.transform.position;
        Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
        Vector3 boxCenter = modelOrigin2 + model.transform.forward * 5f;
        Collider[] cols = Physics.OverlapBox(boxCenter, 
            new Vector3(0.5f, 0.5f, 5f), 
            model.transform.rotation, 
            LayerMask.GetMask("Enemy"));

        if (cols.Length == 0)
        {
            lockTarget = null;
            lockDot.enabled = false;
            lockState = false;
        }
        else
        {
            foreach (var col in cols)
            {
                if (lockTarget != null && lockTarget.obj == col.gameObject)
                {
                    lockTarget = null;
                    lockDot.enabled = false;
                    lockState = false;
                    break;
                }
                lockTarget = new LockTarget(col.gameObject,col.bounds.extents.y);
                lockDot.enabled = true;
                lockState = true;
                break;
            }
        }

    }

    private class LockTarget
    {
        public GameObject obj;
        public float halfHeight;

        public LockTarget(GameObject _obj, float _halfHeight)
        {
            obj = _obj;
            halfHeight = _halfHeight;
        }
    }
}
