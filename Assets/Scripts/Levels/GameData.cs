using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    //Used Elsewhere
    public static GameData GD;
    private static float playerSpeed;
    private static bool[] unlockables;
    private static int currentLevel = 0;
    private static float sfxVolume;
    private static float bgmVolume;
    private static int[] numberOfLevels = { 0, 12, 12, 12 }; //0 = total, 1 = livingRoom, 2 = kitchen, 3 = lab
    private static int[] stageStart = { 0, 0, 0 }; //0 = livingRoom, 1 = kitchen, 2 = lab
    private static bool[] stageUnlock = { true, false, false };
    private static string[] levelName;
    private static bool[] levelCompleted;
    private static int totalStars;
    private static int[] collectedStars;
    private static int totalMoves;
    private static int[] moveCount;
    private static int[,] moves;

    //Used here only
    private static bool gameInitalized = false;
    private static int[] startGame = { 0, 0 }; //0 = stage, 1 = level

    private void Start()
    {
        GD = this;

        if (!gameInitalized)
        {
            setLevelData();
        }
    }

    private void setLevelData()
    {
        //Use number of levels to determine the starting level for each stage
        //stageStart[0] = 0;
        stageStart[1] = numberOfLevels[1];
        stageStart[2] = stageStart[1] + numberOfLevels[2];
        numberOfLevels[0] = stageStart[2] + numberOfLevels[3];

        levelName = new string[numberOfLevels[0]];
        levelCompleted = new bool[numberOfLevels[0]];
        collectedStars = new int[numberOfLevels[0]];
        moveCount = new int[numberOfLevels[0]];
        moves = new int[numberOfLevels[0], 2];
        unlockables = new bool[1];

        loadData();

        //Loop through each level and set default values for each
        for (int i = 0; i < numberOfLevels[0]; i++)
        {
            if (i < stageStart[1])
            {
                levelName[i] = "Living Room - " + (i + 1);
            }
            else if (i < stageStart[2])
            {
                levelName[i] = "Kitchen - " + (i + 1 - stageStart[1]);
            }
            else
            {
                levelName[i] = "Lab - " + (i + 1 - stageStart[2]);
            }
        }

        setLevelMoves();

        gameInitalized = true;
    }

    private void setLevelMoves()
    {
        //Any move count that is a 0 has not been set correctly yet

        //Living Room lowest move counts
        moves[0, 0] = 10;
        moves[1, 0] = 6;
        moves[2, 0] = 12;
        moves[3, 0] = 11;
        moves[4, 0] = 8;
        moves[5, 0] = 13;
        moves[6, 0] = 10;
        moves[7, 0] = 11;
        moves[8, 0] = 8;
        moves[9, 0] = 10;
        moves[10, 0] = 8;
        moves[11, 0] = 5;

        //Kitchen lowest move counts
        moves[12, 0] = 16;
        moves[13, 0] = 7;
        moves[14, 0] = 9;
        moves[15, 0] = 14;
        moves[16, 0] = 10;
        moves[17, 0] = 14;
        moves[18, 0] = 19;
        moves[19, 0] = 32;
        moves[20, 0] = 24;
        moves[21, 0] = 31;
        moves[22, 0] = 17;
        moves[23, 0] = 24;

        //Lab lowest move counts
        moves[24, 0] = 5;
        moves[25, 0] = 5;
        moves[26, 0] = 10;
        moves[27, 0] = 8;
        moves[28, 0] = 6;
        moves[29, 0] = 8;
        moves[30, 0] = 7;
        moves[31, 0] = 38;
        moves[32, 0] = 26;
        moves[33, 0] = 13;
        moves[34, 0] = 13;
        moves[35, 0] = 49;

        // Second Lowest Moves
        for (int i = 0; i < numberOfLevels[0]; i++)
        {
            moves[i, 1] = moves[i, 0] + 3;
        }
    }
    //=======================Persistance==========================
    private void loadData()
    {
        playerSpeed = PlayerPrefs.GetFloat("Player Speed", 16);
        bgmVolume = PlayerPrefs.GetFloat("Music Volume", 1);
        sfxVolume = PlayerPrefs.GetFloat("SFX Volume", 1);
        totalMoves = PlayerPrefs.GetInt("Total Moves", 0);
        totalStars = PlayerPrefs.GetInt("Total Stars", 0);

        PlayerPrefs.GetInt("Stage " + 0, 1);

        if (PlayerPrefs.GetInt("Unlockables " + 0, 0) == 1)
        {
            unlockables[0] = true;
        }
        else
        {
            unlockables[0] = false;
        }

        for (int i = 1; i < 3; i++)
        {
            if (PlayerPrefs.GetInt("Stage " + i, 0) == 1)
            {
                stageUnlock[i] = true;
            }
            else
            {
                stageUnlock[i] = false;
            }
        }

        for (int i = 0; i < numberOfLevels[0]; i++)
        {
            if (PlayerPrefs.GetInt("Level " + i + " Completed", 0) == 1)
            {
                levelCompleted[i] = true;
            }
            else
            {
                levelCompleted[i] = false;
            }

            collectedStars[i] = PlayerPrefs.GetInt("Level " + i + " Stars", 0);
            moveCount[i] = PlayerPrefs.GetInt("Level " + i + " Moves", 0);
        }
    }

    //Save player data
    public void saveData()
    {
        PlayerPrefs.SetFloat("Player Speed", playerSpeed);
        PlayerPrefs.SetFloat("Music Volume", bgmVolume);
        PlayerPrefs.SetFloat("SFX Volume", sfxVolume);
        PlayerPrefs.SetInt("Total Moves", totalMoves);
        PlayerPrefs.SetInt("Total Stars", totalStars);

        if (unlockables[0])
        {
            PlayerPrefs.SetInt("Unlockables " + 0, 1);
        }
        else
        {
            PlayerPrefs.SetInt("Unlockables " + 0, 0);
        }

        for (int i = 0; i < 3; i++)
        {
            if (stageUnlock[i])
            {
                PlayerPrefs.SetInt("Stage " + i, 1);
            }
            else
            {
                PlayerPrefs.SetInt("Stage " + i, 0);
            }
        }

        for (int i = 0; i < numberOfLevels[0]; i++)
        {
            if (levelCompleted[i])
            {
                PlayerPrefs.SetInt("Level " + i + " Completed", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Level " + i + " Completed", 0);
            }

            PlayerPrefs.SetInt("Level " + i + " Stars", collectedStars[i]);
            PlayerPrefs.SetInt("Level " + i + " Moves", moveCount[i]);
        }
    }

    //Delete data and reset inital values
    public void deleteData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("Player Speed", 16);
        PlayerPrefs.SetFloat("Music Volume", 1);
        PlayerPrefs.SetFloat("SFX Volume", 1);
        PlayerPrefs.SetInt("Total Moves", 0);
        PlayerPrefs.SetInt("Total Stars", 0);

        PlayerPrefs.SetInt("Unlockables " + 0, 0);

        for (int i = 0; i < 3; i++)
        {
            if (i == 0)
            {
                PlayerPrefs.SetInt("Stage " + i, 1);
            }
            else
            {
                PlayerPrefs.SetInt("Stage " + i, 0);
            }
        }

        for (int i = 0; i < numberOfLevels[0]; i++)
        {
            PlayerPrefs.SetInt("Level " + i + " Completed", 0);

            PlayerPrefs.SetInt("Level " + i + " Stars", 0);
            PlayerPrefs.SetInt("Level " + i + " Moves", 0);
        }
    }

    //===================Getters and Setters======================

    public void setCurrentLevel(int curLev)
    {
        currentLevel = curLev;
    }
    public int getCurrentLevel()
    {
        return currentLevel;
    }

    public void setPlayerSpeed(float speed)
    {
        playerSpeed = speed;
    }
    public float getPlayerSpeed()
    {
        return playerSpeed;
    }

    public void setVolume(float sfx, float bgm)
    {
        sfxVolume = sfx;
        bgmVolume = bgm;
    }
    public float getVolume(string type)
    {
        return (type == "SFX") ? sfxVolume : bgmVolume;
    }

    public void setUnlockables(bool unlock, int unlockable)
    {
        unlockables[unlockable] = unlock;
    }
    public bool getUnlockables(int unlockable)
    {
        return unlockables[unlockable];
    }

    public int getLevelAmt(int stage)
    {
        return numberOfLevels[stage];
    }

    public int getLevelStartID(int stage)
    {
        return stageStart[stage];
    }

    public string getLevelName(int level)
    {
        return levelName[level];
    }

    public void setStageUnlock(int stage)
    {
        stageUnlock[stage] = true;
    }
    public bool getStageUnlock(int stage)
    {
        return stageUnlock[stage];
    }

    public void setLevelComplete(bool complete, int level)
    {
        levelCompleted[level] = complete;
    }
    public bool getLevelComplete(int level)
    {
        return levelCompleted[level];
    }

    public void setStars(int stars)
    {
        int diff = stars - collectedStars[currentLevel];
        collectedStars[currentLevel] = stars;
        totalStars += diff;
    }

    public void subtractStars(int amt)
    {
        totalStars -= amt;
    }

    public int getLevelStars(int level)
    {
        return collectedStars[level];
    }

    public int getTotalStars()
    {
        return totalStars;
    }

    public void setMoves()
    {
        moveCount[currentLevel]++;
        totalMoves++;
    }

    public void resetMoveCount()
    {
        totalMoves -= moveCount[currentLevel];
        moveCount[currentLevel] = 0;
    }

    public int getLevelMoves(int level)
    {
        return moveCount[level];
    }
    public int getTotalMoves()
    {
        return moveCount[0];
    }

    public int getLowestMoves(int level)
    {
        return moves[level, 0];
    }

    public int getSecondLowestMoves(int level)
    {
        return moves[level, 1];
    }

    public void setStartStage(int stage)
    {
        startGame[0] = stage;
    }
    public int getStartStage()
    {
        return startGame[0];
    }

    public void setStartLevel(int level)
    {
        if (startGame[0] == 0)
        {
            startGame[1] = level;
        }
        else if (startGame[0] == 1)
        {
            startGame[1] = level + stageStart[1];
        }
        else if (startGame[0] == 2)
        {
            startGame[1] = level + stageStart[2];
        }

        currentLevel = startGame[1];
    }
    public int getStartLevel()
    {
        return startGame[1];
    }
}
