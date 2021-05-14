using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
    public GameObject m_go;
    public Vector3 m_local_pos;
    public Animator m_anim;
    public UIPate m_pate;
    public MonsterType m_type;
    public int m_insID;
    public string m_modelPath;
    public ObjectBase()
    {

    }
    
    public virtual void CreatObj(MonsterType type)
    {
        m_type = type;
        if(!string.IsNullOrEmpty(m_modelPath) && m_insID >= 0)
        {
            Debug.Log(m_modelPath);
            m_go = (GameObject)GameObject.Instantiate(Resources.Load(m_modelPath));
            m_go.name = m_insID.ToString();
            m_go.transform.position = m_local_pos;
            if (m_go)
            {

                OnCreat();
            }
        }
    }

    public virtual void OnCreat()
    {

    }

    public virtual void SetPos(Vector3 pos)
    {
        m_local_pos = pos;
    }

    public void MoveByTranslate(Vector3 look,Vector3 move)
    {
        m_go.transform.LookAt(look);
        m_go.transform.Translate(move);
    }

    public void AutoMove(Vector3 look,Vector3 move)
    {
        MoveByTranslate(look, move);
    }

    public virtual void Destory()
    {
        if (m_pate)
        {
            GameObject.Destroy(m_pate);
        }
        GameObject.Destroy(m_go);
        m_local_pos = Vector3.zero;
        m_anim = null;
        m_insID = -1;
    }

   
}

 
