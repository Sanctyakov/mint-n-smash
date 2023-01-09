using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject mainMenu, HUD, debriefing, youWin, youLose;
    public HammerCursor hammerCursor;
    public GameObject[] smashables;
    public ParticleSystem[] sparks;
    public Animator clickBlockerAnimator;
    public Transform spawnPoint, worldSpace, popUpCanvas;
    public TMP_Text userName, timer, score, greenPopUp, redPopUp, orangePopUp;
    public int targetPoints = 200, pointsEarned;

    private Smashable fixedSmashable;
    private int fixedSpawnsCounter, maxFixedSpawns;
    private bool fixedSpawns;
    private float time = 120.0f, minSpawnRate, maxSpawnRate, timeCounter, spawnTimeCounter = 0.0f, rewardMultiplier = 1.0f, timeMultiplier = 1.0f;
    
    //set custom range for random position
    public float spawnMinX;
    public float spawnMaxX;

    private enum GameStates
    {
        MainMenu,
        Game,
        Win,
        Lose
    }

    private GameStates gameState = GameStates.MainMenu;

    void Start()
    {
        ChangeState (GameStates.MainMenu);
    }

    void ChangeState (GameStates gs)
    {
        gameState = gs;

        switch (gameState)
        {
            case GameStates.MainMenu:
                mainMenu.SetActive(true);
                HUD.SetActive(false);
                debriefing.SetActive(false);
                hammerCursor.gameObject.SetActive(false);
                Cursor.visible = true;
                break;
            case GameStates.Game:
                mainMenu.SetActive(false);
                HUD.SetActive(true);
                debriefing.SetActive(false);
                hammerCursor.gameObject.SetActive(true);
                Cursor.visible = false;
                break;
            case GameStates.Win:
                mainMenu.SetActive(false);
                HUD.SetActive(true);
                debriefing.SetActive(true);
                youWin.SetActive(true);
                youLose.SetActive(false);
                hammerCursor.gameObject.SetActive(false);
                Cursor.visible = true;
                break;
            case GameStates.Lose:
                mainMenu.SetActive(false);
                HUD.SetActive(true);
                debriefing.SetActive(true);
                youWin.SetActive(false);
                youLose.SetActive(true);
                hammerCursor.gameObject.SetActive(false);
                Cursor.visible = true;
                break;
        }
    }

    public void SetUserName(string enteredString)
    {
        userName.text = enteredString;
    }

    public void GameStart(float min)
    {
        minSpawnRate = min;

        maxSpawnRate = min * 2;

        ChangeState(GameStates.Game);
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }

    void Update()
    {
        if (gameState != GameStates.Game)
        {
            return;
        }

        ProcessTimer();

        if (timeCounter >= spawnTimeCounter)
        {
            SpawnSmashableAtRandomPosition();

            spawnTimeCounter += Random.Range(minSpawnRate, maxSpawnRate);
        }
    }

    private void ProcessTimer()
    {
        if (time - timeCounter <= 0.0f)
        {
            ChangeState(GameStates.Lose);
            return;
        }

        timeCounter += Time.deltaTime * timeMultiplier;

        System.DateTime dt = new System.DateTime();
        string displayTime;
        string suffix = " seconds";

        int remainingSeconds = Mathf.CeilToInt(time - timeCounter);
        dt = dt.AddSeconds(remainingSeconds);

        if (remainingSeconds <= 10)
        {
            //timeAlert.Begin();
        }

        if (time >= 60)
        {
            displayTime = dt.Minute.ToString("0") + ":" + dt.Second.ToString("00");

            if (dt.Minute > 0)
            {
                suffix = " minutes";
            }
        }
        else
        {
            displayTime = dt.Second.ToString("00");
        }

        timer.text = displayTime + suffix;
    }

    void SpawnSmashableAtRandomPosition()
    {
        float x = Random.Range(spawnMinX, spawnMaxX);

        Vector3 v3 = new Vector3(x, spawnPoint.position.y, spawnPoint.position.z);

        if (fixedSpawns)
        {
            Instantiate(fixedSmashable, v3, spawnPoint.rotation);

            fixedSpawnsCounter++;

            if (fixedSpawnsCounter >= maxFixedSpawns)
            {
                fixedSpawns = false;
                fixedSpawnsCounter = 0;
            }
        }
        else
        {
            SpawnRandomSmashable(v3);
        }
    }

    void SpawnRandomSmashable(Vector3 v3)
    {
        Instantiate(smashables[Random.Range(0, smashables.Length)], v3, spawnPoint.rotation, worldSpace);
    }

    void SpawnRandomSparks(Transform smashableTransform)
    {
        Instantiate(sparks[Random.Range(0, sparks.Length)], smashableTransform.position, smashableTransform.rotation, worldSpace);
    }

    void SpawnGreenPopUp(string greenPopUpText, Transform smashableTransform)
    {
        Camera cam = Camera.main;

        Vector3 pos = cam.WorldToScreenPoint(smashableTransform.position);
        TMP_Text g = Instantiate(greenPopUp, pos, Quaternion.identity, popUpCanvas);
        g.text = greenPopUpText;
    }

    void SpawnRedPopUp(string redPopUpText, Transform smashableTransform)
    {
        Camera cam = Camera.main;

        Vector3 pos = cam.WorldToScreenPoint(smashableTransform.position);
        TMP_Text r = Instantiate(redPopUp, pos, Quaternion.identity, popUpCanvas);
        r.text = redPopUpText;
    }

    void SpawnOrangePopUp(string orangePopUpText, Transform smashableTransform)
    {
        Camera cam = Camera.main;

        Vector3 pos = cam.WorldToScreenPoint(smashableTransform.position);
        TMP_Text o = Instantiate(orangePopUp, pos, Quaternion.identity, popUpCanvas);
        o.text = orangePopUpText;
    }

    public void AwardPoints(int pointsAwarded, Transform smashableTransform)
    {
        int pointsToAdd = (int) (pointsAwarded * rewardMultiplier);

        pointsEarned += pointsToAdd;

        score.text = pointsEarned.ToString();

        SpawnGreenPopUp("+" + pointsToAdd, smashableTransform);

        SpawnRandomSparks(smashableTransform);

        if (pointsEarned >= targetPoints)
        {
            ChangeState(GameStates.Win);
        }
    }

    public void LosePoints(int pointsLost, Transform smashableTransform)
    {
        if (gameState != GameStates.Game) return; // Don't keep subtracting points after you've won.

        pointsEarned -= pointsLost;

        if (pointsEarned < 0) pointsEarned = 0;

        score.text = pointsEarned.ToString();

        SpawnRedPopUp("-" + pointsLost, smashableTransform);

        SpawnRandomSparks(smashableTransform);
    }

    public void AwardTime(float timeAwarded, float extraTime, float extraTimeRate, Transform smashableTransform)
    {
        timeAwarded += pointsEarned * extraTime / extraTimeRate;

        time += timeAwarded;

        SpawnGreenPopUp("+" + timeAwarded + "s", smashableTransform);

        SpawnRandomSparks(smashableTransform);
    }

    public void ScrambleSmashables(Transform smashableTransform)
    {
        GameObject[] smashablesOnScreen;

        smashablesOnScreen = GameObject.FindGameObjectsWithTag("Smashable");

        foreach (GameObject s in smashablesOnScreen)
        {
            SpawnRandomSmashable(s.transform.position);

            Destroy(s);
        }

        SpawnOrangePopUp("SCRAMBLE!", smashableTransform);

        SpawnRandomSparks(smashableTransform);
    }

    public void MultiplyMouseSpeed(float speedMultiplier, float speedBoostDuration, Transform smashableTransform)
    {
        SpawnGreenPopUp("Hammer speed x" + speedMultiplier+ "!", smashableTransform);

        SpawnRandomSparks(smashableTransform);

        hammerCursor.ChangeSpeed(speedMultiplier, speedBoostDuration);
    }

    public void AccelerateGame(float timeMultiplier, float rewardMultiplier, Transform smashableTransform)
    {
        this.rewardMultiplier *= rewardMultiplier;
        this.timeMultiplier *= timeMultiplier;

        SpawnOrangePopUp("Time speed x" + this.timeMultiplier + "!\n Rewards x" + this.rewardMultiplier + "!", smashableTransform);

        SpawnRandomSparks(smashableTransform);
    }

    public void BlockClicks(/*float timeBlocked,*/ Transform smashableTransform) // Click block time set by animation.
    {
        SpawnRedPopUp("SMASHING BLOCKED!", smashableTransform);

        SpawnRandomSparks(smashableTransform);

        clickBlockerAnimator.SetTrigger("Animate");
    }

    public void FixNextSpawns(int spawnsAffected, Smashable fixedSmashable, Transform smashableTransform)
    {
        SpawnGreenPopUp("Mint 'em coins!", smashableTransform);

        SpawnRandomSparks(smashableTransform);

        fixedSpawns = true;
        maxFixedSpawns = spawnsAffected;
        this.fixedSmashable = fixedSmashable;
    }
}
