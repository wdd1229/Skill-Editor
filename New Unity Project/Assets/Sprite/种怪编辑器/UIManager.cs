using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject m_uiroot;//人物父物体
    public GameObject m_hudroot;//ui父物体

    public Dictionary<string, UIbase> m_uiDic;

    public void InIt(GameObject root,GameObject hud)
    {
        m_uiroot = root;
        m_hudroot = hud;
        m_uiDic = new Dictionary<string, UIbase>();

        m_uiDic.Add("Lobby", new Lobbysys());
        m_uiDic.Add("battle", new Battlesys());
        m_uiDic.Add("TaskPanel", new TaskSys());//任务
        m_uiDic.Add("minimap", new MinimapSys());//地图

        //Open("Lobby"); //左上角人物信息
        Open("battle");

        Open("TaskPanel");//任务


        Open("minimap");//右上角小地图

    }

    public void Open(string key)
    {
        UIbase ui;
        if(m_uiDic.TryGetValue(key,out ui))
        {
            ui.DoCreate(key);
        }
    }

    public void Close(string key)
    {
        UIbase ui;
        if (m_uiDic.TryGetValue(key,out ui))
        {
            ui.Destroy();
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
