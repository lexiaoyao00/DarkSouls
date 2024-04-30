using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : IUserInput
{
    //TODO:��ť��װ����˫����ʵ�� �ο��ֱ�����ű�
    [Header("===== Key Setting  =====")]
    public string KeyUp = "w";
    public string KeyDown = "s";
    public string KeyLeft = "a";
    public string KeyRight = "d";

    public string MouseX = "Mouse X";
    public string MouseY = "Mouse Y";
    public string MouseScrollWheel = "Mouse ScrollWheel";

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


    public MyButton buttonA = new MyButton();
    public MyButton buttonB = new MyButton();
    public MyButton buttonC = new MyButton();
    public MyButton buttonD = new MyButton();
    public MyButton buttonLB = new MyButton();
    public MyButton buttonRB = new MyButton();
    public MyButton buttonJstick = new MyButton();

    // Update is called once per frame
    void Update()
    {

        buttonA.Tick(Input.GetKey(KeyA));//�ܲ�
        buttonB.Tick(Input.GetKey(KeyB));//��
        buttonC.Tick(Input.GetKey(KeyC));//����
        buttonD.Tick(Input.GetKey(KeyD));//����
        buttonLB.Tick(Input.GetKey("mouse 1"));//�Ҽ�
        //buttonRB.Tick(Input.GetMouseButton(btnRB));
        buttonJstick.Tick(Input.GetKey("mouse 2"));//�м�


        //****���������****
        if (mouseEnable == true)
        {
            Jup = Input.GetAxis(MouseY)  * mouseSensitivityY;
            JRight = Input.GetAxis(MouseX)  * mouseSensitivityX;
            JForward = Input.GetAxis(MouseScrollWheel);
        }
        else
        {
            Jup = (Input.GetKey(keyJUp) ? 1.0f : 0f) - (Input.GetKey(keyJDown) ? 1.0f : 0f);//����Input.GetAxis("Mouse Y")
            JRight = (Input.GetKey(keyJRight) ? 1.0f : 0f) - (Input.GetKey(keyJLeft) ? 1.0f : 0f);//����Input.GetAxis("Mouse X")
        }



        //****�ƶ�����****
        targetDup = (Input.GetKey(KeyUp) ? 1.0f : 0) - (Input.GetKey(KeyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(KeyRight) ? 1.0f : 0) - (Input.GetKey(KeyLeft) ? 1.0f : 0);

        if (!inputEnable)
        {
            targetDup = 0;
            targetDright = 0;
        }

        
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        //��һ��ʹб���ƶ�ʱ��Dmag���ᳬ��1
        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        float Dright2 = tempDAxis.x;
        float Dup2 = tempDAxis.y;

        Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));//����vector3.magnitude
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;

        run = (buttonA.IsPressing && !buttonA.IsDelaying) || buttonA.IsExtending;
        jump = buttonB.OnPressed && (buttonA.IsExtending  || buttonA.IsPressing);
        roll = buttonB.OnPressed && !jump;


        defense = buttonLB.IsPressing;
        attack = buttonC.OnPressed;
        lockon = buttonJstick.OnPressed;


        //run = Input.GetKey(KeyA);
        //defense = Input.GetKey(KeyD);

        ////****��Ծ����****
        //bool newJump = Input.GetKey(KeyB);
        //if (newJump != lastJump && newJump == true)
        //{
        //    jump = true;
        //}
        //else
        //{
        //    jump = false;
        //}
        //lastJump = newJump;

        ////****��������****
        //bool newAttack = Input.GetKey(KeyC);
        //if (newAttack != lastAttack && newAttack == true)
        //{
        //    attack = true;
        //}
        //else
        //{
        //    attack = false;
        //}
        //lastAttack = newJump;
    }
}
