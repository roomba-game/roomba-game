using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StarCount : MonoBehaviour
{
    public Image starImage;
    public int level;
    public Sprite star_0;
    public Sprite star_1;
    public Sprite star_2;
    public Sprite star_3;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // set right level for stage
        int stage = GameData.GD.getStartStage();
        if (stage != level / 12) {
            level %= 12;
            level += GameData.GD.getLevelStartID(stage);
        }

        int stars = GameData.GD.getLevelStars( level );

        if (stars == 1)
        {
            starImage.sprite = star_1;
        }
        else if (stars == 2)
        {
            starImage.sprite = star_2;
        }
        else if (stars == 3)
        {
            starImage.sprite = star_3;
        }
        else
        {
            starImage.sprite = star_0;
        }
    }
}
