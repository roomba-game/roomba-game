using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public TextMeshProUGUI levelName;
    public TextMeshProUGUI starCount;
    public TextMeshProUGUI moveCount;

    private void Update()
    {
        Player.P.LockMovement();

        levelName.text = GameData.GD.getLevelName(GameData.GD.getCurrentLevel());
        starCount.text = "Stars: " + GameData.GD.getLevelStars(GameData.GD.getCurrentLevel());
        moveCount.text = "Moves: " + GameData.GD.getLevelMoves(GameData.GD.getCurrentLevel());
    }

    public void returnToTitle()
    {
        //Return to title screen
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void retryLevel()
    {
        //Restart level
        Main.S.restart();
    }
}
