using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeOfDayController : MonoBehaviour {
    enum TimesOfDay {
        dawn,
        daytime,
        evening,
        lateEvening,
        night,
        lateNight,
    }
    NightLight[] nightLights;
    Dictionary<TimesOfDay, float> timeOfDayStartDict;
    Dictionary<TimesOfDay, Color32> timeOfDayColourDict;
    Dictionary<TimesOfDay, float> timeOfDayEarthRotationDict;
    Dictionary<TimesOfDay, float> timeofDayIntensityDict;
    Color32 currentColour, dawnColour, dayColour, eveningColour, nightColour;
    public float fullDayCycle;
    float fullDayCyclePortion, cycleTransition, currentTime, currentTransition, currentRotation, currentIntensity,
          transitionToDay, transitionToEvening, transitionToNight, transitionToDawn;
    TimesOfDay timeOfDayStatus;
    Light sun;
 
	// Use this for initialization
	void Start () {

        nightLights = FindObjectsOfType<NightLight>();
        TurnOffNightLights();
        print(nightLights.Length + " nightlights found");

        sun = transform.Find("Sun").GetComponent<Light>();
        fullDayCycle = 100f;
        fullDayCyclePortion = fullDayCycle / 10f;
        cycleTransition = fullDayCycle / 10f;


        dawnColour = new Color32(255, 223, 143, 255);
        dayColour = new Color32(255,255,255,255);
        eveningColour = new Color32(234,112,33,255);
        nightColour = new Color32(255, 255, 255, 255);
        currentColour = dayColour;

        print("current colour: " + currentColour.r + ", " + currentColour.g + ", " + currentColour.b);

        timeOfDayStartDict = new Dictionary<TimesOfDay, float>();
        timeOfDayStartDict.Add(TimesOfDay.daytime, fullDayCyclePortion);
        timeOfDayStartDict.Add(TimesOfDay.evening, fullDayCyclePortion * 5f);
        timeOfDayStartDict.Add(TimesOfDay.lateEvening, fullDayCyclePortion * 6f);
        timeOfDayStartDict.Add(TimesOfDay.night, fullDayCyclePortion * 7f);
        timeOfDayStartDict.Add(TimesOfDay.lateNight, fullDayCyclePortion * 8f);
        timeOfDayStartDict.Add(TimesOfDay.dawn, fullDayCyclePortion * 9f);
        timeOfDayColourDict = new Dictionary<TimesOfDay, Color32>();
        timeOfDayColourDict.Add(TimesOfDay.daytime, dayColour);
        timeOfDayColourDict.Add(TimesOfDay.evening, eveningColour);
        timeOfDayColourDict.Add(TimesOfDay.lateEvening, nightColour);
        timeOfDayColourDict.Add(TimesOfDay.night, nightColour);
        timeOfDayColourDict.Add(TimesOfDay.lateNight, nightColour);
        timeOfDayColourDict.Add(TimesOfDay.dawn, dawnColour);
        timeOfDayEarthRotationDict = new Dictionary<TimesOfDay, float>();
        timeOfDayEarthRotationDict.Add(TimesOfDay.daytime, 0f);
        timeOfDayEarthRotationDict.Add(TimesOfDay.evening, 65f);
        timeOfDayEarthRotationDict.Add(TimesOfDay.lateEvening, 45f);
        timeOfDayEarthRotationDict.Add(TimesOfDay.night, 0f);
        timeOfDayEarthRotationDict.Add(TimesOfDay.lateNight, -35f);
        timeOfDayEarthRotationDict.Add(TimesOfDay.dawn, -65f);
        timeofDayIntensityDict = new Dictionary<TimesOfDay, float>();
        timeofDayIntensityDict.Add(TimesOfDay.daytime, 0.85f);
        timeofDayIntensityDict.Add(TimesOfDay.evening, 1.2f);
        timeofDayIntensityDict.Add(TimesOfDay.lateEvening, 0.25f);
        timeofDayIntensityDict.Add(TimesOfDay.night, 0.2f);
        timeofDayIntensityDict.Add(TimesOfDay.lateNight, 0.25f);
        timeofDayIntensityDict.Add(TimesOfDay.dawn, 1.2f);


        timeOfDayStatus = TimesOfDay.daytime;
        currentTime = timeOfDayStartDict[TimesOfDay.evening];
        currentTransition = 10f;
        currentRotation = timeOfDayEarthRotationDict[TimesOfDay.daytime];
        currentIntensity = timeofDayIntensityDict[TimesOfDay.daytime];
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
        //print("late night starts: " + timeOfDayStartDict[TimesOfDay.lateNight]);
        //print("current time = " + currentTime + " (" + timeOfDayStatus + ")");
    }

    void CheckToStartTransition() {
        
        if (currentTime >= timeOfDayStartDict[TimesOfDay.dawn]) {
            SetTimeOfDay(TimesOfDay.dawn);
        }
        else if (currentTime >= timeOfDayStartDict[TimesOfDay.lateNight]) {
            SetTimeOfDay(TimesOfDay.lateNight);
        }

        else if (currentTime >= timeOfDayStartDict[TimesOfDay.night]) {
            
            SetTimeOfDay(TimesOfDay.night);
        }
        else if (currentTime >= timeOfDayStartDict[TimesOfDay.lateEvening]) {

            SetTimeOfDay(TimesOfDay.lateEvening);
        }
        else if (currentTime >= timeOfDayStartDict[TimesOfDay.evening]) {
            
            SetTimeOfDay(TimesOfDay.evening);
        }


        else if (currentTime >= timeOfDayStartDict[TimesOfDay.daytime]) {
            SetTimeOfDay(TimesOfDay.daytime);
        }
    }

    void TransitionTimeOfDay() {
        currentTransition = currentTime - timeOfDayStartDict[timeOfDayStatus];
        float percentageToTransitionEnd = (1 / cycleTransition) * currentTransition;
       // print("percentage to transitionEnd = " + percentageToTransitionEnd);
        byte newRed, newGreen, newBlue;
        float newRotation, newIntensity,
            redMod, greenMod, blueMod, rotationMod, intensityMod,
            redDiff, greenDiff, blueDiff, rotationDiff, intensityDiff;
        intensityMod = ((currentIntensity - timeofDayIntensityDict[timeOfDayStatus]) < 0) ? percentageToTransitionEnd : -percentageToTransitionEnd;
        rotationMod = ((currentRotation - timeOfDayEarthRotationDict[timeOfDayStatus]) < 0) ? percentageToTransitionEnd : -percentageToTransitionEnd;
        redMod = ((currentColour.r - timeOfDayColourDict[timeOfDayStatus].r) < 0) ? percentageToTransitionEnd : - percentageToTransitionEnd;
        greenMod = ((currentColour.g - timeOfDayColourDict[timeOfDayStatus].g) < 0) ? percentageToTransitionEnd : - percentageToTransitionEnd;
        blueMod = ((currentColour.b - timeOfDayColourDict[timeOfDayStatus].b) < 0) ? percentageToTransitionEnd : - percentageToTransitionEnd;
        intensityDiff = Math.Abs(currentIntensity - timeofDayIntensityDict[timeOfDayStatus]);
        rotationDiff = Math.Abs(currentRotation - timeOfDayEarthRotationDict[timeOfDayStatus]);
        redDiff = Math.Abs(currentColour.r - timeOfDayColourDict[timeOfDayStatus].r);
        greenDiff = Math.Abs(currentColour.g - timeOfDayColourDict[timeOfDayStatus].g);
        blueDiff = Math.Abs(currentColour.b - timeOfDayColourDict[timeOfDayStatus].b);
        //print("Multipliers: " + redMod + ", " + greenMod + ", " + blueMod);
        //print("Diffs: " + redDiff + ", " + greenDiff + ", " + blueDiff);
        newIntensity = currentIntensity + (intensityDiff * intensityMod);
        newRotation = currentRotation + (rotationDiff * rotationMod);
        newRed = (byte) (currentColour.r + (redDiff * redMod));
        newGreen = (byte)(currentColour.g + (greenDiff * greenMod));
        newBlue = (byte)(currentColour.b + (blueDiff * blueMod));
        //usingColour = new Color32(newRed, newGreen, newBlue, 255);
        //usingRotation = newRotation;
        //print("using colour: " + usingColour.r + ", " + usingColour.g + ", " + usingColour.b);
        //print("transitioning to: " + timeOfDayStatus + ", current transition = " + currentTransition);
        sun.color = new Color32(newRed, newGreen, newBlue, 255);
        sun.transform.rotation = Quaternion.Euler(-20f, newRotation, 0);
        sun.intensity = newIntensity;

        if (currentTransition > cycleTransition) {
            //print("transition done");
            currentColour = timeOfDayColourDict[timeOfDayStatus];
            currentRotation = timeOfDayEarthRotationDict[timeOfDayStatus];
            currentIntensity = timeofDayIntensityDict[timeOfDayStatus];
            if (timeOfDayStatus == TimesOfDay.lateEvening) {
                TurnOnNightLights();
            } else if (timeOfDayStatus == TimesOfDay.lateNight) {
                TurnOffNightLights();
            }
            //usingColour = currentColour;
            //usingRotation = currentRotation;
            //print("current colour: " + currentColour.r + ", " + currentColour.g + ", " + currentColour.b);
        }
    }

    void TurnOnNightLights() {
        print("turning on night lights");
        foreach (NightLight nl in nightLights) {
            nl.TurnOn();
        }
    }

    void TurnOffNightLights() {
        print("turning off night lights");
        foreach (NightLight nl in nightLights) {
            nl.TurnOff();
        }
    }
}
