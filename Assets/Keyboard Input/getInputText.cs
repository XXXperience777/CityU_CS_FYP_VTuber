using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRUiKits.Utils;
using UnityEngine.UI;

public class getInputText : MonoBehaviour
{   
    public UIKitInputField inputfield;
    public string output;

    //public Text inputText;
    //public UIKitInputField danField;
    private Text danText;
    //danText = GameObject.Find("CanvasKeyboard/newDan/Text").GetComponent<Text>();
    // Start is called before the first frame update
    // void Start()
    // {
    //     danText =  = GameObject.Find("CanvasDanmaku/Button").GetComponentInChildren<Text>();
    // }

    // Update is called once per frame
    public void setText()
    {
        output = inputfield.text;
        
        danText.text = output;
        //danText.text = inputText.text;
       // danField.text = output;
       // danText.text = output;
       Debug.Log(output);
    }
}
