using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public GameObject main;
    public Slider sfxSlider;
    public Slider bgmSlider;
    public TextMeshProUGUI speedText;
    public GameObject unlockText;
    public TextMeshProUGUI unlock;
    public GameObject realButton;
    private AudioSource bg;

    private void Start()
    {
        sfxSlider.value = GameData.GD.getVolume("SFX"); ;
        bgmSlider.value = GameData.GD.getVolume("MUSIC"); ;
        bg = main.GetComponent<AudioSource>();
    }

    private void Update()
    {
        GameData.GD.setVolume(sfxSlider.value, bgmSlider.value);

        bg.volume = GameData.GD.getVolume("MUSIC");

        if (GameData.GD.getTotalStars() >= 60)
        {
            realButton.GetComponent<Buttons>().locked = false;
        }

        if (realButton.GetComponent<Buttons>().locked)
        {
            unlockText.SetActive(true);
            unlock.text = "Unlock at 60 total stars. You have " + GameData.GD.getTotalStars();
        }
        else
        {
            unlockText.SetActive(false);
        }

        if (GameData.GD.getPlayerSpeed() == 32)
        {
            speedText.text = "Roomba Speed: Fast";
        }
        else if (GameData.GD.getPlayerSpeed() == 16)
        {
            speedText.text = "Roomba Speed: Slow";
        }
        else if (GameData.GD.getPlayerSpeed() == 2)
        {
            speedText.text = "Roomba Speed: Realistic";
        }
    }

    public void setSpeed(float speed)
    {
        GameData.GD.setPlayerSpeed(speed);
    }

    public void Save()
    {
        GameData.GD.saveData();
    }

    public void Delete()
    {
        GameData.GD.deleteData();
    }
}
