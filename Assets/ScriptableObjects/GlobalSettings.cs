using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SettingsSO/Settings")]
public class GlobalSettings : SettingsSO
{
    public enum AILevels
    {
        IDIOT,
        ROOKIE,
        CYBORG
    };


    [Header("Debug")]
    public bool DebugMode;
    [Header("AI Enabled for players")]
    public bool Player1AIEnabled;
    public bool Player2AIEnabled;
    public bool Player3AIEnabled;
    public bool Player4AIEnabled;

    public AILevels AILevel = AILevels.ROOKIE;

    public int GameOverScore;

}
