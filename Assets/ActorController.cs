using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public IUserInput pi;
    public float walkSpeed = 2.0f;
    public float runMultipli = 2.0f;
    public float jumpVelocity = 5.0f;
    public float rollVelocity = 3.0f;

    [Space(10)]
    [Header("===== Friction Setting =====")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    //[SerializeField]
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 planarVec;
    private Vector3 thrustVec;
    private bool canAttack;
    private CapsuleCollider col;
    private float lerpTarget;
    private Vector3 deltaPos;

    private bool lockPlanar = false;
    // Start is called before the first frame update
    void Awake()
    {

        IUserInput[] inputs = GetComponents<IUserInput>();
        foreach (var input in inputs)
        {
            if(input.enabled  == true)
            {
                pi = input;
                break;
            }
        }
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //地面动画播放 平滑过渡
        anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), pi.run ? 2.0f : 1.0f, 0.5f));
        //防御举盾
        anim.SetBool("defense", pi.defense);
        //翻滚动画播放
        if (pi.roll || rigid.velocity.magnitude >7f )
        {
            anim.SetTrigger("roll");
            canAttack = false;
        }
        //跳跃动画
        if (pi.jump) {
            anim.SetTrigger("jump");
            canAttack = false;
        }
        //攻击动画
        if (pi.attack && CheckState("ground") && canAttack)
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
        rigid.position += deltaPos;

        //rigid.position += movingVec * Time.fixedDeltaTime * walkSpeed;
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }

    private bool CheckState(string stateName,string layerName = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
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
        canAttack = true;
        col.material = frictionOne;
    }

    public void OnGroundExit()
    {
        col.material = frictionZero;
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

    public void OnAttack1hAEnter()
    {
        pi.inputEnable = false;
        //lockPlanar = true;
        lerpTarget = 1.0f;
    }

    public void OnAttack1hAUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1hAVelocity");
        anim.SetLayerWeight(anim.GetLayerIndex("attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.1f));
    }

    public void OnAttackIdleEnter()
    {
        pi.inputEnable = true;
        //lockPlanar = false;
        //anim.SetLayerWeight(anim.GetLayerIndex("attack"), 0);
        lerpTarget = 0f;
    }

    public void OnAttackIdleUpdate()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.1f));

    }

    public void OnUpdateRM(object _deltaPos)
    {
        if (CheckState("attack1hC", "attack"))
        {
            deltaPos += (0.8f * deltaPos + 0.2f * ((Vector3)_deltaPos)) / 2;

        }
    }
}
