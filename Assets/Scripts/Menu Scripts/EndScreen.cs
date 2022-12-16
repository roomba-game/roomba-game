using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    private bool next = true;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI levelName;
    public TextMeshProUGUI starCount;
    public TextMeshProUGUI moveCount;
    public GameObject nextButton;
    public GameObject loadScreen;
    public GameObject newStageText;

    private void Update()
    {
        //Check to see if the playert won
        if (GameData.GD.getLevelComplete(GameData.GD.getCurrentLevel()))
        {
            levelName.text = GameData.GD.getLevelName(GameData.GD.getCurrentLevel());
            starCount.text = "Stars: " + GameData.GD.getLevelStars(GameData.GD.getCurrentLevel()) + " / 3";
            moveCount.text = "Moves: " + GameData.GD.getLevelMoves(GameData.GD.getCurrentLevel()) + " / " + GameData.GD.getLowestMoves(GameData.GD.getCurrentLevel());

            //Check if this is the last level in the stage
            if (GameData.GD.getCurrentLevel() + 1 == GameData.GD.getLevelStartID(1))
            {
                winText.text = "You cleaned the entire living room!";
                newStageText.SetActive(true);
                next = false;
                GameData.GD.setStageUnlock(1);
            }
            else if (GameData.GD.getCurrentLevel() + 1 == GameData.GD.getLevelStartID(2))
            {
                winText.text = "You cleaned the entire kitchen!";
                newStageText.SetActive(true);
                next = false;
                GameData.GD.setStageUnlock(2);
            }
            else if (GameData.GD.getCurrentLevel() + 1 == 36)
            {
                winText.text = "You cleaned the entire lab!";
                newStageText.SetActive(false);
                next = false;
            }
            else
            {
                winText.text = "YOU WIN!";
                newStageText.SetActive(false);
                next = true;
            }
        }
        else
        {
            winText.text = "You knocked over an obstacle!";
            moveCount.text = "";
            starCount.text = "";
            next = false;
            newStageText.SetActive(false);
        }

        if (next)
        {
            nextButton.SetActive(true);
        }
        else
        {
            nextButton.SetActive(false);
        }
    }

    public void retryLevel()
    {
        //Restart level
        Main.S.restart();
    }

    public void returnToTitle()
    {
        //Return to the title screen
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void nextLevel()
    {
        if (next)
        {
            //Next level
            loadScreen.SetActive(true);

            Main.S.nextLevel();
        }
    }
}
