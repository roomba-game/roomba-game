using UnityEngine; // Required for Unity
using System.Collections; // Required for Arrays & other Collections
using System.Collections.Generic; // Required to use Lists or Dictionaries
using UnityEngine.SceneManagement; // Required for SceneManager
using UnityEngine.UI;
using TMPro;

public class Main : MonoBehaviour
{
    static public Main S;
    public float gameRestartDelay = 2f;
    public int numMess = 1;

    public GameObject Menu;
    public GameObject HUD;
    public GameObject endScreen;
    public GameObject loadScreen;
    public GameObject nextButton;
    public TextMeshProUGUI gameText;
    static private string[] MSGS;
    private AudioSource bgm;

    public MapCell[,] grid;

    public float cameraDistanceConfidant = 2.5f;

    // the current level
    public static uint level = 0;


    void Awake()
    {
        S = this;
        level = (uint)GameData.GD.getStartLevel();
        bgm = this.GetComponent<AudioSource>();
    }

    void Start()
    {
        StartLevel();
    }

    // Update is called once per frame
    public void Update()
    {
        // Keypress reactions
        if (Input.GetKeyDown(KeyCode.Escape)) { Menu.SetActive(!(Menu.activeSelf)); }
        else if (Input.GetKeyDown(KeyCode.R)) { restart(); }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (endScreen.activeSelf && nextButton.activeSelf) { nextLevel(); }
        }
    }

    // Loading a Level
    IEnumerator LoadLevel(float sec)
    {
        yield return new WaitForSeconds(sec);

        // Play correct stage music
        AudioClip stage_clip = SFX.S.music[GameData.GD.getStartStage()];
        if (bgm.clip != stage_clip)
        {
            bgm.clip = stage_clip;
            bgm.Play();
        }

        // Clear the grid
        if (grid != null)
        {
            Player.P.LockMovement();

            foreach (MapCell cell in grid)
            {
                if (cell.gameObject == null) continue;
                Destroy(cell.gameObject);
            }
        }
        grid = ImportLevel.Import(level);

        //Resets movecount for the level
        GameData.GD.resetMoveCount();

        loadScreen.SetActive(false);
    }

    // Start/Restart a Level
    public void StartLevel()
    {
        StartLevel(gameRestartDelay);
    }
    public void StartLevel(float sec)
    {
        StartCoroutine(LoadLevel(sec));
    }

    public void MessDestroyed()
    {
        numMess--;
        if (numMess > 0)
        {
            // Inform the player that they've cleaned a Mess
            SFX.S.Play(1);
            BriefText("Cleaned a Mess.");
        }
        else
        {
            Player.P.LockMovement();

            // Invoke win message
            SFX.S.Play(0);
            StartCoroutine(endLevel(true, 0));
        }
    }

    public IEnumerator endLevel(bool won, int sec)
    {
        yield return new WaitForSeconds(sec);

        if (won)
        {
            //Set level to completed
            GameData.GD.setLevelComplete(true, GameData.GD.getCurrentLevel());

            //Set players stars
            giveStars();
        }

        //Turn on the win screen
        endScreen.SetActive(true);
    }

    private void giveStars()
    {
        int curr_level = GameData.GD.getCurrentLevel();
        int curr_moves = GameData.GD.getLevelMoves(curr_level);

        if (curr_moves <= GameData.GD.getLowestMoves(curr_level) || GameData.GD.getLevelStars(curr_level) == 3)
        {
            GameData.GD.setStars(3);
        }
        else if (curr_moves <= GameData.GD.getSecondLowestMoves(curr_level) || GameData.GD.getLevelStars(curr_level) == 2)
        {
            GameData.GD.setStars(2);
        }
        else
        {
            GameData.GD.setStars(1);
        }
    }

    public void closeWinScreen()
    {
        // Turn off next level button
        nextButton.SetActive(false);
        // Turn off the win screen 
        endScreen.SetActive(false);
    }

    public void restart()
    {
        closeWinScreen();
        //restart the same level
        StartLevel(0f);
    }

    public void nextLevel()
    {
        closeWinScreen();
        level++;
        GameData.GD.setCurrentLevel((int)level);
        StartLevel(0.5f);
    }

    // Show and Hide WinText text
    IEnumerator HideText(string msg, float sec)
    {
        gameText.text = msg;
        yield return new WaitForSeconds(sec);
        gameText.text = "";
    }
    // Briefly show WinText text
    public void BriefText(string msg, float sec)
    {
        StartCoroutine(HideText(msg, sec));
    }
    public void BriefText(string msg)
    {
        BriefText(msg, gameRestartDelay / 2);
    }
    public void ShowText(string msg)
    {
        BriefText(msg, gameRestartDelay);
    }
}

