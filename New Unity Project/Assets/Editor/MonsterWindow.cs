using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Newtonsoft.Json;
using System.IO;



public enum MonsterType
{
    Null=0,
    Normal,//怪物
    Gather,//采集物
    Biaoche,//跟随物
    NPC,
}
public class monsterValue
{
    public bool isselect = true;
    public MonsterType type = MonsterType.Normal;
}
public class MonsterWindow : EditorWindow
{
   static Transform parent;

    static List<GameObject> monsters = new List<GameObject>();
   static List<monsterValue> monste_value = new List<monsterValue>();

   static MonsterWindow monster;

    [MenuItem("MyTool/种怪编辑器")]
    static void InIt()
    {
        monsters.Clear();
        monste_value.Clear();
        parent = GameObject.Find("Mon_Parent").transform;

        
        monster = MonsterWindow.GetWindow<MonsterWindow>("种怪编辑器");
        monster.Show();
    }

    //MonsterType op;
    private void OnGUI()
    {
        //op = (MonsterType)EditorGUILayout.EnumPopup("类型：", op);


        EditorGUILayout.BeginVertical();
        if (monsters.Count > 0)
        {
            for (int i = 0; i < monsters.Count; i++)
            {
                //是显示状态
                if (monsters[i].activeSelf)
                {
                    EditorGUILayout.BeginHorizontal();
                    monste_value[i].isselect = EditorGUILayout.Toggle(monsters[i].name, monste_value[i].isselect);
                   
                    Debug.Log(monste_value[i].type);
                    monste_value[i].type = (MonsterType)EditorGUILayout.EnumPopup("类型：", monste_value[i].type);
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    monste_value[i].isselect = false;
                }
            }

            if (GUILayout.Button("Save"))
            {
                SaveData();
            }
        }
        EditorGUILayout.EndVertical();
    }

   
    /// <summary>
    /// 保存方法
    /// </summary>
    void SaveData()
    {
        List<Data> datas = new List<Data>();
        for (int i = 0; i < monste_value.Count; i++)
        {
            if (monste_value[i].isselect)
            {
                Data data = new Data();
                data.name = monsters[i].name;
                data.type = (int)monste_value[i].type;
                data.pos = monsters[i].transform.position.ToString();
                data.rotate = monsters[i].transform.eulerAngles.ToString();
                datas.Add(data);
            }
        }

        string str = JsonConvert.SerializeObject(datas);
        File.WriteAllText(Application.dataPath + "/Model/Monsters.json", str);
        AssetDatabase.Refresh();
        //foreach (var item in monste_value)
        //{
        //    if (item.isselect)
        //    {
                
        //    }
        //}
        //string str=JsonConvert.SerializeObject()
    }

    // Update is called once per frame
    int cc;
    void Update()
    {
        if (parent)
        {
            cc = parent.childCount;
            if (cc > 0 && cc != monsters.Count)
            {
                monsters.Clear();
                monste_value.Clear();
                for (int i = 0; i < cc; i++)
                {
                    GameObject ga = parent.GetChild(i).gameObject;
                    monsters.Add(ga);
                    monsterValue monsterValue = new monsterValue();
                    monsterValue.isselect = true;
                    monste_value.Add(monsterValue);
                }
                monster.Repaint();

            }
        }
    }
}
