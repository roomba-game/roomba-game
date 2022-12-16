using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    public GameObject thisButton;
    private Button thisB;
    private Image thisImage;
    public TextMeshProUGUI thisText;
    public bool locked = false;
    public TMP_ColorGradient unlockedColor;
    public TMP_ColorGradient lockedColor;

    private void Start()
    {
        thisB = thisButton.GetComponent<Button>();
        thisImage = thisButton.GetComponent<Image>();
    }

    private void Update()
    {
        if (locked)
        {
            thisText.colorGradientPreset = lockedColor;
            thisB.enabled = false;
            thisImage.color = new Color(0, 0, 0, 0);
        }
        else
        {
            thisText.colorGradientPreset = unlockedColor;
            thisB.enabled = true;
            thisImage.color = new Color(0, 0, 0, 255);
        }
    }

    public void setLocked(bool Lock)
    {
        locked = Lock;
    }

}
