using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{

    public Font erFont;
    public int headingFontSize;
    public int buttonFontSize;
    public int scoreFontSize;

    public static float dimMultiplier;

    public static GUIStyle buttonStyle;
    GUIStyle textStyle;
    GUIStyle textStyleScore;

    private static string mStatusText = "Ready.";

    void Start()
    {
        dimMultiplier = Screen.width / 320.0f;
        ResetStatusText();
    }

    public static void ResetStatusText(string text = null)
    {
        if (text == null)
        {
            mStatusText = Localise.translate("High Score") + ": " + PlayerPrefs.GetInt("Score");
            int fast100 = PlayerPrefs.GetInt("Time100");
            if (fast100 > 0)
            {
                mStatusText += "\n" + Localise.translate("Fast") + " 100: " + (fast100 / 1000.0f).ToString("F2");
            }
            int fast200 = PlayerPrefs.GetInt("Time200");
            if (fast200 > 0)
            {
                mStatusText += "\n" + Localise.translate("Fast") + " 200: " + (fast200 / 1000.0f).ToString("F2");
            }
            int fast500 = PlayerPrefs.GetInt("Time500");
            if (fast500 > 0)
            {
                mStatusText += "\n" + Localise.translate("Fast") + " 500: " + (fast500 / 1000.0f).ToString("F2");
            }
            int fast1000 = PlayerPrefs.GetInt("Time1000");
            if (fast1000 > 0)
            {
                mStatusText += "\n" + Localise.translate("Fast") + " 1000: " + (fast1000 / 1000.0f).ToString("F2");
            }
            int fast2000 = PlayerPrefs.GetInt("Time2000");
            if (fast2000 > 0)
            {
                mStatusText += "\n" + Localise.translate("Fast") + " 2000: " + (fast2000 / 1000.0f).ToString("F2");
            }
        }
        else
        {
            mStatusText = text;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnGUI()
    {

        if (!GameService.IsAuthenticated())
        {
            if (PlayerPrefs.HasKey("authenticated"))
            {
                PlayerPrefs.DeleteKey("authenticated");
                GameService.Authenticate(0);
            }
        }

        if (buttonStyle == null)
        {
            buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.font = erFont;
            buttonStyle.fontSize = Mathf.FloorToInt(buttonFontSize * dimMultiplier);
        }

        if (textStyle == null)
        {
            textStyle = new GUIStyle(GUI.skin.label);
            textStyle.font = erFont;
            textStyle.fontSize = Mathf.FloorToInt(headingFontSize * dimMultiplier);
        }

        if (textStyleScore == null)
        {
            textStyleScore = new GUIStyle(GUI.skin.label);
            textStyleScore.font = erFont;
            textStyleScore.fontSize = Mathf.FloorToInt(scoreFontSize * dimMultiplier);
        }

        GUILayout.Label(" ESCAPE\n ROUGE", textStyle);

        GUILayout.BeginArea(new Rect(10, Screen.height / 2 - 100, Screen.width - 20, 800));

        if (GUILayout.Button(Localise.translate("PLAY"), buttonStyle))
        {
            Application.LoadLevel("RandomAsteroids");
        }
        GUILayout.Space(20);

        if (GUILayout.Button(Localise.translate("Leaderboard"), buttonStyle))
        {
            GameService.ShowLeaderboard();
        }
        GUILayout.Space(20);

        if (GUILayout.Button(Localise.translate("Achievements"), buttonStyle))
        {
            GameService.ShowAchievements();
        }
        GUILayout.Space(20);

        if (GUILayout.Button(Localise.translate("Quit"), buttonStyle))
        {
            Application.Quit();
        }

        GUILayout.Space(20);

        GUILayout.Label(mStatusText, textStyleScore);

        GUILayout.EndArea();

    }


}
