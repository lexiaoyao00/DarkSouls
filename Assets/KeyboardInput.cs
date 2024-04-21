using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : IUserInput
{
    //TODO:按钮封装长按双击等实现 参考手柄输入脚本
    [Header("===== Key Setting  =====")]
    public string KeyUp = "w";
    public string KeyDown = "s";
    public string KeyLeft = "a";
    public string KeyRight = "d";

    public string KeyA;
    public string KeyB;
    public string KeyC;
    public string KeyD;

    public string keyJUp = "up";
    public string keyJDown = "down";
    public string keyJRight = "right";
    public string keyJLeft = "left";

    [Header("===== Key Setting  =====")]
    public bool mouseEnable = false;
    public float mouseSensitivityX = 1f;
    public float mouseSensitivityY = 1f;


    // Update is called once per frame
    void Update()
    {
        //****摄像机控制****
        if (mouseEnable == true)
        {
            Jup = Input.GetAxis("Mouse Y") * 2.5f * mouseSensitivityY;
            JRight = Input.GetAxis("Mouse X") * 3f * mouseSensitivityX;
            //JForward = Input.GetAxis("Mouse ScrollWheel");
        }
        else
        {
            Jup = (Input.GetKey(keyJUp) ? 1.0f : 0f) - (Input.GetKey(keyJDown) ? 1.0f : 0f);//类似Input.GetAxis("Mouse Y")
            JRight = (Input.GetKey(keyJRight) ? 1.0f : 0f) - (Input.GetKey(keyJLeft) ? 1.0f : 0f);//类似Input.GetAxis("Mouse X")
        }



        //****移动控制****
        targetDup = (Input.GetKey(KeyUp) ? 1.0f : 0) - (Input.GetKey(KeyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(KeyRight) ? 1.0f : 0) - (Input.GetKey(KeyLeft) ? 1.0f : 0);

        if (!inputEnable)
        {
            targetDup = 0;
            targetDright = 0;
        }

        
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        //这一步使斜向移动时的Dmag不会超出1
        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        float Dright2 = tempDAxis.x;
        float Dup2 = tempDAxis.y;

        Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));//类似vector3.magnitude
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;

        run = Input.GetKey(KeyA);
        defense = Input.GetKey(KeyD);

        //****跳跃控制****
        bool newJump = Input.GetKey(KeyB);
        if (newJump != lastJump && newJump == true)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
        lastJump = newJump;

        //****攻击控制****
        bool newAttack = Input.GetKey(KeyC);
        if (newAttack != lastAttack && newAttack == true)
        {
            attack = true;
        }
        else
        {
            attack = false;
        }
        lastAttack = newJump;
    }
}
