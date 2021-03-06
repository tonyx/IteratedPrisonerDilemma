﻿using System;
using System.Collections.Generic;

namespace IteratedPrisonerDilemma2
{
    public class Constants
    {

        public const float DIRECTION_ADJUSTMENT_FACTOR = 0.5f;
        public const int ANIMAL_PER_TYPE = 30;


        public const int INTERVAL_FOR_LOG_IN_SECONDS = 20;
        public const string CSV_FILE_OUTPUT_PATH = "/Users/Tonyx/IteratedPrisonerDilemmaSimulations/";
        public const bool WRITE_TO_LOG = true;

        public const int LOG_QUANTUM_TIME_IN_MILLIS = 100000;

        public const int RIGHT_MARGIN = 150;

        public const int NUM_OF_ITERATIONS_PER_GAME = 100;

        public const int PANEL_INIT_Y_POSITION = 50;

        public const int PANEL_INIT_X_MARGIN = 5;


        public const int PANEL_INIT_X_POSITION = WINDOW_WIDTH - RIGHT_MARGIN + PANEL_INIT_X_MARGIN - 100;
        public const int PANEL_STEP_ITEM = 50;

        public const float ANIMAL_SPEED = 0.30f;
        public const int WINDOW_WIDTH = 800;
        public const int WINDOW_HEIGHT = 600;

        public Dictionary<Tuple<PlayMoves,PlayMoves>,int[]> payoff_matrix;

        private static Constants instance = null;

        private Constants() {
            Init ();
        }

        private void Init() {
            payoff_matrix = new Dictionary<Tuple<PlayMoves,PlayMoves>,int[]> {
                {new Tuple<PlayMoves,PlayMoves>(PlayMoves.Defect,PlayMoves.Defect) ,new int[]{1,1}},
                {new Tuple<PlayMoves,PlayMoves>(PlayMoves.Defect,PlayMoves.Cooperate) ,new int[]{3,0}},
                {new Tuple<PlayMoves,PlayMoves>(PlayMoves.Cooperate,PlayMoves.Defect) ,new int[]{0,3}},
                {new Tuple<PlayMoves,PlayMoves>(PlayMoves.Cooperate,PlayMoves.Cooperate) ,new int[]{2,2}},
            };
        }

        public static Constants Instance() {
            if (instance == null) {
                instance = new Constants ();
                return instance;
            } else {
                return instance;
            }
        }
    }
}

