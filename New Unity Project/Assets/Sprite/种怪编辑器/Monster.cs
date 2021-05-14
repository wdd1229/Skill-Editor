using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : ObjectBase
{
    public monster_info m_info;
    public Monster(MonsterType type, monster_info info)
    {
        info.m_type = type;
        m_info = info;
        m_insID = info.ID;
        m_modelPath = info.m_res;
    }

    public override void OnCreat()
    {
        base.OnCreat();
    }
}

/// <summary>
/// 正常怪物
/// </summary>
public class Normal : Monster
{
    public Normal(monster_info info) : base(MonsterType.Normal, info)
    {

    }

    public Normal(object_info info) : base(MonsterType.Normal, new monster_info(MonsterType.Normal, info))
    {

    }

    public override void CreatObj(MonsterType type)
    {
        SetPos(m_local_pos);
        base.CreatObj(type);
    }

    public override void OnCreat()
    {
        base.OnCreat();

        m_pate = m_go.AddComponent<UIPate>();
        m_pate.InitPate();
        m_pate.m_name.text = m_info.m_res;
        m_pate.m_name.gameObject.SetActive(true);
        m_pate.m_hp.gameObject.SetActive(true);
        m_pate.m_mp.gameObject.SetActive(false);
        m_pate.m_gather.gameObject.SetActive(false);
    }
}

/// <summary>
/// 采集物
/// </summary>
public class Gather : Monster
{
    public Gather(monster_info info) : base(MonsterType.Gather, info)
    {

    }

    public Gather(object_info info) : base(MonsterType.Gather, new monster_info(MonsterType.Gather, info))
    {

    }

    public override void CreatObj(MonsterType type)
    {
        SetPos(m_info.m_pos);
        base.CreatObj(type);
    }

    int gather_falseCount = 2;
    public override void OnCreat()
    {
        base.OnCreat();
        //范围检测
        StaticCircleCheck check = m_go.AddComponent<StaticCircleCheck>();

        //监听采集信息 隐藏对应采集物
        MsgCenter.Instance.AddListener("ServerMsg", ServerNotify);

        check.m_taget = World.Instance.m_player.m_go;
        check.m_call = (isenter) =>
        {
            if (isenter)
            {
                Debug.Log("进入" + m_info.m_res + "范围" + isenter);
                Notification notify = new Notification();
                notify.Refresh("gather_trigger", m_info.ID, gather_falseCount);
                MsgCenter.Instance.SendMsg("ClientMsg", notify);
                //gather_falseCount -= 1;
            }
            else
            {
                //范围之外
                Notification not = new Notification();
                not.Refresh("gather_false", m_info.ID);
                MsgCenter.Instance.SendMsg("ServerMsg", not);
            }
           

            //m_pate.m_hp.gameObject.SetActive(true);
        };

        m_pate = m_go.AddComponent<UIPate>();
        m_pate.InitPate();

        m_pate.m_name.gameObject.SetActive(false);
        m_pate.m_hp.gameObject.SetActive(false);
        m_pate.m_mp.gameObject.SetActive(false);
        m_pate.m_gather.gameObject.SetActive(true);




    }

    public void ServerNotify(Notification obj)
    {
        if (obj.msg.Equals("gather_SetActiveFalse"))
        {
            int insID = (int)obj.data[0];

            m_pate.SetData((int)obj.data[1]);
            gather_falseCount -= 1;
        }
    }

    public void RefreshGatherCount(int cnt)
    {
        if (m_pate && m_pate.m_gathers.Count > 0)
        {
            for (int i = 0; i < m_pate.m_gathers.Count; i++)
            {
                m_pate.m_gathers[i].gameObject.SetActive(cnt <= i + 1);

            }
        }
    }
}

/// <summary>
/// 跟随
/// </summary>
public class Biaoche : Monster
{
    public Biaoche(monster_info info) : base(MonsterType.Biaoche, info)
    {

    }

    public Biaoche(object_info info) :
        base(MonsterType.Biaoche, new monster_info(MonsterType.Biaoche, info))
    {

    }

    public override void CreatObj(MonsterType type)
    {
        SetPos(m_info.m_pos);
        base.CreatObj(type);
    }

    public override void OnCreat()
    {
        base.OnCreat();

        StaticCircleCheck check = m_go.AddComponent<StaticCircleCheck>();
        check.m_call = (isenter) =>
        {
            Debug.LogError("进入Biaoche检测范围");
            //follow跟随
        };
    }

}
/// <summary>
/// NPC
/// </summary>
public class NPCObj : ObjectBase
{
    public npc_info m_info;
    public NPCObj(npc_info info)
    {
        m_info = info;
        m_insID = info.ID;
        m_modelPath = info.m_res;
    }

    public NPCObj(int plot,object_info info)
    {
        m_info = new npc_info(plot, info);
        m_insID = info.ID;
        m_modelPath = info.m_res;
    }

    public override void CreatObj(MonsterType type)
    {
        SetPos(m_info.m_pos);
        base.CreatObj(type);
    }
}



//public class PlayerObj : ObjectBase
//{
//    public player_info m_info;
//    public PlayerObj(player_info info)
//    {
//        m_info = info;
//    }

//    public override void SetPos(Vector3 pos)
//    {
//        base.SetPos(pos);
//    }
//    public void SetPos(Vector3 pos, float speed)
//    {
//        //平滑移动
//    }

//    public override void OnCreat()
//    {
//        base.OnCreat();

//        m_pate = m_go.AddComponent<UIPate>();
//        m_pate.InitPate();
//        m_pate.m_gather.SetActive(false);
//        m_pate.SetData(m_info.m_name, m_info.m_HP / m_info.m_hpMax, m_info.m_MP / m_info.m_mpMax);

//    }

//    public void AddBuff(string path)
//    {
//        HostPlayer
//    }

//}
