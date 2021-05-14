using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class player : ObjectBase
{
    public player_info m_info;
    public player(player_info info)
    {
        m_info = info;
    }
    
    public override void SetPos(Vector3 pos)
    {
        base.SetPos(pos);
    }

    public void SetPos(Vector3 pos,float speed)
    {
        //平滑移动
    }

    public override void OnCreat()
    {
        base.OnCreat();
        m_pate = m_go.AddComponent<UIPate>();
        m_pate.InitPate();
        m_pate.m_gather.SetActive(false);
        m_pate.SetData(m_info.m_name, m_info.m_HP / m_info.m_hpMax, m_info.m_MP / m_info.m_mpMax);

    }

    public  void AddBuff(string path)
    {

    }

    
    
}

public class Notification
{
    public string msg;
    public object[] data;
    public void Refresh(string msg,params object[] data)
    {
        this.msg = msg;
        this.data = data;
    }

    public void Clear()
    {
        msg = string.Empty;
        data = null;
    }
}

public class HostPlayer : player
{
    Player play;//技能
    public HostPlayer(player_info info) : base(info)
    {
        m_insID = info.ID;
        m_modelPath = info.m_res;
    }
    public override void CreatObj(MonsterType type)
    {
        SetPos(m_info.m_pos);
        base.CreatObj(type);
    }

    public override void OnCreat()
    {
        base.OnCreat();
        m_anim = m_go.GetComponent<Animator>();
        play = m_go.AddComponent<Player>();

        //初始化
        play.InItData();

        MsgCenter.Instance.AddListener("atkActionPlay", (notify) => {
            if (notify.msg.Equals("ByServer"))
            {
                int skillid = (int)notify.data[0];
                Debug.Log(skillid.ToString());
                play.SetData(skillid.ToString());
                play.play();
            }
        
        
        });
        //GatherSlider.anim = m_anim;
    }

    Notification notify = new Notification();

    public void JoystickHandlerMoving(float h,float v)
    {
        if (Math.Abs(h) > 0.05f || (Math.Abs(v) > 0.05f))
        {
            MoveByTranslate(new Vector3(m_go.transform.position.x+h,m_go.transform.position.y,m_go.transform.position.z+v),Vector3.forward*Time.deltaTime*1);
            notify.Refresh("Player", m_go.transform.position);

            MsgCenter.Instance.SendMsg("MovePos", notify);
            //Debug.Log(Math.Abs(h) + Math.Abs(v));
            m_anim.SetFloat("Run", Math.Abs(h) + Math.Abs(v));

            //m_anim.SetTrigger("run");
        }

    }

    /// <summary>
    /// 技能 
    /// </summary>
    /// <param name="name"></param>
    public void JoyButtonHandler(string name)
    {
        List<SkillBase> componentList;
        switch (name)
        {
            case "attack":
                //发送消息
                Notification m_notify = new Notification();
                //int atkId = (int)nott.data[0];
                //int targetID = (int)nott.data[1];
                //int skillID = (int)nott.data[2];
                m_notify.Refresh("atkOther", 1, 2, 1);
                MsgCenter.Instance.SendMsg("ByClent_Battle", m_notify);

                break;
            default:
                break;
        }
    }
}