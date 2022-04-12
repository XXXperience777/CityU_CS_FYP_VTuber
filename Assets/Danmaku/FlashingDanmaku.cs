using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashingDanmaku : MonoBehaviour
{
	private Color HintColor;
	Image danImg;
    Text danText;

    // Start is called before the first frame update
    void Start()
    {
        danImg = GetComponent<Image>();
        danText = GetComponent<Text>();
		HintColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        HintColor.a = Mathf.PingPong(Time.time, 1F);
		danImg.color = HintColor;
        danText.color = HintColor;
    }
	
}
