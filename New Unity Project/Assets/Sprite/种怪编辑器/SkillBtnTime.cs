using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBtnTime : MonoBehaviour
{
    // Start is called before the first frame update
    Button btn;
    Image ima;
    Text txt;
    void Start()
    {
        btn = transform.GetComponent<Button>();
        ima = transform.GetComponent<Image>();
        txt = transform.Find("Text").GetComponent<Text>();
        txt.text = "1";
        //if (btn.IsActive())
        //{
        //    Debug.Log("激活");
        //}
        btn.onClick.AddListener(() =>
        {
            btn.interactable = false;
        });
    }

    // Update is called once per frame
    float time = 1;
    void Update()
    {
        if (btn.interactable == false)
        {
            time -= Time.deltaTime;
            ima.fillAmount = time / 1;
            txt.text = (time / 1).ToString("0.00");
            if (ima.fillAmount <= 0)
            {
                txt.text = "1";
                btn.interactable = true;
                time = 1;
                ima.fillAmount = 1;
            }
            

        }
    }
}
