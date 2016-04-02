﻿using System;
using System.Collections.Generic;

namespace IteratedPrisonerDilemma2
{
    public class Constants
    {

        public const float DIRECTION_ADJUSTMENT_FACTOR = 0.5f;
        public const int NUMBER_OF_ANIMALS = 500; // deprecated
        public const int PLAYING_TIME= 300;
        public const int RIGHT_MARGIN = 200;
        public const int MIN_INTERVAL_BETWEEN_PLAYS=500;
        public const int NUM_OF_ITERATIONS_PER_GAME = 100;
        public const int NUM_OF_UPDATE_CYCLE_FOR_GENERATION = 10000;
        public const int NUM_OF_LOW_RANK_ANIMAL_WHO_WILL_DIE = (int)NUMBER_OF_ANIMALS/10; // deprecated
        public const int PANEL_INIT_Y_POSITION = 50;
        public const int PANEL_INIT_X_MARGIN = 40;
        public const int PANEL_INIT_X_POSITION = WINDOW_WIDTH - RIGHT_MARGIN + PANEL_INIT_X_MARGIN;
        public const int PANEL_STEP_ITEM = 50;
        public const int NUM_ANIMAL_FOR_TYPE = 10; 
        public const int UNAVAILABILITY_TIME = 0; //? 

        public const float ANIMAL_SPEED = 0.30f;
        public const int WINDOW_WIDTH = 800;
        public const int WINDOW_HEIGHT = 600;


        public Dictionary<Tuple<PlayMoves,PlayMoves>,int[]> payoff_matrix;

        public Tuple<int,int> PAYOFF_COOPERATE_COOPERATE = new Tuple<int,int>(9,9);
        public Tuple<int,int> PAYOFF_COOPERATE_DEFEAT = new Tuple<int,int>(0,10);
        public Tuple<int,int> PAYOFF_DEFEAT_COOPERATE = new Tuple<int,int>(10,0);
        public Tuple<int,int> PAYOFF_DEFEAT_DEFEAT = new Tuple<int,int>(2,2);


        private static Constants instance = null;

        private Constants() {
            Init ();
        }

        private void Init() {
            payoff_matrix = new Dictionary<Tuple<PlayMoves,PlayMoves>,int[]> {
                {new Tuple<PlayMoves,PlayMoves>(PlayMoves.Defeat,PlayMoves.Defeat) ,new int[]{4,4}},
                {new Tuple<PlayMoves,PlayMoves>(PlayMoves.Defeat,PlayMoves.Cooperate) ,new int[]{10,0}},
                {new Tuple<PlayMoves,PlayMoves>(PlayMoves.Cooperate,PlayMoves.Defeat) ,new int[]{0,10}},
                {new Tuple<PlayMoves,PlayMoves>(PlayMoves.Cooperate,PlayMoves.Cooperate) ,new int[]{8,8}},
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
