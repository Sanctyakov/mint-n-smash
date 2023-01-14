using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject mainMenu, HUD, debriefing, youWin, youLose;
    public CustomCursor customCursor;
    public GameObject[] hittables; // Spawn rate of each set by amount in array. Set in editor.
    public ParticleSystem[] sparks;
    public Animator clickBlockerAnimator;
    public Transform spawnPoint, worldSpace, popUpCanvas;
    public TMP_Text userName, timer, score, greenPopUp, redPopUp, orangePopUp;
    public int targetPoints = 200, pointsEarned;

    private GameObject fixedHittable;
    private int fixedSpawnsCounter, maxFixedSpawns;
    private bool fixedSpawns;
    private float time = 120.0f, minSpawnRate, maxSpawnRate, timeCounter, spawnTimeCounter = 0.0f, rewardMultiplier = 1.0f, timeMultiplier = 1.0f;
    
    //Set custom range for random position.
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
                customCursor.gameObject.SetActive(false);
                Cursor.visible = true;
                break;
            case GameStates.Game:
                mainMenu.SetActive(false);
                HUD.SetActive(true);
                debriefing.SetActive(false);
                customCursor.gameObject.SetActive(true);
                Cursor.visible = false;
                break;
            case GameStates.Win:
                mainMenu.SetActive(false);
                HUD.SetActive(true);
                debriefing.SetActive(true);
                youWin.SetActive(true);
                youLose.SetActive(false);
                customCursor.gameObject.SetActive(false);
                Cursor.visible = true;
                break;
            case GameStates.Lose:
                mainMenu.SetActive(false);
                HUD.SetActive(true);
                debriefing.SetActive(true);
                youWin.SetActive(false);
                youLose.SetActive(true);
                customCursor.gameObject.SetActive(false);
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
            SpawnHittableAtRandomPosition();

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
            //timeAlert.Begin(); // Pending implementation.
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

    void SpawnHittableAtRandomPosition()
    {
        float x = Random.Range(spawnMinX, spawnMaxX);

        Vector3 v3 = new Vector3(x, spawnPoint.position.y, spawnPoint.position.z);

        if (fixedSpawns)
        {
            Instantiate(fixedHittable, v3, spawnPoint.rotation);

            fixedSpawnsCounter++;

            if (fixedSpawnsCounter >= maxFixedSpawns)
            {
                fixedSpawns = false;
                fixedSpawnsCounter = 0;
            }
        }
        else
        {
            SpawnRandomHittable(v3);
        }
    }

    void SpawnRandomHittable(Vector3 v3)
    {
        Instantiate(hittables[Random.Range(0, hittables.Length)], v3, spawnPoint.rotation, worldSpace);
    }

    public void SpawnRandomSparks(Transform hittableTransform)
    {
        Instantiate(sparks[Random.Range(0, sparks.Length)], hittableTransform.position, hittableTransform.rotation, worldSpace);
    }

    void SpawnPopUp(TMP_Text popUpToInstantiate, string popUpText, Transform hittableTransform)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(hittableTransform.position);
        TMP_Text t = Instantiate(popUpToInstantiate, pos, Quaternion.identity, popUpCanvas);
        t.text = popUpText;
    }

    public void UpdatePoints(int points, Transform hittableTransform)
    {
        if (gameState != GameStates.Game || points == 0) return; // Don't keep updating points after you've won, or if there are no points to be updated.

        int pointsTotal = (int)(points * rewardMultiplier);

        pointsEarned += pointsTotal;

        if (pointsEarned <= 0) pointsEarned = 0; // Can't have negative points.

        score.text = pointsEarned.ToString();

        if (points < 0)
        {
            SpawnPopUp(redPopUp, pointsTotal.ToString(), hittableTransform); // Negative ints already have a "-".
        }
        else
        {
            SpawnPopUp(greenPopUp, "+" + pointsTotal, hittableTransform);
        }

        if (pointsEarned >= targetPoints)
        {
            ChangeState(GameStates.Win);
        }
    }

    public void UpdateTimer(float newTime, float extraTime, float extraTimeRate, Transform hittableTransform)
    {
        newTime += pointsEarned * extraTime / extraTimeRate;

        time += newTime;

        SpawnPopUp(greenPopUp, "+" + newTime + "s", hittableTransform);
    }

    public void ScrambleHittables(Transform hittableTransform)
    {
        GameObject[] hittablesOnScreen;

        hittablesOnScreen = GameObject.FindGameObjectsWithTag("Hittable");

        foreach (GameObject h in hittablesOnScreen)
        {
            SpawnRandomHittable(h.transform.position);

            Destroy(h);
        }

        SpawnPopUp(orangePopUp, "SCRAMBLE!", hittableTransform);
    }

    public void MultiplyMouseSpeed(float speedMultiplier, float speedBoostDuration, Transform hittableTransform)
    {
        SpawnPopUp(greenPopUp, "Hammer speed x" + speedMultiplier+ "!", hittableTransform);

        customCursor.ChangeSpeed(speedMultiplier, speedBoostDuration);
    }

    public void AccelerateGame(float timeMultiplier, float rewardMultiplier, Transform hittableTransform)
    {
        this.rewardMultiplier *= rewardMultiplier;
        this.timeMultiplier *= timeMultiplier;

        SpawnPopUp(orangePopUp, "Time speed x" + this.timeMultiplier + "!\n Rewards x" + this.rewardMultiplier + "!", hittableTransform);
    }

    public void BlockClicks(/*float timeBlocked,*/ Transform hittableTransform) // Click block time set by animation.
    {
        SpawnPopUp(redPopUp, "HITS BLOCKED!", hittableTransform);

        clickBlockerAnimator.SetTrigger("Animate");
    }

    public void FixNextSpawns(int spawnsAffected, GameObject fixedHittable, Transform hittableTransform)
    {
        SpawnPopUp(greenPopUp, "Mint 'em coins!", hittableTransform);

        fixedSpawns = true;
        maxFixedSpawns += spawnsAffected; // += added. Get two of these in a row, mint double the coins.
        this.fixedHittable = fixedHittable;
    }
}
