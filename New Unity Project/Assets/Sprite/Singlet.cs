using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singlet<T> where T: class,new()
{
    private static T ins;
    public T Ins
    {
        get
        {
            if (ins == null)
            {
                ins = new T();
            }
            return ins;
        }

    }
}
