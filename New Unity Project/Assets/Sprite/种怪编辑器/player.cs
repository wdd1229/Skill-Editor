using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.IO;
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
                //技能回馈里

                if (play == null)//玩家挂载 实体 不为空
                {
                    return;
                }

                //判断是否为自己释放
                if ((int)notify.data[0] == 1)
                {
                    
                    int skillid = (int)notify.data[0];

                    Debug.Log("atk===" + (int)notify.data[0]);
                    Debug.Log(skillid.ToString());

                    //去加载 配置 设置声音 动画
                    play.SetData(skillid.ToString());
                    //播放
                    play.play();


                    //读取配置表
                    string st = File.ReadAllText(Application.streamingAssetsPath + "/skill.txt");
                    string[] lines = st.Split('|');

                    //attack|audio1|tx1|100|100|普通攻击

                    m_info.m_HP -= int.Parse(lines[3]);
                    m_info.m_MP -= int.Parse(lines[4]);
                    //血量和蓝量的变化
                    m_pate.SetData(m_info.m_name, (m_info.m_HP) / m_info.m_hpMax, (m_info.m_MP) / m_info.m_mpMax);


                    //面板显示 技能名称

                    GameObject.Find("UIRoot").transform.GetComponent<ShowSkillName>().SetName(lines[5]);


                    //ShowSkillName.SetName("attack");
                }
                else
                {

                }

              
            }
        
        
        });
        //GatherSlider.anim = m_anim;
    }

    Notification notify = new Notification();

  public  bool isMove = false;

    public void JoystickHandlerMoving(float h,float v)
    {
        if (Math.Abs(h) > 0.05f || (Math.Abs(v) > 0.05f))
        {
            MoveByTranslate(new Vector3(m_go.transform.position.x+h,m_go.transform.position.y,m_go.transform.position.z+v),Vector3.forward*Time.deltaTime*1);
            notify.Refresh("Player", m_go.transform.position);

            MsgCenter.Instance.SendMsg("MovePos", notify);
            //Debug.Log(Math.Abs(h) + Math.Abs(v));
            m_anim.SetFloat("Run", Math.Abs(h) + Math.Abs(v));
            isMove = true;
            //m_anim.SetTrigger("run");
        }

    }

    List<string> skillList = new List<string>() { "attack", "skill1", "skill2", "skill3" };
    /// <summary>
    /// 技能 
    /// </summary>
    /// <param name="name"></param>
    float time=1.5f;
    public void JoyButtonHandler(string name)
    {
        //技能之前的一系列判断

        //玩家是否为空
        if (m_anim.gameObject==null)
        {
            Debug.Log("玩家为空");
            return;
        }

        //技能名称 同时技能列表中存在
        if (name == "" || !skillList.Contains(name))
        {
            Debug.Log("技能不为空");
            return;
        }

        //玩家在移动中
        if (isMove)
        {
            return;
        }

        if (time < 1)
        {
            //连续点击 间隔时间
            return;
        }

        //检测消耗

        Debug.Log(m_info.m_HP+"??");
        Debug.Log(m_info.m_MP + "??");

        if (m_info.m_HP < 10 ||m_info.m_MP<50)
        {
            Debug.Log("血量或蓝量不足");
            return;
        }

        ///攻击目标

       

        


        List<SkillBase> componentList;
        switch (name)
        {
            //case "attack":
            default:
                //读取配置表
                string st = File.ReadAllText(Application.streamingAssetsPath+"/skill.txt");
                string[] lines = st.Split('|');

                //attack|audio1|tx1|100|100
                //判断技能名称
                if (lines[0] != "attack")
                {
                    return;
                    Debug.Log("不等于");
                }

                //判断消耗
                if (int.Parse(lines[3]) > m_info.m_HP)
                {
                    Debug.Log("大于当前血量");
                    return;
                }

                if (int.Parse(lines[4]) > m_info.m_MP)
                {
                    Debug.Log("大于当前蓝量");
                    return;
                }




                //发送消息
                Notification m_notify = new Notification();
                //int atkId = (int)nott.data[0];
                //int targetID = (int)nott.data[1];
                //int skillID = (int)nott.data[2];
                m_notify.Refresh("atkOther", 1, 2, 1);
                MsgCenter.Instance.SendMsg("ByClent_Battle", m_notify);

                break;
                //default:

                //    break;
        }
    }
}