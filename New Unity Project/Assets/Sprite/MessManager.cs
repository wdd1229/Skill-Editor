using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noati
{
    public string name;
    public object[] data;

    public void Refresh(string na,params object[] da)
    {
        name = na;
        data = da;
    }
}

public class MessManager : Singlet<MessManager>
{
    public Dictionary<string, Action<Noati>> MesDic = new Dictionary<string, Action<Noati>>();

    public void AddListener(string msg,Action<Noati> action)
    {
        if (MesDic.ContainsKey(msg))
        {
            MesDic[msg] += action;
        }
        else
        {
            MesDic.Add(msg, action);
        }
    }

    public void Send(string name,Noati noati)
    {
        if (MesDic.ContainsKey(name))
        {
            MesDic[name](noati);
        }
    }
}
