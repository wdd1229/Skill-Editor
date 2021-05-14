using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 技能父类
/// </summary>
public class SkillBase  
{
    public string name = string.Empty;
    public virtual void Play() { } 
    public virtual void Init() { }
    public virtual void Stop() { }

}
