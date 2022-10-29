using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileSetter : MonoBehaviour
{
    public GameObject profileTemplate;
    public GameObject sectionTemplate;
    private Profile profileData;
    public GameObject[] profileInstances;
    int departmentLineCnt = 0;

    void Start()
    {
        profileData = Resources.Load<Profile>("Data/ProfileData");
        profileInstances = new GameObject[profileData.name.Length];

        for(int i = 0; i < profileData.name.Length; i++)
        {
            setProfileNumberOf(i);
            setSectionLineNumberOf(i);
        }
    }

    void setProfileNumberOf(int n){
        if(profileData.name[n] != "-")
        {
            profileInstances[n] = Instantiate(profileTemplate, transform);
            profileInstances[n].transform.Find("Name").GetComponent<TextMeshProUGUI>().text = profileData.name[i];
            profileInstances[n].transform.Find("Introduce").GetComponent<TextMeshProUGUI>().text = profileData.introduce[i];
            Material material = Instantiate(transform.Find("Profile").GetComponent<Image>().material);
            mat.SetFloat//머티리얼에서 hsv 쉐이더 접근해서 바꾸는거 시도중. 쉐이더 다 변하지 않아야 함.
            //https://answers.unity.com/questions/920091/how-can-i-change-the-shader-parameters-for-an-ui-i.html
            profileInstances[n].transform.Find("Profile").GetComponent<Image>().material = ;
        }
    }

    void setSectionLineNumberOf(int n){
        if(profileData.name[n] == "-")
        {
            profileInstances[n] = Instantiate(sectionTemplate, transform);
            profileInstances[n].transform.Find("DepartmentName").GetComponent<TextMeshProUGUI>().text = profileData.department[departmentLineCnt];
            departmentLineCnt++;
        }
    }
}