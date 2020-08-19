using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUserInterface : MonoBehaviour
{
    [SerializeField] private TMP_Text cherryCountText = null;

    public void UpdateCherryCounter(int amount)
    {
        this.cherryCountText.text = amount.ToString();
    }
}
