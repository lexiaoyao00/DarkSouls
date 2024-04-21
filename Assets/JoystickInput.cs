using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickInput : IUserInput
{
    [Header("===== Joystick Setting  =====")]
    public string axisX = "AxisX";
    public string axisY = "AxisY";
    public string axisJRight = "Axis4";
    public string axisJup = "Axis5";
    public string btnA = "Btn0";
    public string btnB = "Btn1";
    public string btnC = "Btn2";
    public string btnD = "Btn3";
    public string btnLB = "Btn4";
    public string btnRB = "Btn5";
    public string btnJstick = "Btn9";

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

        buttonA.Tick(Input.GetButton(btnA));
        buttonB.Tick(Input.GetButton(btnB));
        buttonC.Tick(Input.GetButton(btnC));
        buttonD.Tick(Input.GetButton(btnD));
        buttonLB.Tick(Input.GetButton(btnLB));
        buttonRB.Tick(Input.GetButton(btnRB));
        buttonJstick.Tick(Input.GetButton(btnJstick));
        
        //print(buttonJstick.OnPressed);

        //****摄像机控制****
        Jup = Input.GetAxis(axisJup);
        JRight = Input.GetAxis(axisJRight);

        //****移动控制****
        targetDup = Input.GetAxis(axisY);
        targetDright = Input.GetAxis(axisX);

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

        run = (buttonA.IsPressing && !buttonA.IsDelaying) || buttonA.IsExtending;
        jump = buttonA.OnPressed && buttonA.IsExtending;
        roll = buttonA.OnReleased && buttonA.IsDelaying;


        defense = buttonLB.IsPressing;
        attack = buttonC.OnPressed;
        lockon = buttonJstick.OnPressed;

    }

}
