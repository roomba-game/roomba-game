using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject main;
    public GameObject OptionsMenu;
    private AudioSource bg;

    private void Start()
    {
        bg = main.GetComponent<AudioSource>();

        bg.volume = GameData.GD.getVolume("MUSIC");
    }

    public void QuitGame()
    {
        GameData.GD.saveData();

        //Quits the program. This does not work in the editor
        Application.Quit();
    }
}
