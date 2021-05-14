using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Battlesys : UIbase
{
    private Button m_gatherBtn;
    private Slider m_gatherSlider;
    private int m_gatherInsid;

    public override void DoCreate(string path)
    {
        base.DoCreate(path);

        MsgCenter.Instance.AddListener("ClientMsg", RefreshBtn);
        MsgCenter.Instance.AddListener("ServerMsg", ServerNotify);
    }

    int gather_falseCount=2 ;
    void RefreshBtn(Notification obj)
    {
        if (obj.msg.Equals("gather_trigger"))
        {
            m_gatherInsid = (int)obj.data[0];
            if (isOverGather)
            {
                Debug.Log((int)obj.data[1] + "bat....................");
                Debug.Log("BattlesysBattlesys");
                gather_falseCount = (int)obj.data[1];
                m_gatherBtn.gameObject.SetActive(true);
                m_gatherSlider.value = 0;
                GatherSlider.action += Update;
            }
           


        }

        if (obj.msg.Equals("gather_false"))
        {
            m_gatherSlider.gameObject.SetActive(false);

            //m_go.SetActive(false);
            m_gatherBtn.gameObject.SetActive(false);
            GatherSlider.action = null;
        }
    }


    bool isStartSlider = false;
    // Update is called once per frame
    float x = 0;
    void Update()
    {
        //Debug.Log(1324354657687);
        if (isStartGather == true)
        {
            if (m_gatherSlider.value < 1)
            {
                m_gatherSlider.value = Mathf.MoveTowards(m_gatherSlider.value, 1, Time.deltaTime);

            }
            else
            {
                Debug.Log("停");
                m_gatherSlider.value = 0;
                isStartGather = false;
                m_gatherSlider.gameObject.SetActive(false);

                Notification not = new Notification();
                not.Refresh("gather_over",1, gather_falseCount);
                MsgCenter.Instance.SendMsg("ServerMsg", not);
                gather_falseCount -= 1;


                //ByClent_Task
                Notification noti = new Notification();
                noti.Refresh("get", gather_falseCount);//gather_falseCount 为采集物下标
                MsgCenter.Instance.SendMsg("ByClent_Task", noti);
            }
            //Debug.Log(m_gatherSlider.value);
            //debug.log(x + "ddddddddddd");
        }
    }

    bool isOverGather = true;
    private void ServerNotify(Notification obj)
    {
        if (obj.msg.Equals("gather_callback"))
        {
            m_gatherSlider.gameObject.SetActive(true);
            isStartGather = true;
            //SliderTo();
        }
        if (obj.msg.Equals("gather_AllOver"))
        {
            isOverGather = false;
        }
    }

    bool isStartGather = false;
    int count = 0;
   
      
         

  

    public override void DoShow(bool active)
    {
        base.DoShow(active);
        //Debug.Log(m_go+"????????????????????");
        m_gatherBtn=m_go.transform.Find("gather_btn").GetComponent<Button>();
        m_gatherSlider = m_go.transform.Find("gather_slider").GetComponent<Slider>();
        m_gatherBtn.onClick.AddListener(() =>
        {
            isStartSlider = true;

            Notification notift = new Notification();
            Debug.Log("开始采集>>>>>>>>>>>>>>>>>");
            notift.Refresh("gather", 1);
            MsgCenter.Instance.SendMsg("ServerMsg", notift);
        });

        m_gatherSlider = m_go.transform.Find("gather_slider").GetComponent < Slider > ();
        m_gatherBtn.gameObject.SetActive(false);
        m_gatherSlider.gameObject.SetActive(false);
    }

    public override void Destroy()
    {
        base.Destroy();
        MsgCenter.Instance.RemoveListener("ClientMsg", RefreshBtn);
    }
}
