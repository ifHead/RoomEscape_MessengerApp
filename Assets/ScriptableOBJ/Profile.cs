using UnityEngine;

[CreateAssetMenu(fileName = "Profile")]
public class Profile : ScriptableObject
{
    [TextArea(10, 100)]
    public string textAreaString = "1. name, color, introduce의 개수는 같아야 합니다.\n2.Department 부서명은 구분선으로 쓰입니다. \n3.구분선이 나와야 하는 곳은 name을 공란으로 둡니다.";
    public string[] departments;
    public string[] names;
    public string[] colors;
    public string[] introduces;
    public string[] chatRooms;
}