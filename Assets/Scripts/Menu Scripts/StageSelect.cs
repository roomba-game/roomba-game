using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    public GameObject[] stageButtons;
    private static float gameVolume;

    // Start is called before the first frame update
    void Update()
    {
        //See if players have enough stars to unlock the next stages
        for (int i = 0; i < 3; i++)
        {
            if (GameData.GD.getStageUnlock(i))
            {
                stageButtons[i].GetComponent<Buttons>().setLocked(false);
            }
        }
    }

    public void setStage(int stage)
    {
        //Set the stage the player will start in
        GameData.GD.setStartStage(stage);
    }
}
