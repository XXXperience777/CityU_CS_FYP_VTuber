using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MicrophoneChecker : MonoBehaviour
{
    private IEnumerator Start()
    {
        // 檢查是否有麥克風權限，否則提出權限請求
        if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        }

        // 再次檢查，如仍然無權限則將遊戲關閉
        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            Debug.Log("Device: " + Microphone.devices.Select(x => x.ToString()));
        }
        else
        {
            Application.Quit();
        }
    }
}