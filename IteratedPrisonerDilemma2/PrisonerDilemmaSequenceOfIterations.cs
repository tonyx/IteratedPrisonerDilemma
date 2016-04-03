using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections;


namespace IteratedPrisonerDilemma2
{
    public class PrisonerDilemmaSequenceOfIterations
    {

        private Dictionary<Tuple<PlayMoves,PlayMoves>,int[]> payoffs;

        private int ellapsedGameTime; 
        private MoveStrategy moveStrategy1;
        private MoveStrategy moveStrategy2;
        private Animal animal1;
        private Animal animal2;
        private bool isRemoving;


        private int numberOfIterations;

        [MethodImpl(MethodImplOptions.Synchronized)] 
        public PrisonerDilemmaSequenceOfIterations (int numberOfIterations,MoveStrategy moveStrategy1, MoveStrategy moveStrategy2, Dictionary<Tuple<PlayMoves,PlayMoves>,int[]> payOffs)
        {
            this.numberOfIterations = numberOfIterations;
            this.moveStrategy1 = moveStrategy1;
            this.moveStrategy2 = moveStrategy2;
            this.payoffs = payOffs;
        }

        [MethodImpl(MethodImplOptions.Synchronized)] 
        public PrisonerDilemmaSequenceOfIterations (int numberOfIterations,MoveStrategy moveStrategy1, MoveStrategy moveStrategy2)
        {
            this.numberOfIterations = numberOfIterations;
            this.moveStrategy1 = moveStrategy1;
            this.moveStrategy2 = moveStrategy2;
            this.payoffs = Constants.Instance ().payoff_matrix;
        }

//        [MethodImpl(MethodImplOptions.Synchronized)] 
//        public PrisonerDilemmaSequenceOfIterations (int numberOfIterations,MoveStrategy moveStrategy1, MoveStrategy moveStrategy2)
//        {
//            this.numberOfIterations = numberOfIterations;
//            this.moveStrategy1 = moveStrategy1;
//            this.moveStrategy2 = moveStrategy2;
//        }

        public Animal Animal1 {
            get {
                return this.animal1;
            }
        }

        public Animal Animal2 {
            get {
                return this.animal2;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)] 
        public PrisonerDilemmaSequenceOfIterations (int numberOfIterations,Animal animal1, Animal animal2)
        {
            this.numberOfIterations = numberOfIterations;
            this.moveStrategy1 = animal1.MoveStrategy;
            this.moveStrategy2 = animal2.MoveStrategy;
            this.animal1 = animal1;
            this.animal2 = animal2;
            this.payoffs = Constants.Instance ().payoff_matrix;
        }

        public bool IsaPlayerInThisGame(Animal animal) {
            return animal==this.animal1 || animal==this.animal2;
        }

        public int[] CumulateGamingScores() {
            int[] scores = new int[]{ 0, 0 };
            List<PlayMoves> accumulMovesOfFirstPlayer = new List<PlayMoves> ();
            List<PlayMoves> accumulMovesOfSecondPlayer = new List<PlayMoves> ();
            for (int i = 0; i < numberOfIterations; i++) {
                PlayMoves currentPlayerOneMove = moveStrategy1 (accumulMovesOfFirstPlayer, accumulMovesOfSecondPlayer);
                PlayMoves currentPlayerTwoMove = moveStrategy2 (accumulMovesOfSecondPlayer, accumulMovesOfFirstPlayer);

                accumulMovesOfFirstPlayer.Add (currentPlayerOneMove);
                accumulMovesOfSecondPlayer.Add (currentPlayerTwoMove);


//                int[] currentScores = Constants.Instance ().payoff_matrix [new Tuple<PlayMoves,PlayMoves> (currentPlayerOneMove, currentPlayerTwoMove)];

                int[] currentScores = payoffs[new Tuple<PlayMoves,PlayMoves> (currentPlayerOneMove, currentPlayerTwoMove)];


                scores [0] += currentScores [0];
                scores [1] += currentScores [1];
            }
            return scores;
        }
        
        public void UpdateGametime(GameTime gameTime) {
            this.ellapsedGameTime += gameTime.ElapsedGameTime.Milliseconds;
            Console.Out.WriteLine (ellapsedGameTime);
        }

    }
}

