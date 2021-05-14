using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class MySkillEditorWindows : EditorWindow
{

    class PlayerData
    {
        public int _foldIndex = 0;
        public int _roleIndex = 0;
        public List<string> charList = new List<string>();
        public Player player = null;

        public string _foldName="";
        public string _roleName="";

    }

     PlayerData _playerData = new PlayerData();
    List<string> foldNames = new List<string>();
    List<string> AllPrefabsName = new List<string>();


    private void OnEnable()
    {
        ReadFolds();
        ReadAllPrefab();
    }
    /// <summary>
    /// 读取所有预制体
    /// </summary>
    void ReadAllPrefab()
    {
        string[] files = Directory.GetFiles(Application.dataPath + "/Model", "*.prefab", SearchOption.AllDirectories);

        foreach (var item in files)
        {
            //Debug.Log("ALLPrefab"+Path.GetFileNameWithoutExtension(item));
            AllPrefabsName.Add(Path.GetFileNameWithoutExtension(item));
        }
        //AllPrefabsName.Insert(0, "");
    }
    /// <summary>
    /// 读取文件夹
    /// </summary>
    void ReadFolds()
    {
        string[] folds = Directory.GetDirectories(Application.dataPath + "/Model/");
        foreach (var item in folds)
        {
            //Debug.Log(Path.GetFileNameWithoutExtension(item));
            foldNames.Add(Path.GetFileNameWithoutExtension(item));
        }
        foldNames.Sort();
        foldNames.Insert(0, "null");
        foldNames.Insert(1, "all");
    }

    [MenuItem("Tool/技能编辑器")]
    static void InIt()
    {
        if (Application.isPlaying)
        {
            MySkillEditorWindows windows = EditorWindow.GetWindow<MySkillEditorWindows>("技能编辑器");
            windows.Show();
        }
    }

    int foldIndex = 0;
    int roleIndex = 0;
    Dictionary<string, List<string>> PrefabsAtFold = new Dictionary<string, List<string>>();
    private void OnGUI()
    {
        foldIndex = EditorGUILayout.Popup("选择文件夹", foldIndex,foldNames.ToArray());
        if(foldIndex!= _playerData._foldIndex)
        {
            _playerData._foldIndex = foldIndex;
            _playerData._foldName = foldNames[foldIndex];

            List<string> list;

            //按预制体
            if (_playerData._foldName == "all")
            {
                list = AllPrefabsName;
            }
            else
            {

                //按文件夹
                if (!PrefabsAtFold.TryGetValue(foldNames[foldIndex], out list))
                {
                    list = new List<string>();

                    //Debug.Log(list);
                    //字典中不存在
                    //Debug.Log(_playerData._foldName);
                    string[] files = Directory.GetFiles(Application.dataPath + "/Model/" + _playerData._foldName+"/", "*.prefab",SearchOption.AllDirectories);
                    foreach (var item in files)
                    {
                        //if(!string.IsNullOrEmpty(Path.GetFileNameWithoutExtension(item)))
                        //{
                        //    Debug.Log(Path.GetFileNameWithoutExtension(item));
                        //    list.Add(Path.GetFileNameWithoutExtension(item));

                        //}
                        list.Add(Path.GetFileNameWithoutExtension(item));
                    }
                    //_playerData.charList = list;

                    //Debug.Log(_playerData._foldName);

                    PrefabsAtFold[_playerData._foldName] = list;
                }

            }
            _playerData.charList.Clear();
            _playerData.charList.AddRange(list);

        }

        roleIndex = EditorGUILayout.Popup("选择角色", roleIndex, _playerData.charList.ToArray());
        if (roleIndex != _playerData._roleIndex)
        {
            _playerData._roleIndex = roleIndex;
            Debug.Log(_playerData.charList[roleIndex]);

        }


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
