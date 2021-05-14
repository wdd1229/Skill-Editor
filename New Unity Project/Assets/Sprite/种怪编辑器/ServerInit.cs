using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class LocalProps
{
    public static Dictionary<long, SPlayer> players = new Dictionary<long, SPlayer>();

}
public class ServerInit : MonoBehaviour
{
    public Vector3 m_playerPos;//主角
    public Dictionary<int, Vector3> m_otherPosDic;//其他人

    Slider gatherSlider;

    int gather_falseCount=2;

    private void Awake()
    {
        
        MsgCenter.Instance.AddListener("MovePos", (notify) =>
        {
            //Debug.Log("MovePos");
            if (notify.msg.Equals("Player"))
            {

                m_playerPos = (Vector3)notify.data[0];
            }else if (notify.msg.Equals("Other"))
            {
                if (m_otherPosDic == null)
                {
                    m_otherPosDic = new Dictionary<int, Vector3>();

                }
                int insid = (int)notify.data[0];
                Vector3 pos = (Vector3)notify.data[1];
                if (!m_otherPosDic.ContainsKey(insid))
                {
                    m_otherPosDic.Add(insid, pos);

                }
                else
                {
                    m_otherPosDic[insid] = pos;
                }
            }
        });

        //开始采集
        MsgCenter.Instance.AddListener("ServerMsg", (notify) =>
        {
            if (notify.msg.Equals("gather"))
            {
                Debug.Log(notify.data.Length);
                int insid = (int)notify.data[0];
                notify.Refresh("gather_callback", insid);
                MsgCenter.Instance.SendMsg("ServerMsg", notify);
                //Debug.Log(typeof(notify.data[1]));

                //object ob = notify.data[1];
                //Debug.Log(ob);
                //gatherSlider =notify.data[1] as Slider;
                //isStartGather = true;

            }

            if (notify.msg.Equals("gather_false"))
            {
                //检测范围之外 需要隐藏采集 
                Notification notification = new Notification();
                notification.Refresh("gather_false", 0);
                MsgCenter.Instance.SendMsg("ClientMsg",notification);
            }

            //采集结束 也就是slider每走完一次
            if (notify.msg.Equals("gather_over"))
            {
                Notification notification = new Notification();
                //隐藏对应的采集物
                //Debug.Log((int)notify.data[1]+"....................");
                if ((int)notify.data[1] >=0)
                {
                    notification.Refresh("gather_SetActiveFalse", (int)notify.data[0], (int)notify.data[1]);/*ServerMsg*/
                    MsgCenter.Instance.SendMsg("ServerMsg", notification);

                    //Notification not = new Notification();
                    //not.Refresh("RefreshTaskView", gather_falseCount);
                    //MsgCenter.Instance.SendMsg("ServerMsg", not);
                }
                else
                {
                    notification.Refresh("gather_AllOver");
                    MsgCenter.Instance.SendMsg("ServerMsg", notification);
                }
               
                //gather_falseCount -= 1;
                //广播消息 人物模块监听
            }

            //玩家接受任务 添加任务组件
            if (notify.msg.Equals("AcceptTask"))
            {
                int taskid = (int)notify.data[0];
                foreach (var item in LocalProps.players)
                {
                    if (item.Key == 1)
                    {
                        item.Value.components.Add(ComponentType.task, new TaskComponent());
                        item.Value.components[ComponentType.task].Init();

                        //添加完组件 像客户端发送消息 刷新下任务面板
                    }
                }
            }


        });

       
        //初始化角色信息；
        SPlayer splayer = new SPlayer();
        splayer.InitPlayer();
        splayer.m_insid = 1;
        splayer.Hp = 100;

        splayer.components.Add(ComponentType.battle, new BattleComponent());
        LocalProps.players.Add(splayer.m_insid, splayer);

        if (LocalProps.players == null) return;
        foreach (var item in LocalProps.players)
        {
            foreach (var i in item.Value.components)
            {
                i.Value.GetPlayerById = GetPlayer;
                i.Value.Init();
            }
        }

    }

    private SPlayer GetPlayer(long id)
    {
        using (var tmp = LocalProps.players.GetEnumerator())
        {
            while (tmp.MoveNext())
            {
                if (tmp.Current.Key == id)
                {
                    return tmp.Current.Value;
                }
            }
        }
        return null;
    }

    bool isStartGather = false;
    private void Update()
    {
        //if (isStartGather )
        //{
        //    gatherSlider.value += Time.deltaTime;
        //}
    }
}

public enum ComponentType : byte
{
    nil=0,task=1,battle=2,
   
}

public class SkillProp
{
    public float range;
    //范围 。 攻击力 时长 触发节点
}

public class SPlayer
{
    public long m_insid;
    public Vector3 m_pos;

    public float Hp;
    public float Mp;
    public float Atk;

    public List<int> buffs;
    public List<SkillProp> skills;

    public Dictionary<ComponentType, SComponent> components;

    public void InitPlayer()
    {
        buffs = new List<int>();
        skills = new List<SkillProp>();
        components = new Dictionary<ComponentType, SComponent>();
    }

    public void PropOperation(int type,float value)
    {
        switch (type)
        {
            case 1:
                Hp += value;
                break;
            case 2:
                Mp += value;
                break;
            default:
                break;
        }
        Notification m_notify = new Notification();
        m_notify.Refresh("ByServer", type, value);
        MsgCenter.Instance.SendMsg("propchange", m_notify);

    }
}

public class SComponent
{

    public Func<long, SPlayer> GetPlayerById;
    Notification m_notify;
    public virtual void S2CMsg(string cmd,object value)
    {
        if (m_notify == null)
        {
            m_notify = new Notification();
        }
        m_notify.Refresh("ByServer", value);
        MsgCenter.Instance.SendMsg(cmd, m_notify);
    }

    public virtual void Init() { }
}

public class TaskComponent : SComponent
{
   static int count = 1;
    public List<int> Tasks;
    public override void Init()
    {
        MsgCenter.Instance.AddListener("ByClent_Task", (notify) =>
        {
            Debug.Log("任务消息");
            if (notify.msg.Equals("over"))
            {
                int id = (int)notify.data[0];
                S2CMsg("over", id);
            }
            if (notify.msg.Equals("get"))
            { 
                int id = (int)notify.data[0];
                Debug.Log(count);
                //Debug.LogWarning(id);
                //刷新界面
                Notification no = new Notification();
                no.Refresh("RefreshTaskView", count);
                MsgCenter.Instance.SendMsg("ServerMsg", no);
                count++;

            }
        });
    }
}

public class BattleComponent : SComponent
{
    public override void Init()
    {
        MsgCenter.Instance.AddListener("ByClent_Battle", (nott) =>
        {
            Debug.Log(nott.msg);
            if (nott.msg.Equals("atkOther"))
            {
                int atkId = (int)nott.data[0];
                int targetID = (int)nott.data[1];
                int skillID = (int)nott.data[2];
                AtkPlayer(atkId, targetID, skillID);
            }
        });
    }

    private void AtkPlayer(long atk,long target,int skillid)
    {
        SPlayer p1 = GetPlayerById(atk);
        SPlayer p2 = GetPlayerById(target);

        S2CMsg("atkActionPlay", skillid);
    }
}