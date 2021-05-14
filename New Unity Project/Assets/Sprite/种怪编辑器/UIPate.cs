﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPate : MonoBehaviour
{

    private GameObject m_go;

    public Text m_name;
    public Slider m_hp;
    public Slider m_mp;
    public GameObject m_gather;
    public List<Image> m_gathers;

    int timerid = -1;

    public void InitPate()
    {
        m_go = GameObject.Instantiate(Resources.Load<GameObject>("pate"));
        m_go.transform.SetParent(UIManager.Instance.m_hudroot.transform);
        m_go.transform.localPosition = Vector3.zero;
        m_go.transform.localScale = Vector3.one;

        m_name = m_go.transform.Find("name").GetComponent<Text>();
        m_hp = m_go.transform.Find("hp").GetComponent<Slider>();
        m_mp = m_go.transform.Find("mp").GetComponent<Slider>();
        m_gather = m_go.transform.Find("gather").gameObject;
        m_gathers = new List<Image>();

        for (int i = 0; i < m_gather.transform.childCount; i++)
        {
            m_gathers.Add(m_gather.transform.GetChild(i).GetComponent<Image>());

        }
    }

    public void Show(bool active)
    {
        if (m_go)
        {
            m_go.SetActive(active);
        }
    }

    /// <summary>
    /// 设置头顶字和血量
    /// </summary>
    /// <param name="name"></param>
    /// <param name="hp"></param>
    /// <param name="mp"></param>
    public void SetData(string name,float hp,float mp)
    {
        m_name.text = name;
        m_hp.value = hp;
        m_mp.value = mp;
    }

    Vector3 camerapos = Vector3.zero;

   public void SetData(int gather)
    {
        for (int i = 0; i < m_gathers.Count; i++)
        {
            m_gathers[i].gameObject.SetActive(i < gather);
        }

    }
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //转换坐标
        camerapos.Set(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, this.gameObject.transform.position.z);
        m_go.transform.position = World.Instance.m_main.WorldToScreenPoint(camerapos);


    }

    ~UIPate()
    {
        m_name = null;
        m_hp = null;
        m_mp = null;
        m_gather = null;
        if (m_gather != null)
        {
            m_gathers.Clear();//清除

        }
        m_gathers = null;
    }
}
