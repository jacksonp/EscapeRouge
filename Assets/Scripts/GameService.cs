using UnityEngine;
using System.Collections;

using UnityEngine.SocialPlatforms;

#if UNITY_ANDROID

using GooglePlayGames;

public static class GameService {
  
  private const string achIDRun100  = "CgkIhsf3wOQXEAIQAg";
  private const string achIDRun200  = "CgkIhsf3wOQXEAIQAw";
  private const string achIDRun500  = "CgkIhsf3wOQXEAIQBA";
  private const string achIDRun1000 = "CgkIhsf3wOQXEAIQBQ";
  private const string achIDEscape  = "CgkIhsf3wOQXEAIQBg";

  public static bool mWaitingForAuth = false;

  public static bool IsAuthenticated () {
    return PlayGamesPlatform.Instance.localUser.authenticated;
  }

  public static void Authenticate (int doNext) {
    mWaitingForAuth = true;
    MainMenu.ResetStatusText(Localise.translate("Authenticating..."));
    PlayGamesPlatform.Instance.localUser.Authenticate((bool success) => {
      mWaitingForAuth = false;
      MainMenu.ResetStatusText(success ? null : Localise.translate("Authentication failed."));
      if (success) {
        PlayerPrefs.SetInt("authenticated", 1);
        PlayerPrefs.Save();
        if (doNext == 1) {
          PlayGamesPlatform.Instance.ShowLeaderboardUI();
        } else if (doNext == 2) {
          PlayGamesPlatform.Instance.ShowAchievementsUI();
        }
      } else {
        PlayerPrefs.DeleteKey("authenticated"); // in case there is a previously saved value
      }
    });
  }

  public static void SignOut () {
    ((GooglePlayGames.PlayGamesPlatform) PlayGamesPlatform.Instance).SignOut();
    PlayerPrefs.DeleteKey("authenticated");
    PlayerPrefs.Save();
  }

  public static void ShowLeaderboard () {
    if (!mWaitingForAuth) {
      if (PlayGamesPlatform.Instance.localUser.authenticated) {
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
      } else {
        Authenticate(1); 
      }
    }
  }

  public static void ShowAchievements () {  
    if (!mWaitingForAuth) {
      if (PlayGamesPlatform.Instance.localUser.authenticated) {
        PlayGamesPlatform.Instance.ShowAchievementsUI();
      } else {
        Authenticate(2); 
      }
    }
  }
  
  public static void ReportFinalScore (int score) {
    if (!PlayGamesPlatform.Instance.localUser.authenticated) {
      return;
    }
    PlayGamesPlatform.Instance.ReportScore(score, "CgkIhsf3wOQXEAIQBw", (bool success) => {
      PlayerPrefs.SetInt("HighestReportedScore", score);
      PlayerPrefs.Save();
    });
  }
  
  public static void ReportTime (int distance, int time) {
    if (time <= 0 || !PlayGamesPlatform.Instance.localUser.authenticated) {
      return;
    }
    string leaderboardID;
    if (distance == 100) {
      leaderboardID = "CgkIhsf3wOQXEAIQCA";
    } else if (distance == 200) {
      leaderboardID = "CgkIhsf3wOQXEAIQCQ";
    } else if (distance == 500) {
      leaderboardID = "CgkIhsf3wOQXEAIQCg";
    } else if (distance == 1000) {
      leaderboardID = "CgkIhsf3wOQXEAIQCw";
    } else if (distance == 2000) {
      leaderboardID = "CgkIhsf3wOQXEAIQDA";
    } else {
      return;
    }
    PlayGamesPlatform.Instance.ReportScore(time, leaderboardID, (bool success) => {
      PlayerPrefs.SetInt("LowestReportedTime" + distance, time);
      PlayerPrefs.Save();
    });
  }

  public static void ReportOngoingAchievement (int score) {
    
    if (!PlayGamesPlatform.Instance.localUser.authenticated) {
      return;
    }

    int bestPrevReported = PlayerPrefs.GetInt("HighestReportedScore");
    
    if (score < bestPrevReported) {
      return;
    }

    if (bestPrevReported < 100 && score >= 100) {
      PlayGamesPlatform.Instance.ReportProgress(achIDRun100, 100.0f, (bool success) => {});
      PlayGamesPlatform.Instance.ReportProgress(achIDRun200, 0.0f, (bool success) => {});
    }
    if (bestPrevReported < 200 && score >= 200) {
      PlayGamesPlatform.Instance.ReportProgress(achIDRun200, 100.0f, (bool success) => {});
      PlayGamesPlatform.Instance.ReportProgress(achIDRun500, 0.0f, (bool success) => {});
    }
    if (bestPrevReported < 500 && score >= 500) {
      PlayGamesPlatform.Instance.ReportProgress(achIDRun500, 100.0f, (bool success) => {});
      PlayGamesPlatform.Instance.ReportProgress(achIDRun1000, 0.0f, (bool success) => {});
    }
    if (bestPrevReported < 1000 && score >= 1000) {
      PlayGamesPlatform.Instance.ReportProgress(achIDRun1000, 100.0f, (bool success) => {});
      PlayGamesPlatform.Instance.ReportProgress(achIDEscape, 0.0f, (bool success) => {});
    }
    if (bestPrevReported < 2000 && score >= 2000) {
      PlayGamesPlatform.Instance.ReportProgress(achIDEscape, 100.0f, (bool success) => {});
    }


  }


}

