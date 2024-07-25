using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIController : MonoBehaviour
{
    static GameUIController instance;
    public static GameUIController Instance => instance;
    public TextMeshProUGUI coinsAmt;
    public void Awake()
    {
        if(instance != null && instance != this) DestroyImmediate(this);
        instance = this;
    }

    public void CaptureCoins(int coinsCount)
    {
        coinsAmt.text = "Coins: " + coinsCount;
    }
}
