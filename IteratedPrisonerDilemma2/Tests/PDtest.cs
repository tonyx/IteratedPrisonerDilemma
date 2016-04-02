using System;
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


        [Test]
        public void myTest() {
            Assert.AreEqual (2, 2);
        }



        [Test]
        public void DefeaterWillDefeatAtFirstMove() {
            MoveStrategy defeatStrategy = (x,y) => PlayMoves.Defeat;
            var expected = PlayMoves.Defeat;

            var actual = defeatStrategy (new List<PlayMoves> (), new List<PlayMoves> ());

            Assert.AreEqual (expected, actual);

        }

        [Test]
        public void DefeaterWillDefeatAtAnyTime() {
            MoveStrategy defeatStrategy = (x,y) => PlayMoves.Defeat;
            var expected = PlayMoves.Defeat;

            var actual = defeatStrategy (new List<PlayMoves> {PlayMoves.Defeat}, new List<PlayMoves> {PlayMoves.Defeat});

            Assert.AreEqual (expected, actual);

        }

        [Test]
        public void TitForTatWillCooperateAtTheFirstTime() {
            MoveStrategy titForTat = (myMoves, yourMoves) => {
                if (yourMoves == null || yourMoves.Count == 0)
                    return PlayMoves.Cooperate;
                else
                    return yourMoves.Last ();
            }; 

            var expected = PlayMoves.Cooperate;

            var actual = titForTat (new List<PlayMoves>(),new List<PlayMoves>());

            Assert.AreEqual (expected, actual);
        }

//        [Test]
//        public void AOneTimeSequenceOfGamesWithTwoDefeaters() {
//            MoveStrategy defeatStrategy = (x,y) => PlayMoves.Defeat;
//            PrisonerDilemmaSequenceOfIterations game = new PrisonerDilemmaSequenceOfIterations (defeatStrategy, defeatStrategy);
//
//            int[] scores = game.OneMoveScore (defeatStrategy, defeatStrategy,new List<PlayMoves>(),new List<PlayMoves>());
//            Assert.AreEqual (4, scores [0]);
//            Assert.AreEqual (4, scores [1]);
//
//        }

//        [Test]
//        public void AOnTimeSequenceOfGamesWithTwoCooperators() {
//            MoveStrategy cooperateStrategy = (x,y) => PlayMoves.Cooperate;
//            PrisonerDilemmaSequenceOfIterations game = new PrisonerDilemmaSequenceOfIterations (cooperateStrategy, cooperateStrategy);
//
//            int[] scores = game.OneMoveScore (cooperateStrategy, cooperateStrategy,new List<PlayMoves>(),new List<PlayMoves>());
//            Assert.AreEqual (8, scores [0]);
//            Assert.AreEqual (8, scores [1]);
//
//        }

        [Test]
        public void GetCumulateScoreOfTwoDefeaters() {
            MoveStrategy defeatStrategy = (x,y) => PlayMoves.Defeat;
            PrisonerDilemmaSequenceOfIterations game = new PrisonerDilemmaSequenceOfIterations (2,defeatStrategy,defeatStrategy);
            int[] scores = game.CumulateGamingScores();
            Assert.AreEqual (8, scores [0]);
            Assert.AreEqual (8, scores [1]);

        }

        [Test]
        public void GetCumulateScoreOfTwoDefeatersInAGameOfThreeSteps() {
            MoveStrategy defeatStrategy = (x,y) => PlayMoves.Defeat;
            PrisonerDilemmaSequenceOfIterations game = new PrisonerDilemmaSequenceOfIterations (3,defeatStrategy,defeatStrategy);
            int[] scores = game.CumulateGamingScores();
            Assert.AreEqual (12, scores [0]);
            Assert.AreEqual (12, scores [1]);

        }

        [Test]
        public void GetCumulateScoreOfTwoCooperatorsInAGameOfThreeSteps() {
            MoveStrategy cooperator = (x,y) => PlayMoves.Cooperate;
            PrisonerDilemmaSequenceOfIterations game = new PrisonerDilemmaSequenceOfIterations (3,cooperator,cooperator);
            int[] scores = game.CumulateGamingScores();
            Assert.AreEqual (24, scores [0]);
            Assert.AreEqual (24, scores [1]);

        }

        [Test]
        public void CumulateScoreOfACooperatorAgainstADefeater() {
            MoveStrategy cooperator = (x,y) => PlayMoves.Cooperate;
            MoveStrategy defeater = (x,y) => PlayMoves.Defeat;
            PrisonerDilemmaSequenceOfIterations game = new PrisonerDilemmaSequenceOfIterations (3,cooperator,defeater);
            int[] scores = game.CumulateGamingScores();
            Assert.AreEqual (0, scores [0]);
            Assert.AreEqual (30, scores [1]);

        }



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



