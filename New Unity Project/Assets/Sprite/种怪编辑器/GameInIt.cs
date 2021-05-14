using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEditor;

public class Data
{
    public string name;
    public int type;
    public string pos;
    public string rotate;
}
public class GameInIt : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] OnDontDestorys;//不删除物体

    string[] typeNames = new string[] { "","Boy", "Teddy", "TeddyBear" , "Teddy" };

    public ETCJoystick joystick;
    public List<ETCButton> Attacks;
    void Start()
    {
        foreach (var item in OnDontDestorys)
        {
            DontDestroyOnLoad(item);
        }
        ///切换了场景 添加回掉方法
        GameSceneUntil.LoadSceneAsync("lobby", () =>
        {
            //摇杆管理类
            JoyStickMgr.Instance.m_joyGo = OnDontDestorys[0];
            JoyStickMgr.Instance.m_joystick = joystick;
            JoyStickMgr.Instance.m_skillBtn = Attacks;

            //解析数据
            GameData.Instance.InitByRoleName("Teddy");
            //初始化任务数据
            GameData.Instance.InitTaskData();
            World.Instance.InIt();
        });

        //string str=File.ReadAllText(Application.dataPath + "/Model/Monsters.json");
        //datas = JsonConvert.DeserializeObject<List<Data>>(str);

        //    Null = 0,
        //Normal,//怪物
        //Gather,//采集物
        //Biaoche,//跟随物
        //NPC,

        //foreach (var item in datas)
        //{
        //    GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Model/Player/"+ typeNames[item.type] + ".prefab");
        //    GameObject it = GameObject.Instantiate(obj,GameObject.Find("Mon_Parent").transform);
        //    it.transform.position = StringToVector3(item.pos);
        //    it.transform.eulerAngles = StringToVector3(item.rotate);

        //    Player player = it.GetComponent<Player>();

        //    if (item.type == 1)
        //    {

        //    }else if (item.type == 2)
        //    {

        //    }
        //    else if (item.type == 3)
        //    {

        //    }
        //    else if (item.type == 4)
        //    {

        //    }
        //}


    }

    public Vector3 StringToVector3(string str)
    {
        string st = "";
        foreach (var item in str)
        {
            if (item == '(' || item == ')')
            {
                continue;
            }
            else
            {
                st += item;
            }
        }

        string[] arr = st.Split(',');

        float[] brr = new float[3];
        for (int i = 0; i < arr.Length; i++)
        {
            //Debug.Log(arr[i]);
            //brr[i]= Convert.ToInt32(arr[i]);
            brr[i] = float.Parse(arr[i]);
        }
        return new Vector3(brr[0], brr[1], brr[2]);
        //return new Vector3(0, 0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
