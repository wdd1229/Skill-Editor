using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Anim : SkillBase
{
    Player player;

    Animator anim;

    public AnimationClip animClip;
    AnimatorOverrideController controller;

    public Skill_Anim(Player _player)
    {
        player = _player;
        anim = player.gameObject.GetComponent<Animator>();
        controller = player.overrideController;
    }

    public override void Init()
    {
        controller["Attack1"] = animClip;
    }

    public void SetAnimClip(AnimationClip _animClip)
    {
        animClip = _animClip;
        name = animClip.name;
        controller["Attack1"] = animClip;
    }

    public override void Play()
    {
        base.Play();
        anim.StopPlayback();
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("att"))
        {
            //return;
        }
        else
        {
            
            //Debug.Log(stateInfo.IsName.ToString());
            Debug.Log("攻击");
            anim.SetTrigger("att");

        }


    }
    public override void Stop()
    {
        base.Play();
        anim.StartPlayback();


    }

}
