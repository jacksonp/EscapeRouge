using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{

    public int targetFramerate;

    public Transform player;
    private Vector3 lastPosition;

    public GameObject hazard;

    public int hazardCount;
    public int hazardIncrease;
    public int maxHazardCount;

    GameObject[] hazards;

    public float spawnWait;
    public float startWait;

    public Text nobodyEscapesText;
    public Text restartText;

    public int score;
    public Text scoreText;

    public float timePlayed;
    public Text timeText;
    private float time100;
    private float time200;
    private float time500;
    private float time1000;
    private float time2000;

    private bool gameOver;
    private bool restart;
    private bool paused;

    private bool mIsAppLeft = false;

    public Texture twitterImg, whatsAppImg, fbImage;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFramerate;

        hazards = new GameObject[maxHazardCount];
        // Set up object pool of hazards
        for (int i = 0; i < maxHazardCount; i++)
        {
            GameObject obj = (GameObject)Instantiate(hazard);
            obj.SetActive(false);
            hazards[i] = obj;
        }

        StartCoroutine(SpawnWaves());
        gameOver = false;
        restart = false;
        paused = false;
        restartText.text = "";
        restartText.fontSize = Mathf.FloorToInt(restartText.fontSize * MainMenu.dimMultiplier);
        nobodyEscapesText.text = "";
        nobodyEscapesText.fontSize = Mathf.FloorToInt(nobodyEscapesText.fontSize * MainMenu.dimMultiplier);
        //nobodyEscapesText.pixelOffset = new Vector2(0, nobodyEscapesText.pixelOffset.y * MainMenu.dimMultiplier);
        score = 0;
        scoreText.text = "0";
        scoreText.fontSize = Mathf.FloorToInt(scoreText.fontSize * MainMenu.dimMultiplier);
        timePlayed = 0.0f;
        timeText.text = "0.00";
        timeText.fontSize = Mathf.FloorToInt(timeText.fontSize * MainMenu.dimMultiplier);
        time100 = -1.0f;
        time200 = -1.0f;
        time500 = -1.0f;
        time1000 = -1.0f;
        time2000 = -1.0f;
    }

    void OnGUI()
    {

        if (!paused && !restart)
        {
            return;
        }

        GUILayout.BeginArea(new Rect(10, Screen.height / 2 + 50, Screen.width - 20, 700));

        if (paused)
        {
            if (GUILayout.Button(Localise.translate("Resume"), MainMenu.buttonStyle))
            {
                Resume();
            }
        }
        else
        {
            if (GUILayout.Button(Localise.translate("New Game"), MainMenu.buttonStyle))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
        GUILayout.Space(40);

        if (GUILayout.Button(Localise.translate("Main Menu"), MainMenu.buttonStyle))
        {
            if (paused)
            {
                Resume();
            }
            Application.LoadLevel("MainMenu");
        }
        GUILayout.Space(40);

        /*
    if (!paused) {
      string shareTxt = "I%20ran%20" + score + "%20clicks%20through%20Rouge!%20%23nobodyescapes%0Aagorite.com/escaperouge%0A";
      //string shareTxt = "I ran " + score + " clicks through Rouge! #nobodyescapes\nhttp://agorite.com/escaperouge\n";

      GUILayoutOption[] imgButOpts = new GUILayoutOption[] {
        GUILayout.Width(30 * MainMenu.dimMultiplier),
        GUILayout.Height(30 * MainMenu.dimMultiplier)
      };

      GUILayout.BeginHorizontal("box");
      if (GUILayout.Button (twitterImg, imgButOpts)) {
        StartCoroutine (Tweet (shareTxt));
      }
      GUILayout.Space(40);
      if (GUILayout.Button (whatsAppImg, imgButOpts)) {
        Application.OpenURL("whatsapp://send?text=" + shareTxt);
      }
      GUILayout.Space(40);
      if (GUILayout.Button (fbImage, imgButOpts)) {
        Application.OpenURL("http://www.facebook.com/dialog/feed?app_id=753625594683679&link=" + WWW.EscapeURL("http://agorite.com/escaperouge") + 
                            "&name=" + WWW.EscapeURL("Escape Rouge") + "&caption=" + WWW.EscapeURL("Nobody escapes.") + "&description=" + 
                            WWW.EscapeURL("I ran " + score + " clicks through Rouge!") + "&picture=" + WWW.EscapeURL("http://www.agorite.com/img/escape-rouge.png") +
                            "&redirect_uri=" + WWW.EscapeURL("http://www.facebook.com/"));
      }
      GUILayout.EndHorizontal();
    }
        */

        GUILayout.EndArea();

    }

    void FixedUpdate()
    {
        UpdateScore();
    }

    void UpdateScore()
    {
        if (!gameOver)
        {
            scoreText.text = "" + score;
            timeText.text = timePlayed.ToString("F2");
            /*if (player) {
              timeText.text = ((player.position - lastPosition).magnitude).ToString("f2") + " ... " + Input.acceleration.z.ToString("f1");
              lastPosition = player.position;
            }*/
            if (score == 100 || score == 200 || score == 500 || score == 1000 || score == 2000)
            {
                GameService.ReportOngoingAchievement(score);
            }
        }
    }

    void OnApplicationFocus(bool focusStatus)
    {
        if (focusStatus)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    void OnApplicationPause(bool inIsPause)
    {
        this.mIsAppLeft = true;
    }

    /*
    IEnumerator Facebook(string text) {
      this.mIsAppLeft = false;
      Application.OpenURL("fb:post?text=foo");// + text);
      yield return new WaitForSeconds(1f);
      if (this.mIsAppLeft) {
        this.mIsAppLeft = false;
      } else {
        Application.OpenURL("http://www.facebook.com/dialog/feed?app_id=753625594683679&link=" + WWW.EscapeURL("http://agorite.com/escaperouge") + 
                            "&name=" + WWW.EscapeURL("Escape Rouge") + "&caption=" + WWW.EscapeURL("Nobody escapes.") + "&description=" + 
                            WWW.EscapeURL("I ran " + score + " clicks through Rouge!") + "&picture=" + WWW.EscapeURL("http://www.agorite.com/img/escape-rouge.png") +
                            "&redirect_uri=" + WWW.EscapeURL("http://www.facebook.com/"));
      }
    }
  */

    IEnumerator Tweet(string text)
    {
        this.mIsAppLeft = false;
        Application.OpenURL("twitter://post?message=" + text);
        yield return new WaitForSeconds(1f);
        if (this.mIsAppLeft)
        {
            this.mIsAppLeft = false;
        }
        else
        {
            Application.OpenURL("http://twitter.com/intent/tweet?text=" + text + "&amp;lang=en");
        }
    }

    void Update()
    {
        if (player != null)
        {
            score = (int)Mathf.Floor(Vector2.Distance(player.position, Vector2.zero));
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.LoadLevel("MainMenu");
        }
        if (!gameOver && !paused)
        {
            timePlayed += Time.deltaTime;
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Pause();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Pause();
            }
        }
        if (time100 < 0.0f && score == 100)
        {
            time100 = timePlayed;
        }
        if (time200 < 0.0f && score == 200)
        {
            time200 = timePlayed;
        }
        if (time500 < 0.0f && score == 500)
        {
            time500 = timePlayed;
        }
        if (time1000 < 0.0f && score == 1000)
        {
            time1000 = timePlayed;
        }
        if (time2000 < 0.0f && score == 2000)
        {
            time2000 = timePlayed;
        }
    }

    void Pause()
    {
        paused = true;
        Time.timeScale = 0.0f;
        restartText.text = Localise.translate("Paused");
        if (PlayerPrefs.GetInt("Score") < 100)
        {
            restartText.text += "\n" + Localise.translate("Steer: tilt left & right") + "\n" + Localise.translate("Speed: tilt back & forth");
        }
    }

    void Resume()
    {
        paused = false;
        Time.timeScale = 1.0f;
        restartText.text = "";
    }

    public void GameOver()
    {
        Invoke("ResetScreenDim", 1);
        nobodyEscapesText.text = Localise.translate("NOBODY\nESCAPES\nROUGE");
        gameOver = true;
        if (score > PlayerPrefs.GetInt("Score"))
        {
            PlayerPrefs.SetInt("Score", score);
            PlayerPrefs.Save();
        }
        GameService.ReportFinalScore(score);
        if (time100 > 0.0f)
        {
            int time100ms = Mathf.CeilToInt(time100 * 1000);
            if (!PlayerPrefs.HasKey("Time100") || time100ms < PlayerPrefs.GetInt("Time100"))
            {
                PlayerPrefs.SetInt("Time100", time100ms);
                PlayerPrefs.Save();
            }
            GameService.ReportTime(100, time100ms);
        }
        if (time200 > 0.0f)
        {
            int time200ms = Mathf.CeilToInt(time200 * 1000);
            if (!PlayerPrefs.HasKey("Time200") || time200ms < PlayerPrefs.GetInt("Time200"))
            {
                PlayerPrefs.SetInt("Time200", time200ms);
                PlayerPrefs.Save();
            }
            GameService.ReportTime(200, time200ms);
        }
        if (time500 > 0.0f)
        {
            int time500ms = Mathf.CeilToInt(time500 * 1000);
            if (!PlayerPrefs.HasKey("Time500") || time500ms < PlayerPrefs.GetInt("Time500"))
            {
                PlayerPrefs.SetInt("Time500", time500ms);
                PlayerPrefs.Save();
            }
            GameService.ReportTime(500, time500ms);
        }
        if (time1000 > 0.0f)
        {
            int time1000ms = Mathf.CeilToInt(time1000 * 1000);
            if (!PlayerPrefs.HasKey("Time1000") || time1000ms < PlayerPrefs.GetInt("Time1000"))
            {
                PlayerPrefs.SetInt("Time1000", time1000ms);
                PlayerPrefs.Save();
            }
            GameService.ReportTime(1000, time1000ms);
        }
        if (time2000 > 0.0f)
        {
            int time2000ms = Mathf.CeilToInt(time2000 * 1000);
            if (!PlayerPrefs.HasKey("Time2000") || time2000ms < PlayerPrefs.GetInt("Time2000"))
            {
                PlayerPrefs.SetInt("Time2000", time2000ms);
                PlayerPrefs.Save();
            }
            GameService.ReportTime(2000, time2000ms);
        }
    }

    void ResetScreenDim()
    {
        if (gameOver && Screen.sleepTimeout == SleepTimeout.NeverSleep)
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting; // doing this directly after a long game causes the screen to dim instantly.
        }
    }

    IEnumerator SpawnWaves()
    {
        Vector3 spawnPosition;

        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int j = 0; player != null && j < maxHazardCount; j++)
            {
                if (!hazards[j].activeInHierarchy)
                {

                    float rand = Random.Range(0.1f, 2.8f);
                    if (rand < 1)
                    {
                        spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(-0.1f, rand, 0));
                    }
                    else if (rand < 2)
                    {
                        spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(rand - 1, 1.1f, 0));
                    }
                    else
                    {
                        spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, rand - 2 + 0.1f, 0));
                    }

                    spawnPosition.z = 0;
                    hazards[j].transform.position = spawnPosition;

                    hazards[j].SetActive(true);
                }
                yield return new WaitForSeconds(spawnWait);
            }

            if (gameOver)
            {
                scoreText.text = "";
                timeText.text = "";
                restartText.text = "Score: " + score;
                if (time100 > 0)
                {
                    restartText.text += "\n" + Localise.translate("Time to") + " 100: " + time100.ToString("F2");
                }
                if (time200 > 0)
                {
                    restartText.text += "\n" + Localise.translate("Time to") + " 200: " + time200.ToString("F2");
                }
                if (time500 > 0)
                {
                    restartText.text += "\n" + Localise.translate("Time to") + " 500: " + time500.ToString("F2");
                }
                if (time1000 > 0)
                {
                    restartText.text += "\n" + Localise.translate("Time to") + " 1000: " + time1000.ToString("F2");
                }
                if (time2000 > 0)
                {
                    restartText.text += "\n" + Localise.translate("Time to") + " 2000: " + time2000.ToString("F2");
                }
                if (score < 30)
                {
                    restartText.text += "\n" + Localise.translate("Steer: tilt left & right") + "\n" + Localise.translate("Speed: tilt back & forth");
                }
                restart = true;
                break;
            }
        }
    }
}
