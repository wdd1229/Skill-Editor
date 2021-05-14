using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 摇杆 控制
/// </summary>
public class JoyStickMgr : Singleton<JoyStickMgr>
{
    //摇杆父物体
    public GameObject m_joyGo;
    //摇杆
    public ETCJoystick m_joystick;
    //技能按钮
    public List<ETCButton> m_skillBtn;

    HostPlayer m_target;

    //显示 隐藏
    public bool JoyActive
    {
        set
        {
            if (m_joyGo.activeSelf != value)
            {
                m_joyGo.SetActive(value);
            }
        }
    }

    public void SetJoyArg(Camera camera,HostPlayer target)
    {
        m_target = target;
        m_joystick.cameraLookAt = target.m_go.transform;
        m_joystick.cameraTransform = camera.transform;
        SetJoytick();


    }

    public void SetJoytick()
    {
        if (m_joystick && m_target.m_go)
        {
            m_joystick.OnPressLeft.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));

            m_joystick.OnPressRight.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));

            m_joystick.OnPressUp.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));

            m_joystick.OnPressDown.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));

            m_joystick.onMoveEnd.AddListener(() => {   m_target.m_anim.SetFloat("Run", 0); });
        }
        ///技能监听
        if (m_target.m_go && m_skillBtn.Count != 0)
        {
            foreach (var item in m_skillBtn)
            {
                //玩家类里的方法
                item.onUp.AddListener(() => m_target.JoyButtonHandler(item.name));
            }
        }

        
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //m_joystick.OnPressLeft.AddListener(() => { });
    }
}
