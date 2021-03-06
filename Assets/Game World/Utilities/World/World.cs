﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameUtilities {
    public class World {

        public static void PauseGame(float timeSpeed = 0) {
            Time.timeScale = timeSpeed;
        }

        public static void UnpauseGame() {
            Time.timeScale = 1;
        }

        public static bool IsGamePaused() {
            if (Time.timeScale<0.1) {
                return true;
            } else {
                return false;
            }
        }

        public static float GetDistanceFromPositions2D(Vector2 pos1, Vector2 pos2) {
            return (float)Math.Sqrt((Math.Pow((pos1.x - pos2.x), 2)
                                  + (Math.Pow((pos1.y - pos2.y), 2))));
        }

        public static Vector2 GetVector2DistanceFromPositions2D(Vector2 pos1, Vector2 pos2) {
            return new Vector2( Math.Abs(pos1.x - pos2.x), 
                                Math.Abs(pos1.y - pos2.y));
        }

    }

}