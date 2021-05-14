using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
   public Animator anim;
    AudioSource audioSource;
   public Transform effectparnet;

    RuntimeAnimatorController controller;
    
  public  AnimatorOverrideController overrideController;
    public static Player CreatRole(string foldName,string name)
    {
        Debug.Log("Assets/" + foldName + "/" + name + ".prefab");
        GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Model/" + foldName + "/" + name + ".prefab");
       GameObject it= GameObject.Instantiate(obj);
        Player player = it.GetComponent<Player>();

        player.anim = player.GetComponent<Animator>();
        player.audioSource = player.gameObject.AddComponent<AudioSource>();
        player.effectparnet = player.transform.Find("effectparnet");
        player.gameObject.name = name;

        player.controller = Resources.Load<RuntimeAnimatorController>("Player");

        player.overrideController = new AnimatorOverrideController();
        player.overrideController.runtimeAnimatorController = player.controller;

        player.anim.runtimeAnimatorController = player.overrideController;

        return player;
    }

    /// <summary>
    /// 外部调用
    /// </summary>
    public void InItData()
    {
        overrideController = new AnimatorOverrideController();
        controller = Resources.Load<RuntimeAnimatorController>("Role");
        overrideController.runtimeAnimatorController = controller;
        Debug.Log(anim);
        anim.runtimeAnimatorController = overrideController;

        audioSource = gameObject.AddComponent<AudioSource>();
        effectparnet = transform.Find("effectparnet");
    }

    public void play()
    {
        //根据条件判断播放不同的组件  遍历   skillsList  item.player


        //使用前先判空
        if (_Anim != null)
        {
            //先打断之前的 起手动作
            _Anim.Play();

        }

        //声音
        if (_Aduio != null)
        {
            _Aduio.Play();

        }

        //特效
        if (_Effect != null)
        {
            _Effect.Play();
        }


    }


    private Skill_Anim _Anim;
    private Skill_Audio _Aduio;
    private Skill_Effects _Effect;
    public void SetData(string skillName)
    {
        List<SkillXml> skillList = GameData.Instance.GetSkillByRoleName("Teddy");
        Debug.Log(skillList.Count);
        foreach (var item in skillList)
        {
            if(item.name== skillName)
            {
                foreach (var ite in item.skills)
                {
                    foreach (var it in ite.Value)
                    {
                        if (ite.Key.Equals("动画"))
                        {
                            AnimationClip clip= AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/GameDate/Anim/" + it + ".anim");
                            if (_Anim == null) _Anim = new Skill_Anim(this);
                            _Anim.SetAnimClip(clip);
                        }else if (ite.Key.Equals("音效"))
                        {
                            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/GameDate/Audio/" + it + ".mp3");
                            if (_Aduio == null) _Aduio = new Skill_Audio(this);
                            _Aduio.SetAnimClip(clip);
                        }
                        else if (ite.Key.Equals("特效"))
                        {
                            GameObject clip = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameDate/Effect/Skill/" + it + ".prefab");
                            if (_Effect == null) _Effect = new Skill_Effects(this);
                            _Effect.SetGameClip(clip);
                            //skillsList[item.name].Add(_Anim);
                        }
                    }
                }
            }
        }
    }

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
