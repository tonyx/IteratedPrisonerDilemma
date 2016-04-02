using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Numerics;

namespace IteratedPrisonerDilemma2
{
    public class Agent
    {
        private Vector2 position;
        private Vector2 direction;
        private Random random;
        private BigInteger score;
        private Rectangle drawRectangle; 
        private double angle;
        private Vector2 directionWithSpeed;
        private Texture2D texture;

        public Agent ()
        {
        }
        public Agent(Random random, int initialScore) {
            //this.strGuid = Guid.NewGuid ().ToString ();
            //            strGuid = Guid.NewGuid ().ToString ();
            //this.animalStrategy = animalStrategy;
            this.random = random;
            this.score = initialScore;
            SetInitialRandomPosition ();
        }

        private void SetInitialRandomPosition () {
//            isPlaying = false;
            position = new Vector2 (random.Next (0, Constants.WINDOW_WIDTH-Constants.RIGHT_MARGIN), random.Next (0, Constants.WINDOW_HEIGHT));
            drawRectangle = new Rectangle ((int)position.X, (int)position.Y, texture.Width, texture.Height);

            angle = random.NextDouble () * (Math.PI * 2);
            direction = new Vector2 ((float)Math.Cos (angle), (float)Math.Sin (angle));
            directionWithSpeed.X = direction.X * Constants.ANIMAL_SPEED;
            directionWithSpeed.Y = direction.Y * Constants.ANIMAL_SPEED;

        }

    }
}

