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
        profileInstances = new GameObject[profileData.names.Length];

        for(int i = 0; i < profileData.names.Length; i++)
        {
            setProfileNumberOf(i);
            setSectionLineNumberOf(i);
        }
    }

    void setProfileNumberOf(int n){
        if(profileData.names[n] != "-")
        {
            setProfileTextsOf(n);
            setProfileColorOf(n);
        }
    }

    void setProfileTextsOf(int n)
    {
        profileInstances[n] = Instantiate(profileTemplate, transform);
        profileInstances[n].transform.Find("Name").GetComponent<TextMeshProUGUI>().text = profileData.names[n];
        profileInstances[n].transform.Find("Introduce").GetComponent<TextMeshProUGUI>().text = profileData.introduces[n];
    }

    void setProfileColorOf(int n)
    {
        Image profileImg = profileInstances[n].transform.Find("Profile").GetComponent<Image>();
        Material mat = Instantiate(profileImg.material);
        mat.SetVector("_HSVAAdjust", new Vector4(float.Parse(profileData.colors[n]), 0f, 0f, 0f));
        profileInstances[n].transform.Find("Profile").GetComponent<Image>().material = mat;
    }

    void setSectionLineNumberOf(int n){
        if(profileData.names[n] == "-")
        {
            profileInstances[n] = Instantiate(sectionTemplate, transform);
            profileInstances[n].transform.Find("DepartmentName").GetComponent<TextMeshProUGUI>().text = profileData.departments[departmentLineCnt];
            departmentLineCnt++;
        }
    }
}