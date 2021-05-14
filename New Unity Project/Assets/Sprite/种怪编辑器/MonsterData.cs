using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class JsonData
{
    public List<MonsterData> datas = new List<MonsterData>();
}
[SerializeField]
public class MonsterData  
{
    public string name;
    public float x;
    public float y;
    public float z;
    public MonsterType type;

    public MonsterData(string name,float x,float y,float z,MonsterType type)
    {
        this.name = name;
        this.x = x;
        this.y = y;
        this.z = z;
        this.type = type;
    }
}

public class MonsterCfg
{
    static MonsterCfg _instance;
    public static MonsterCfg Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MonsterCfg();
                _instance.InIt();
            }
            return _instance;
        }
    }
    public JsonData data=new JsonData();
    string path;
    void InIt()
    {
        path = Application.streamingAssetsPath + @"/monster.json";
        string json = File.ReadAllText(path);

        data.datas = JsonConvert.DeserializeObject<List<MonsterData>>(json);
        //data.datas = list;

        //Debug.Log(list.Count);
        //data = JsonUtility.FromJson<JsonData>(json);
        Debug.Log(data.datas.Count);
    }

    public JsonData GetJsonData()
    {

        return data;


    }
}