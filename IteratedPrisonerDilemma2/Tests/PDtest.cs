﻿using System;
using NUnit.Framework;

using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace IteratedPrisonerDilemma2
{
    [TestFixture]
    public class PDtest
    {
        Dictionary<Tuple<PlayMoves,PlayMoves>,int[]> payoff_matrix
         = new Dictionary<Tuple<PlayMoves,PlayMoves>,int[]> {
            {new Tuple<PlayMoves,PlayMoves>(PlayMoves.Defect,PlayMoves.Defect) ,new int[]{1,1}},
            {new Tuple<PlayMoves,PlayMoves>(PlayMoves.Defect,PlayMoves.Cooperate) ,new int[]{3,0}},
            {new Tuple<PlayMoves,PlayMoves>(PlayMoves.Cooperate,PlayMoves.Defect) ,new int[]{0,3}},
            {new Tuple<PlayMoves,PlayMoves>(PlayMoves.Cooperate,PlayMoves.Cooperate) ,new int[]{2,2}},
        };

        [SetUp]
        public void SetUp() {

        }

        [Test]
        public void myTest() {
            Assert.AreEqual (2, 2);
        }



//        [Test]
//        public void DefecterWillDefectAtFirstMove() {
//            MoveStrategy defeatStrategy = (x,y) => PlayMoves.Defect;
//            var expected = PlayMoves.Defect;
//
//            var actual = defeatStrategy (new List<PlayMoves> (), new List<PlayMoves> ());
//
//            Assert.AreEqual (expected, actual);
//        }
//
//        [Test]
//        public void DefecterWillDefectAtAnyTime() {
//            MoveStrategy defeatStrategy = (x,y) => PlayMoves.Defect;
//            var expected = PlayMoves.Defect;
//
//            var actual = defeatStrategy (new List<PlayMoves> {PlayMoves.Defect}, new List<PlayMoves> {PlayMoves.Defect});
//
//            Assert.AreEqual (expected, actual);
//
//        }
//
//        [Test]
//        public void TitForTatWillCooperateAtTheFirstTime() {
//            MoveStrategy titForTat = (myMoves, yourMoves) => {
//                if (yourMoves == null || yourMoves.Count == 0)
//                    return PlayMoves.Cooperate;
//                else
//                    return yourMoves.Last ();
//            }; 
//
//            var expected = PlayMoves.Cooperate;
//
//            var actual = titForTat (new List<PlayMoves>(),new List<PlayMoves>());
//
//            Assert.AreEqual (expected, actual);
//        }
//
//
//        [Test]
//        public void OneShotOfTwoDefeatersWillGetOneForBoth() {
//            MoveStrategy defeatStrategy = (x,y) => PlayMoves.Defect;
//            PrisonerDilemmaSequenceOfIterations game = new PrisonerDilemmaSequenceOfIterations (1,defeatStrategy,defeatStrategy,payoff_matrix);
//            int[] scores = game.CumulateGamingScores();
//            Assert.AreEqual (1, scores [0]);
//            Assert.AreEqual (1, scores [1]);
//        }
//
//
//
//
//        [Test]
//        public void TwoDefeatersPlayThreeTimes() {
//            MoveStrategy defeatStrategy = (x,y) => PlayMoves.Defect;
//            PrisonerDilemmaSequenceOfIterations game = new PrisonerDilemmaSequenceOfIterations (3,defeatStrategy,defeatStrategy,payoff_matrix);
//            int[] scores = game.CumulateGamingScores();
//            Assert.AreEqual (3, scores [0]);
//            Assert.AreEqual (3, scores [1]);
//
//        }
//
//        [Test]
//        public void CooperatorAndTitForTatPlayTheSame() {
//
//            MoveStrategy titForTat = ((myMoves, yourMoves) => {
//                if (yourMoves == null || yourMoves.Count <= 1)
//                return PlayMoves.Cooperate; else {
//                    return (yourMoves.ElementAt(yourMoves.Count-1));
//                }}
//            );
//
//            MoveStrategy cooperator = (x,y) => PlayMoves.Cooperate;
//            PrisonerDilemmaSequenceOfIterations game = new PrisonerDilemmaSequenceOfIterations (3,titForTat,cooperator,payoff_matrix);
//            int[] scores = game.CumulateGamingScores();
//            Assert.AreEqual (6, scores [0]);
//            Assert.AreEqual (6, scores [1]);
//
//        }
//
//
//        [Test]
//        public void CooperatorLoseWithDefecter() {
//
//            MoveStrategy cooperator = (x,y) => PlayMoves.Cooperate;
//            MoveStrategy defecter = (x,y) => PlayMoves.Defect;
//            PrisonerDilemmaSequenceOfIterations game = new PrisonerDilemmaSequenceOfIterations (3,cooperator,defecter,payoff_matrix);
//            int[] scores = game.CumulateGamingScores();
//            Assert.AreEqual (0, scores [0]);
//            Assert.AreEqual (9, scores [1]);
//
//        }
//
//
//        [Test]
//        public void TitForTatWillCooperateWithDefecterInTheFirstMove() {
//            MoveStrategy titForTat = ((myMoves, yourMoves) => {
//                if (yourMoves == null || yourMoves.Count <= 1)
//                return PlayMoves.Cooperate; else {
//                    return (yourMoves.ElementAt(yourMoves.Count-1));
//                }}
//            );
//
//            MoveStrategy defecter = (x,y) => PlayMoves.Defect;
//            PrisonerDilemmaSequenceOfIterations game = new PrisonerDilemmaSequenceOfIterations (1,titForTat,defecter,payoff_matrix);
//            int[] scores = game.CumulateGamingScores();
//            Assert.AreEqual (0, scores [0]);
//            Assert.AreEqual (3, scores [1]);
//        }
//



        [Test]
        public void TitForTatWillCooperateWithDefecterInTheFirstMoveAndThenDefect() {
            MoveStrategy titForTat = ((myMoves, yourMoves) => {
                if (yourMoves == null || yourMoves.Count < 1) {
                    return PlayMoves.Cooperate;} else {
                    return (yourMoves.ElementAt(yourMoves.Count-1));
                }}
            );

            MoveStrategy defecter = (x,y) => PlayMoves.Defect;
            PrisonerDilemmaSequenceOfIterations game = new PrisonerDilemmaSequenceOfIterations (2,titForTat,defecter,payoff_matrix);
            int[] scores = game.CumulateGamingScores();
            Assert.AreEqual (1, scores [0]);
            Assert.AreEqual (4, scores [1]);
        }



//
//        [Test]
//        public void GetCumulateScoreOfTwoCooperatorsInAGameOfThreeSteps() {
//            MoveStrategy cooperator = (x,y) => PlayMoves.Cooperate;
//            PrisonerDilemmaSequenceOfIterations game = new PrisonerDilemmaSequenceOfIterations (3,cooperator,cooperator);
//            int[] scores = game.CumulateGamingScores();
//            Assert.AreEqual (24, scores [0]);
//            Assert.AreEqual (24, scores [1]);
//
//        }
//
//        [Test]
//        public void CumulateScoreOfACooperatorAgainstADefeater() {
//            MoveStrategy cooperator = (x,y) => PlayMoves.Cooperate;
//            MoveStrategy defeater = (x,y) => PlayMoves.Defect;
//            PrisonerDilemmaSequenceOfIterations game = new PrisonerDilemmaSequenceOfIterations (3,cooperator,defeater);
//            int[] scores = game.CumulateGamingScores();
//            Assert.AreEqual (0, scores [0]);
//            Assert.AreEqual (30, scores [1]);
//
//        }
//
//
//        [Test]
//        public void CooperatorAndTitForTatWillBoothCooperateGettingTheSameScore() {
//
//        }



//        [Test]
//        public void PayoffForDefeatDefeatIs4_4() {
//            MoveStrategy defeatStrategy = (x,y) => PlayMoves.Defeat;
//            var expected = 4;
//            var moves = new List<PlayMoves> ();
//
//            var behaviorPlayer1 = defeatStrategy (moves, moves);
//            var behaviorPlayer2 = defeatStrategy (moves, moves);
//
//            int[] payoffs = getPayOffs (behaviorPlayer1, behaviorPlayer2);
//
//            Assert.AreEqual ();
//
//
//        }







    }

}



