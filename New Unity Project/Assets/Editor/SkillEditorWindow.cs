using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class SkillEditorWindow : EditorWindow
{
    // Start is called before the first frame update




    private void OnEnable()
    {
        ReadFolds();//读取出文件夹
        ReadAllPrefab();//读出所有预制体
    }

    //[MenuItem("MyTool/技能编辑器")]
    static void InIt()
    {
        //运行模式下执行
        if (Application.isPlaying)
        {

            SkillEditorWindow skillEditor = EditorWindow.GetWindow<SkillEditorWindow>("选择人物");
            skillEditor.Show();
        }

    }


    /// <summary>
    /// 读取文件夹
    /// </summary>
    void ReadFolds()
    {
        string[] foldNames = Directory.GetDirectories(Application.dataPath + "/Model");
        foreach (var item in foldNames)
        {

            ;//拿到最后一级的名字

            folds.Add(Path.GetFileNameWithoutExtension(item));
            //存入集合
        }
        folds.Sort();
        folds.Insert(0, "null");
    }

    void ReadAllPrefab()
    {
        string[] files = Directory.GetFiles(Application.dataPath + "/Model", "*.prefab", SearchOption.AllDirectories);

        foreach (var item in files)
        {
            //Debug.Log(Path.GetFileNameWithoutExtension(item));
            prefabs.Add(Path.GetFileNameWithoutExtension(item));
        }
    }
    int foldIndex = 0;
    int roleIndex = 0;

    int roleIndex2 = 0;

    List<string> folds = new List<string>();
    List<string> prefabs = new List<string>();
    string foldName = "";

    Player player;

    Dictionary<string, List<string>> foldAtPrefabs = new Dictionary<string, List<string>>();
    //List<string> roleList = new List<string>();
    private void OnGUI()
    {
        foldIndex = EditorGUILayout.Popup("选择文件夹", foldIndex, folds.ToArray());

        foldName = folds[foldIndex];
        //Debug.Log(foldAtPrefabs[foldName]);

        //List<string> pres = new List<string>();
        List<string> ttt = new List<string>();

        if (foldName == "null")
        {
            Debug.Log("NULLNULL");
            if (player)
            {
                Destroy(player.gameObject);
                roleIndex = 0;
            }
          
        }

        if (!foldAtPrefabs.ContainsKey(foldName))
        {
            if (foldName != "null")
            {
                Debug.Log(foldName);

                //Debug.Log("????");
                //List<string> pres=new List<string>();
                string[] sttr = Directory.GetFiles(Application.dataPath + "/Model/" + foldName + "/", "*.prefab", SearchOption.AllDirectories);
                foreach (var item in sttr)
                {
                    //pres.Add(Path.GetFileNameWithoutExtension(item));
                    //Debug.Log(Path.GetFileNameWithoutExtension(item));
                    ttt.Add(Path.GetFileNameWithoutExtension(item));


                }
                ttt.Insert(0,"null");
                foldAtPrefabs.Add(foldName, ttt);
                //foldAtPrefabs[foldName].Insert(0, "");

                roleIndex = 0;
            }
            else
            {
                ttt.Insert(0,"null");
                foldAtPrefabs.Add(foldName, ttt);
                //foldAtPrefabs[foldName].Insert(0, "");

                roleIndex = 0;

                //if (player)
                //{
                //    //Debug.Log(">>><<<");
                //    Destroy(player.gameObject);
                //}
            }
            //pres.Insert(0, "");

        }
        else
        {
            //foldAtPrefabs[foldName].Insert(0, "");
        }




        roleIndex = EditorGUILayout.Popup("选择人物", roleIndex, foldAtPrefabs[foldName].ToArray());
        if (roleIndex != roleIndex2)
        {
            
            if (player )
            {
                //Debug.Log(">>><<<");
                Destroy(player.gameObject);
            }

            if (roleIndex != 0)
            {
                player = Player.CreatRole(foldName, foldAtPrefabs[foldName][roleIndex]);
                roleIndex2 = roleIndex;
            }
            
          

        }

        //Debug.Log(foldName);

    }
    // Update is called once per frame
    void Update()
    {

    }
}
