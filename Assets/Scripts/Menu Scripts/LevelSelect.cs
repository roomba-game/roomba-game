using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelSelect : MonoBehaviour
{
    public GameObject[] levelButtons;
    private int level;

    // Start is called before the first frame update 
    void Update()
    {
        //Determine the starting level for each stage 
        if (GameData.GD.getStartStage() == 0)
        {
            level = 0;
        }
        else if (GameData.GD.getStartStage() == 1)
        {
            level = GameData.GD.getLevelStartID(1);
        }
        else if (GameData.GD.getStartStage() == 2)
        {
            level = GameData.GD.getLevelStartID(2);
        }

        levelButtons[0].GetComponent<Buttons>().setLocked(false);

        //Determine which levels the player has unlocked 
        for (int i = 0; i < GameData.GD.getLevelAmt(GameData.GD.getStartStage() + 1); i++)
        {
            if (i < 11)
            {
                if (GameData.GD.getLevelComplete(level))
                {
                    levelButtons[i + 1].GetComponent<Buttons>().setLocked(false);
                }
                else
                {
                    levelButtons[i + 1].GetComponent<Buttons>().setLocked(true);
                }
                level++;
            }
        }
    }

    public void setLevel(int level)
    {
        //Set starting level 
        GameData.GD.setStartLevel(level);
        playGame();
    }

    public void playGame()
    {
        //Go to the game scene 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    } 
}