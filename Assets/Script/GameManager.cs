using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text DebugText;

    public void ReceiveMessage(string msg) => DebugText.text = msg;

}
