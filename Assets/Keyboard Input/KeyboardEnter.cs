using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRUiKits.Utils;

public class KeyboardEnter : MonoBehaviour
{
    public UIKitInputField inputfield1;
    // You can have your message box here declared, assume your message box is Text
    public Text message;


    public void Enter() {
        message.text = inputfield1.text;
    }
}
