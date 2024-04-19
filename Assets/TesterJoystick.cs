using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterJoystick : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        //Vertical Horizontal
        //print(Input.GetAxis("AxisX"));//Horizontal
        //print(Input.GetAxis("AxisY"));//Vertical
        //print("Axis4: " + Input.GetAxis("Axis4"));
        //print("Axis5: " + Input.GetAxis("Axis5"));
        //print("btn0: " + Input.GetButtonDown("Btn0"));//A
        //print("btn1: " + Input.GetButtonDown("Btn1"));//B
        //print("btn2: " + Input.GetButtonDown("Btn2"));//X
        //print("btn3: " + Input.GetButtonDown("Btn3"));//Y
        print("PadH: " + Input.GetAxis("Axis6"));
        print("PadV: " + Input.GetAxis("Axis7"));
        print("LB: " + Input.GetButtonDown("Btn4"));
        print("RB: " + Input.GetButtonDown("Btn5"));
        print("LT: " + Input.GetAxis("Axis9"));
        print("RT: " + Input.GetAxis("Axis10"));

    }
}
