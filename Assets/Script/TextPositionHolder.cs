using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextPositionHolder : MonoBehaviour
{
    public InputField inputField;
    bool toggle = true;

    void contentsizefitterToggle()
    {   
        toggle = !toggle;
        transform.GetComponent<ContentSizeFitter>().enabled = toggle;
    }

    public void fitSize()
    {
        if (inputField.multiLine)
        {
            contentsizefitterToggle();
            Invoke("contentsizefitterToggle", 0.001f);
        }
    }
}
