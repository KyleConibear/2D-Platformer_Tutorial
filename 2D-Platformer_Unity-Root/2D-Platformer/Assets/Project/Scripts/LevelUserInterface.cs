using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUserInterface : MonoBehaviour
{
    [SerializeField] private TMP_Text gemCountText = null;
    [SerializeField] private TMP_Text cherryCountText = null;

    public void UpdateGemCounter(int amount)
    {
        this.gemCountText.text = amount.ToString();
    }

    public void UpdateCherryCounter(int amount)
    {
        this.cherryCountText.text = amount.ToString();
    }
}
