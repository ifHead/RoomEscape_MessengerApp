using UnityEngine;

[CreateAssetMenu(fileName = "Quiz")]
public class Quiz : ScriptableObject
{
    [TextArea(10, 100)]
    public string textAreaString = "퀴즈를 어디까지 풀었는지 저장합니다";
    public bool isFirstQuizSolved;
    public bool isSecondQuizSolved;
}
