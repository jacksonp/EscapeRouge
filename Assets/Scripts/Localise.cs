using UnityEngine;
using System.Collections;

public static class Localise
{

    public static string translate(string text)
    {

        if (Application.systemLanguage == SystemLanguage.French)
        {
            switch (text)
            {
                case "PLAY":
                    return "JOUER";
                case "Quit":
                    return "Quitter";
                case "High Score":
                    return "Score";
                case "Fast":
                    return "Chrono";
                case "Resume":
                    return "Résumer";
                case "New Game":
                    return "Rejouer";
                case "Main Menu":
                    return "Menu Principal";
                case "Paused":
                    return "Pause";
                case "Steer: tilt left & right":
                    return "Direction:\nincliner gache & droite";
                case "Speed: tilt back & forth":
                    return "Vitesse:\nincliner haut & bas";
                case "Time to":
                    return "Chrono";
                case "NOBODY\nESCAPES\nROUGE":
                    return "PERSONNE\nN'ECHAPPE\nROUGE";
                default:
                    return text;
            }
        }
        else
        {
            return text;
        }


    }
}
