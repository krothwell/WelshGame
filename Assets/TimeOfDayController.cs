using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeOfDayController : MonoBehaviour {
    enum TimesOfDay {
        dawn,
        daytime,
        evening,
        night
    }
    Dictionary<TimesOfDay, float> timeOfDayStartDict;
    Dictionary<TimesOfDay, Color32> timeOfDayColourDict;
    Color32 usingColour, currentColour, dawnColour, dayColour, eveningColour, nightColour;
    public float fullDayCycle;
    float fullDayCyclePortion, cycleTransition, currentTime, currentTransition,
          transitionToDay, transitionToEvening, transitionToNight, transitionToDawn;
    TimesOfDay timeOfDayStatus;
	// Use this for initialization
	void Start () {
        fullDayCycle = 100f;
        fullDayCyclePortion = fullDayCycle / 10f;
        cycleTransition = fullDayCycle / 10f;

        dawnColour = new Color32(161, 169, 34, 255);
        dayColour = new Color32(255,255,255,255);
        eveningColour = new Color32(146,51,0,255);
        nightColour = new Color32(27, 65, 118, 255);
        usingColour = dayColour;
        currentColour = dayColour;

        print("current colour: " + currentColour.r + ", " + currentColour.g + ", " + currentColour.b);

        timeOfDayStartDict = new Dictionary<TimesOfDay, float>();
        timeOfDayStartDict.Add(TimesOfDay.daytime, fullDayCyclePortion);
        timeOfDayStartDict.Add(TimesOfDay.evening, fullDayCyclePortion * 5);
        timeOfDayStartDict.Add(TimesOfDay.night, fullDayCyclePortion * 7);
        timeOfDayStartDict.Add(TimesOfDay.dawn, fullDayCyclePortion * 9);
        timeOfDayColourDict = new Dictionary<TimesOfDay, Color32>();
        timeOfDayColourDict.Add(TimesOfDay.daytime, dayColour);
        timeOfDayColourDict.Add(TimesOfDay.evening, eveningColour);
        timeOfDayColourDict.Add(TimesOfDay.night, nightColour);
        timeOfDayColourDict.Add(TimesOfDay.dawn, dawnColour);
        timeOfDayStatus = TimesOfDay.daytime;
        currentTime = timeOfDayStartDict[TimesOfDay.evening];
        currentTransition = 10f;

    }

    void SetTimeOfDay(TimesOfDay timeOfDayIn) {
        if (timeOfDayStatus != timeOfDayIn) {
            timeOfDayStatus = timeOfDayIn;
            currentTransition = 0f;

        }
    }
	
	// Update is called once per frame
	void Update () {
        SetCurrentTime();
        currentTime += Time.deltaTime;
        if (currentTransition < cycleTransition) {
            TransitionTimeOfDay();
        } else {
            CheckToStartTransition();
        }
	}

    void SetCurrentTime() {
        if(currentTime > fullDayCycle) {
            currentTime = 0f;
        } else {
            currentTime += Time.deltaTime;
        }
        print("current time = " + currentTime + " (" + timeOfDayStatus + ")");
    }

    void CheckToStartTransition() {
        if (currentTime >= timeOfDayStartDict[TimesOfDay.dawn]) {
            SetTimeOfDay(TimesOfDay.dawn);
        }
        else if (currentTime >= timeOfDayStartDict[TimesOfDay.night]) {
            
            SetTimeOfDay(TimesOfDay.night);
        }
        else if (currentTime >= timeOfDayStartDict[TimesOfDay.evening]) {
            
            SetTimeOfDay(TimesOfDay.evening);
        }
        else if (currentTime >= timeOfDayStartDict[TimesOfDay.daytime]) {
            SetTimeOfDay(TimesOfDay.daytime);
        }
    }

    //void TransitionTimeOfDay() {
    //    float transitionTimePassed = currentTransition;
    //    float timeRemaining = cycleTransition - transitionTimePassed;
    //    currentTransition = currentTime - timeOfDayStartDict[timeOfDayStatus];
    //    float timePassedInFrame = currentTransition - transitionTimePassed;
    //    float percentageTimePassedInFrame = (1 / timeRemaining) * timePassedInFrame;
    //    int redDiff, greenDiff, blueDiff,
    //        redMod, greenMod, blueMod, //modifier +/- 
    //         newRed, newGreen, newBlue;
    //    redDiff = Math.Abs(currentColour.r - timeOfDayColourDict[timeOfDayStatus].r);
    //    greenDiff = Math.Abs(currentColour.g - timeOfDayColourDict[timeOfDayStatus].g);
    //    blueDiff = Math.Abs(currentColour.b - timeOfDayColourDict[timeOfDayStatus].b);
    //    redMod = ((currentColour.r - timeOfDayColourDict[timeOfDayStatus].r) > 0) ? 1: -1;
    //    greenMod = ((currentColour.g - timeOfDayColourDict[timeOfDayStatus].g) > 0) ? 1 : -1;
    //    blueMod = ((currentColour.b - timeOfDayColourDict[timeOfDayStatus].b) > 0) ? 1 : -1;
    //    newRed = (int)(redDiff / 100 * (redMod * percentageTimePassedInFrame));
    //    newGreen = (int)(greenDiff / 100 * (greenMod * percentageTimePassedInFrame));
    //    newBlue = (int)(blueDiff / 100 * (blueMod * percentageTimePassedInFrame));
    //    print("transitioning to: " + timeOfDayStatus + ", current transition = " + currentTransition);
    //    if (currentTransition > cycleTransition) {
    //        print("transition done");
    //    }
    //}
    void TransitionTimeOfDay() {
        currentTransition = currentTime - timeOfDayStartDict[timeOfDayStatus];
        float percentageToTransitionEnd = (1 / cycleTransition) * currentTransition;
        print("percentage to transitionEnd = " + percentageToTransitionEnd);
        byte newRed, newGreen, newBlue;
        float redMod, greenMod, blueMod,
            redDiff, greenDiff, blueDiff;
        redMod = ((currentColour.r - timeOfDayColourDict[timeOfDayStatus].r) < 0) ? percentageToTransitionEnd : - percentageToTransitionEnd;
        greenMod = ((currentColour.g - timeOfDayColourDict[timeOfDayStatus].g) < 0) ? percentageToTransitionEnd : - percentageToTransitionEnd;
        blueMod = ((currentColour.b - timeOfDayColourDict[timeOfDayStatus].b) < 0) ? percentageToTransitionEnd : - percentageToTransitionEnd;
        redDiff = Math.Abs(currentColour.r - timeOfDayColourDict[timeOfDayStatus].r);
        greenDiff = Math.Abs(currentColour.g - timeOfDayColourDict[timeOfDayStatus].g);
        blueDiff = Math.Abs(currentColour.b - timeOfDayColourDict[timeOfDayStatus].b);
        print("Multipliers: " + redMod + ", " + greenMod + ", " + blueMod);
        print("Diffs: " + redDiff + ", " + greenDiff + ", " + blueDiff);
        newRed = (byte) (currentColour.r + (redDiff * redMod));
        newGreen = (byte)(currentColour.g + (greenDiff * greenMod));
        newBlue = (byte)(currentColour.b + (blueDiff * blueMod));
        usingColour = new Color32(newRed, newGreen, newBlue, 255);
        print("using colour: " + usingColour.r + ", " + usingColour.g + ", " + usingColour.b);
        print("transitioning to: " + timeOfDayStatus + ", current transition = " + currentTransition);
        if (currentTransition > cycleTransition) {
            print("transition done");
            currentColour = timeOfDayColourDict[timeOfDayStatus];
            usingColour = currentColour;
            print("current colour: " + currentColour.r + ", " + currentColour.g + ", " + currentColour.b);
        }
    }
}
