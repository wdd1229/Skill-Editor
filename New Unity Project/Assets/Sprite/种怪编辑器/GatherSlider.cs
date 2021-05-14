using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GatherSlider : MonoBehaviour
{
    //public static Slider sli;

    //public ETCButton btn;

    //public static Animator anim;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    btn.onPressed.AddListener(() =>
    //    {
    //        anim.SetTrigger("att");
    //    });
    //}

    public static Action action;
    // Update is called once per frame
    void Update()
    {
        if (action != null)
        {
            //Debug.Log("<<<<<<>>>>>>");
            //action();
        }
        action?.Invoke();
        //if (sli!=null)
        //{
        //    if (sli.value <1)
        //    {
        //        sli.value += Time.deltaTime/5;

        //    }
        //    else
        //    {
               
        //        sli.gameObject.SetActive(false);
        //        sli.value = 0;
        //        sli = null;

        //    }
        //}
    }
}
