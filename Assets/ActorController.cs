using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public PlayerInput pi;
    public float walkSpeed = 2.0f;
    public float runMultipli = 2.0f;
    public float jumpVelocity = 5.0f;
    public float rollVelocity = 3.0f;

    [SerializeField]
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 planarVec;
    private Vector3 thrustVec;

    private bool lockPlanar = false;
    // Start is called before the first frame update
    void Awake()
    {
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //地面动画播放 平滑过渡
        anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), pi.run ? 2.0f : 1.0f, 0.5f));
        //翻滚动画播放
        if (rigid.velocity.magnitude > 1.3f)
        {
            anim.SetTrigger("roll");
        }
        //跳跃动画
        if (pi.jump) anim.SetTrigger("jump");
        //攻击动画
        if (pi.attack)
        {
            anim.SetTrigger("attack");
        }
        //模型转向 平滑过渡
        if (pi.Dmag > 0.1f) {
            model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);
        }
        //移速计算 空中锁死速度
        if (!lockPlanar)
        {
            planarVec = pi.Dmag * model.transform.forward * (pi.run ? runMultipli : 1.0f);
        }
        
        
    }

    private void FixedUpdate()
    {
        //rigid.position += movingVec * Time.fixedDeltaTime * walkSpeed;
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
    }


    /// <summary>
    /// Message processing block
    /// </summary>

    //jump动画enter行为调用(FSMOnEnter)
    public void OnJumpEnter()
    {
        thrustVec = new Vector3(0, jumpVelocity, 0);
        pi.inputEnable = false;
        lockPlanar = true;
    }

    //sensor调用(OnGroundSensor)
    public void IsGround()
    {
        anim.SetBool("isGround", true);

    }

    public void IsNotGround()
    {
        anim.SetBool("isGround", false);

    }

    public void OnGroundEnter()
    {
        pi.inputEnable = true;
        lockPlanar = false;
    }

    public void OnFallEnter()
    {
        pi.inputEnable = false;
        lockPlanar = true;
    }

    public void OnRollEnter()
    {
        thrustVec = new Vector3(0, rollVelocity, 0);
        pi.inputEnable = false;
        lockPlanar = true;
    }

    public void OnJabEnter()
    {
        pi.inputEnable = false;
        lockPlanar = true;
    }

    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity");
    }

}
