using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskSys : UIbase
{
    public Text jindu;
    public Text taskName;
    public Button acceptBtn;
    public override void DoShow(bool active)
    {
        base.DoShow(active);
        taskName = m_go.transform.Find("TaskSys/Text").GetComponent<Text>();
        acceptBtn = m_go.transform.Find("TaskSys").GetComponent<Button>();
        jindu = m_go.transform.Find("TaskSys/jindu").GetComponent<Text>();

        jindu.text = "";



        acceptBtn.onClick.AddListener(() =>
        {
            taskName.text = GameData.Instance.GetTaskDataById(1).taskName;
            //向服务器发送消息
            Notification notification = new Notification();
            notification.Refresh("AcceptTask", 1);//服务器收到之后 添加任务组件

            MsgCenter.Instance.SendMsg("ServerMsg", notification);
        });
    }

    public override void DoCreate(string path)
    {
        base.DoCreate(path);

        MsgCenter.Instance.AddListener("ServerMsg", ( not) =>
        {
            if (not.msg.Equals("RefreshTaskView"))
            {
                if ((int)not.data[0] <= 3)
                {
                    Debug.Log("刷新");
                    Debug.Log(((int)not.data[0]));
                    jindu.text = (int)not.data[0] + "/3";
                }
                else
                {
                    Notification nott = new Notification();
                    nott.Refresh("over", 3);
                    MsgCenter.Instance.SendMsg("ByClent_Task", nott);
                }
                
                
                
            }
        });

        MsgCenter.Instance.AddListener("over", (nott) =>
        {
            if (nott.msg.Equals("ByServer"))
            {
                jindu.text = "任务完成";
            }
        });
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}
