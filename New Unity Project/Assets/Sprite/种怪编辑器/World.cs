using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MonsterType
{
    Null=0,
    Normal,
    Gather,
    Biaoche,
    NPC,
}

public class object_info
{
    public int ID;
    public string m_name;
    public Vector3 m_pos;
    public string m_res;
    public MonsterType m_type;
}

public class player_info : object_info
{
    public int m_level;
    public float m_HP;
    public float m_hpMax;
    public float m_MP;
    public float m_mpMax;
}

public class npc_info : object_info
{
    public int m_plotId = 0;//0 不响应；
    public npc_info(int plot,object_info info)
    {
        ID = info.ID;
        m_name = info.m_name;
        m_pos = info.m_pos;
        m_res = info.m_res;
        m_type = MonsterType.NPC;
    }
}
public class monster_info : object_info
{
    public monster_info(MonsterType type,object_info info)
    {
        ID = info.ID;
        m_name = info.m_name;
        m_pos = info.m_pos;
        m_res = info.m_res;
        m_type = type;
    }
}

public class World : Singleton<World>      
{
  public  Dictionary<int,ObjectBase> m_insDic = new Dictionary<int, ObjectBase>();

    public GameObject npcroot;
    public Camera m_main;

    public float xlength;
    public float ylength;
    public HostPlayer m_player;

    public void InIt()
    {
        GameObject plan = GameObject.Find("Plane");
        Vector3 length = plan.GetComponent<MeshFilter>().mesh.bounds.size;

        xlength = length.x * plan.transform.lossyScale.x;
        ylength = length.z * plan.transform.lossyScale.z;
        Debug.Log("地图尺寸为 x:" + xlength + "y:" + ylength);

        m_main = GameObject.Find("Main Camera").GetComponent<Camera>();
        npcroot = GameObject.Find("Mon_Parent");
        //UIRoot
        UIManager.Instance.InIt(GameObject.Find("UIRoot"), GameObject.Find("HUD"));


        player_info info = new player_info();
        info.ID = 0;
        info.m_name = "tony";
        info.m_level = 9;
        info.m_pos = new Vector3(-4,0,-4);
        info.m_res = "player";
        info.m_HP = 2000;
        info.m_MP = 1000;
        info.m_hpMax = 2000;
        info.m_mpMax = 2000;

        //player 
        m_player = new HostPlayer(info);
        m_player.CreatObj(MonsterType.Null);
        //JoyStickMgr.Instance.setjo

        JoyStickMgr.Instance.SetJoyArg(m_main, m_player);
        JoyStickMgr.Instance.JoyActive = true;

        CreateIns();

        MsgCenter.Instance.AddListener("AutoMove", (notify) =>
        {
            this.AutoMoveByInsId((int)notify.data[0], (Vector3)notify.data[1]);
        });

        MsgCenter.Instance.AddListener("AddBuff", (notify) =>
        {
            int insid = (int)notify.data[0];
            int buffid = (int)notify.data[1];
            ObjectBase p = m_insDic[insid];
            if (p is player)
            {
                BuffSystem.Instance.AddBuff(p as player, buffid);
            }
        });

        MsgCenter.Instance.AddListener("ReMoveBuff", (notify) =>
        {

        });
    }

    private void CreateIns()
    {
        JsonData data = MonsterCfg.Instance.GetJsonData();
        object_info info;
        //Debug.Log("加载")
        for (int i = 0; i < data.datas.Count; i++)
        {
            info = new object_info();
            info.ID = m_insDic.Count + 1;
            info.m_name = string.Format("{0}({1})", data.datas[i].name, info.ID);
            info.m_res = data.datas[i].name;
            info.m_pos = new Vector3(data.datas[i].x, data.datas[i].y, data.datas[i].z);
            info.m_type = data.datas[i].type;
            CreatObj(info);
        }

    }

    ObjectBase monster = null;
    private void CreatObj(object_info info)
    {
        monster = null;
        if (info != null)
        {
            if (info.m_type == MonsterType.Normal)
            {

                monster = new Normal(info);
            }else if(info.m_type == MonsterType.Gather)
            {
                monster = new Gather(info);
            }
            else if (info.m_type == MonsterType.NPC)
            {
                monster = new NPCObj(1,info);
            }
        }

        if (monster == null)
        {
            monster.CreatObj(info.m_type);
            monster.m_go.transform.SetParent(npcroot.transform, false);
            m_insDic.Add(info.ID, monster);
        }
        else
        {
            Debug.Log("生成失败");
        }
    }

    public void AutoMoveByInsId(int target,Vector3 pos)
    {
        using(var tmp = m_insDic.GetEnumerator())
        {
            while (tmp.MoveNext())
            {
                if (target == tmp.Current.Key)
                {
                    //移动
                    tmp.Current.Value.AutoMove(pos, pos);
                }
            }
        }
    }
   
}


