using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 异步加载场景 ， 添加回掉方法
/// </summary>
public class GameSceneUntil : MonoBehaviour
{
   public static void LoadSceneAsync(string sceneName,Action call)
    {
      AsyncOperation ar=  SceneManager.LoadSceneAsync(sceneName);
        ar.completed += (_ar) =>
        {
            call?.Invoke();
        };
    }
}
