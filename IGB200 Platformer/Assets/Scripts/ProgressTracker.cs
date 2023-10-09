using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressTracker
{
    //DEFAULT VALUES - MUST BE REENABLED BEFORE LAUNCH
    //public static bool hasPassedLevel1 = false;
    //public static bool hasPassedLevel2 = false;
    //public static bool hasPassedLevel3 = false;

    //VALUES FOR PLAYTESTS ONLY
    public static bool hasPassedLevel1 = true;
    public static bool hasPassedLevel2 = true;
    public static bool hasPassedLevel3 = true;

    public static int currentLevel = 1;
}