#elif UNITY_IPHONE

public static class GameService
{

    public static bool mWaitingForAuth = false;

    public static bool IsAuthenticated()
    {
        return Social.localUser.authenticated;
    }

    public static void Authenticate(int doNext)
    {
        mWaitingForAuth = true;
        MainMenu.ResetStatusText("Authenticating...");
        Social.localUser.Authenticate((bool success) =>
        {
            mWaitingForAuth = false;
            MainMenu.ResetStatusText(success ? null : "Authentication failed.");
            if (success)
            {
                PlayerPrefs.SetInt("authenticated", 1);
                PlayerPrefs.Save();
                if (doNext == 1)
                {
                    Social.ShowLeaderboardUI();
                }
                else if (doNext == 2)
                {
                    Social.ShowAchievementsUI();
                }
            }
            else
            {
                PlayerPrefs.DeleteKey("authenticated"); // in case there is a previously saved value
            }
        });
    }

    public static void SignOut()
    {
        PlayerPrefs.DeleteKey("authenticated");
        PlayerPrefs.Save();
    }

    public static void ShowLeaderboard()
    {
        if (!mWaitingForAuth)
        {
            if (IsAuthenticated())
            {
                Social.ShowLeaderboardUI();
            }
            else
            {
                Authenticate(1);
            }
        }
    }

    public static void ShowAchievements()
    {
        if (!mWaitingForAuth)
        {
            if (IsAuthenticated())
            {
                Social.ShowAchievementsUI();
            }
            else
            {
                Authenticate(2);
            }
        }
    }

    public static void ReportFinalScore(int score)
    {
        if (!Social.localUser.authenticated)
        {
            return;
        }
        Social.ReportScore(score, "LongestRun", (bool success) =>
        {
            PlayerPrefs.SetInt("HighestReportedScore", score);
            PlayerPrefs.Save();
        });
    }

    public static void ReportTime(int distance, int time)
    {
        if (time <= 0 || !Social.localUser.authenticated)
        {
            return;
        }
        string leaderboardID;
        if (distance == 100)
        {
            leaderboardID = "Fast100";
        }
        else if (distance == 200)
        {
            leaderboardID = "Fast200";
        }
        else if (distance == 500)
        {
            leaderboardID = "Fast500";
        }
        else if (distance == 1000)
        {
            leaderboardID = "Fast1000";
        }
        else if (distance == 2000)
        {
            leaderboardID = "Fast2000";
        }
        else
        {
            return;
        }
        Social.ReportScore(time / 10, leaderboardID, (bool success) =>
        {
            PlayerPrefs.SetInt("LowestReportedTime" + distance, time);
            PlayerPrefs.Save();
        });
    }

    public static void ReportOngoingAchievement(int score)
    {
        if (!Social.localUser.authenticated)
        {
            return;
        }

        int bestPrevReported = PlayerPrefs.GetInt("HighestReportedScore");

        if (score < bestPrevReported)
        {
            return;
        }

        if (bestPrevReported < 100 && score >= 100)
        {
            Social.ReportProgress("Run100", 100.0f, (bool success) => { });
            Social.ReportProgress("Run200", 0.0f, (bool success) => { });
        }
        if (bestPrevReported < 200 && score >= 200)
        {
            Social.ReportProgress("Run200", 100.0f, (bool success) => { });
            Social.ReportProgress("Run500", 0.0f, (bool success) => { });
        }
        if (bestPrevReported < 500 && score >= 500)
        {
            Social.ReportProgress("Run500", 100.0f, (bool success) => { });
            Social.ReportProgress("Run1000", 0.0f, (bool success) => { });
        }
        if (bestPrevReported < 1000 && score >= 1000)
        {
            Social.ReportProgress("Run1000", 100.0f, (bool success) => { });
            Social.ReportProgress("EscapeRouge", 0.0f, (bool success) => { });
        }
        if (bestPrevReported < 2000 && score >= 2000)
        {
            Social.ReportProgress("EscapeRouge", 100.0f, (bool success) => { });
        }

    }
}

#endif