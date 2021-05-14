using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class SkillXml
{
    public string name;
    public Dictionary<string, List<string>> skills = new Dictionary<string, List<string>>();
}
public class GameData : Singleton<GameData>
{
    public Dictionary<string, List<SkillXml>> AllRoleSkillList = new Dictionary<string, List<SkillXml>>();

    
    public void InitByRoleName(string roleName)
    {
        if (File.Exists("Assets/" + roleName + ".txt"))
        {
            string str = File.ReadAllText("Assets/" + roleName + ".txt");

            List<SkillXml> skills = JsonConvert.DeserializeObject<List<SkillXml>>(str);

            AllRoleSkillList.Add(roleName, skills);
        }
    }

    public List<SkillXml> GetSkillByRoleName(string roleName)
    {
        if (AllRoleSkillList.ContainsKey(roleName)){
            return AllRoleSkillList[roleName];
        }

        return null;
    }

    public Dictionary<int, TaskData> AllTaskDic = new Dictionary<int, TaskData>();
    public void InitTaskData()
    {
        TaskData task = new TaskData();
        task.taskId = 1;
        task.taskName = "采集任务";

        AllTaskDic.Add(1, task);
    }

    public TaskData GetTaskDataById(int taskId)
    {
        if (AllTaskDic.ContainsKey(taskId))
        {
            return AllTaskDic[taskId];
        }
        return null;
    }
}

public class TaskData
{
    public int taskId;
    public string taskName;
}