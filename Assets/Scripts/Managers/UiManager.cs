using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    public TextMeshProUGUI levelIdText;
    public TextMeshProUGUI levelLenghtText;
    public TextMeshProUGUI rightToLifeText;
    public void Awake()
    {
        Instance = this;
    }

    public void SetLevelIdText(int level)
    {
        levelIdText.text = "Now Level :"+level;
    }
    public void SetLevelLenghtText(int lenght)
    {
        levelLenghtText.text = "Lenght :" + lenght;
    }
    public void RightToLifeText(int lifeCount)
    {
        rightToLifeText.text = "Right To Life :" + lifeCount;
    }


}
