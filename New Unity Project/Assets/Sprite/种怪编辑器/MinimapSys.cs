﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MinimapSys : UIbase
{
    public MapControl m_map;
    public override void DoCreate(string path)
    {
        base.DoCreate(path);
        Transform map = m_go.transform.Find("/map");
        m_map = map.gameObject.AddComponent<MapControl>();

    }

    public override void DoShow(bool active)
    {
        base.DoShow(active);
    }

    public override void Destroy()
    {
        GameObject.Destroy(m_map);
        base.Destroy();
    }
}
