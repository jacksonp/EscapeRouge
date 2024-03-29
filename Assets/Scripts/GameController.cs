﻿using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public int targetFramerate;

    public Transform player;
    //private Vector3 lastPosition;

    public GameObject hazard;

    public int hazardCount;
    public int hazardIncrease;
    public int maxHazardCount;

    GameObject[] hazards;

    public float spawnWait;
    public float startWait;

    public TMP_Text nobodyEscapesText;
    public TMP_Text restartText;

    public int score;

    public TMP_Text scoreText;

    public float timePlayed;
    public TMP_Text timeText;
    private float time100;
    private float time200;
    private float time500;
    private float time1000;
    private float time2000;

    private bool gameOver;
    private bool restart;
    private bool paused;

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
        nobodyEscapesText.text = "";
        score = 0;
        scoreText.text = "0";
        timePlayed = 0.0f;
        timeText.text = "0.00";
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
                SceneManager.LoadScene("RandomAsteroids");
            }
        }
        GUILayout.Space(40);

        if (GUILayout.Button(Localise.translate("Main Menu"), MainMenu.buttonStyle))
        {
            if (paused)
            {
                Resume();
            }
            SceneManager.LoadScene("MainMenu");
        }
        GUILayout.Space(40);

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

    void Update()
    {
        if (player != null)
        {
            score = (int)Mathf.Floor(Vector2.Distance(player.position, Vector2.zero));
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
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
        if (time100 > 0.0f)
        {
            int time100ms = Mathf.CeilToInt(time100 * 1000);
            if (!PlayerPrefs.HasKey("Time100") || time100ms < PlayerPrefs.GetInt("Time100"))
            {
                PlayerPrefs.SetInt("Time100", time100ms);
                PlayerPrefs.Save();
            }
        }
        if (time200 > 0.0f)
        {
            int time200ms = Mathf.CeilToInt(time200 * 1000);
            if (!PlayerPrefs.HasKey("Time200") || time200ms < PlayerPrefs.GetInt("Time200"))
            {
                PlayerPrefs.SetInt("Time200", time200ms);
                PlayerPrefs.Save();
            }
        }
        if (time500 > 0.0f)
        {
            int time500ms = Mathf.CeilToInt(time500 * 1000);
            if (!PlayerPrefs.HasKey("Time500") || time500ms < PlayerPrefs.GetInt("Time500"))
            {
                PlayerPrefs.SetInt("Time500", time500ms);
                PlayerPrefs.Save();
            }
        }
        if (time1000 > 0.0f)
        {
            int time1000ms = Mathf.CeilToInt(time1000 * 1000);
            if (!PlayerPrefs.HasKey("Time1000") || time1000ms < PlayerPrefs.GetInt("Time1000"))
            {
                PlayerPrefs.SetInt("Time1000", time1000ms);
                PlayerPrefs.Save();
            }
        }
        if (time2000 > 0.0f)
        {
            int time2000ms = Mathf.CeilToInt(time2000 * 1000);
            if (!PlayerPrefs.HasKey("Time2000") || time2000ms < PlayerPrefs.GetInt("Time2000"))
            {
                PlayerPrefs.SetInt("Time2000", time2000ms);
                PlayerPrefs.Save();
            }
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
