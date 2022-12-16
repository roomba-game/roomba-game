using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI moveCount;

    private void Update()
    {
        moveCount.text = "Moves: " + GameData.GD.getLevelMoves(GameData.GD.getCurrentLevel());
    }
}
