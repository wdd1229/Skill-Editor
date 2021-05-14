using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System.IO;
using System;

public class Mons
{
    public string name;
    public string pos;
    public string rotate;
    public string scale;
    public int typeIndex;
}
/// <summary>
/// 读取保存 选中保存人物信息   读取人物信息 生成并设置位置 朝向 大小比例
/// </summary>
public class ScenaWindow : EditorWindow
{
   static GameObject[] gams;
    [MenuItem("MyTool/场景编辑器")]
    static void InIt()
    {
        MonList.Clear();

        ScenaWindow window = EditorWindow.GetWindow<ScenaWindow>();
        window.Show();
        gams = Selection.gameObjects;
  
        Debug.Log(gams.Length);
        foreach (var item in gams)
        {
            Mons mons = new Mons();
            if (item.name.Split('(')[0] != "")
            {             
                mons.name = item.name.Split('(')[0].Trim();
            }
            else
            {
                mons.name = item.name;
            }
            
            
            mons.pos = item.transform.position.ToString();
            mons.rotate = item.transform.eulerAngles.ToString();
            mons.scale = item.transform.localScale.ToString();
            mons.typeIndex = 0;
            MonList.Add(mons);

        }
    }
    string[] Types = new string[] { "Null", "Monster", "NPC", "Player" };
   static List<Mons> MonList = new List<Mons>();
   
    private void OnGUI()
    {
      
        if (GUILayout.Button("读取"))
        {        
            string str = File.ReadAllText(Application.dataPath + "/Model/1.json");
            MonList.Clear();
            MonList /*List <Mons> list*/ = JsonConvert.DeserializeObject<List<Mons>>(str);
          
            foreach (var item in MonList)
            {
            
                Debug.Log(Application.dataPath + "/Model/Player/" + item.name + ".prefab");
                GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Model/Player/" + item.name + ".prefab");
                Debug.Log(obj);
                GameObject it = GameObject.Instantiate(obj);

                it.transform.position= StringToVector3(item.pos);
                it.transform.eulerAngles= StringToVector3(item.rotate);
                it.transform.localScale= StringToVector3(item.scale);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(item.name);

                item.typeIndex = EditorGUILayout.Popup(item.typeIndex,Types);


                EditorGUILayout.EndHorizontal();
                
            }

        }


        if (GUILayout.Button("保存"))
        {
            Debug.Log(MonList.Count);
            
            string str = JsonConvert.SerializeObject(MonList);
            Debug.Log(str);

            File.WriteAllText(Application.dataPath + "/Model/1.json", str);
            AssetDatabase.Refresh();
        }

      

         
        foreach (var item in MonList)
        {
           
       
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(item.name);
            item.typeIndex = EditorGUILayout.Popup(item.typeIndex, Types);
            
                 
            EditorGUILayout.EndHorizontal();
             
        }

        
    }

    public Vector3 StringToVector3(string str)
    {
        string st = "";
        foreach (var item in str)
        {
            if (item == '(' ||item==')' )
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
            brr[i]=float.Parse(arr[i]);
        }
        return new Vector3(brr[0], brr[1], brr[2]);
        //return new Vector3(0, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


