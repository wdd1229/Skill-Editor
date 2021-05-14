using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSkillName : MonoBehaviour
{
    // Start is called before the first frame update
    public  Text skillName;

    void Start()
    {
        
    }
    bool isActive = false;
    public  void SetName(string name)
    {
        skillName.text = name;
        skillName.gameObject.SetActive(true);

        isActive = true;
    }

    // Update is called once per frame
    float time = 0;
    void Update()
    {
        if (isActive)
        {
            time += Time.deltaTime;
            if (time >= 2)
            {
                skillName.gameObject.SetActive(false);
                time = 0;
                isActive = false;
            }
        }
    }
}
