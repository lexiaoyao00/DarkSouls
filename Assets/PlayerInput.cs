using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("===== Key Setting  =====")]
    public string KeyUp = "w";
    public string KeyDown = "s";
    public string KeyLeft = "a";
    public string KeyRight = "d";

    public string KeyA;
    public string KeyB;
    public string KeyC;
    public string KeyD;

    [Header("===== Output Signals  =====")]
    public float Dup;
    public float Dright;
    public float Dmag;
    public Vector3 Dvec;

    // 1.pressing signal
    public bool run;
    // 2.trigger once signal
    public bool jump;
    private bool lastJump;
    // 3.double trigger

    [Header("===== Others  =====")]
    public bool inputEnable;

    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    private Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y)/2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x)/2.0f);

        return output;
    }
}
