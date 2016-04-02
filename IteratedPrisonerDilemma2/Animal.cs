using System;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Collections;

namespace IteratedPrisonerDilemma2
{
    public class Animal
    {
        private Vector2 position;
        private Rectangle drawRectangle;
        private double angle;
        private Vector2 direction;
        private Vector2 directionWithSpeed;
        private AnimalType animalType;
        private int score=0;


        public bool IsInaGame(List<PrisonerDilemmaSequenceOfIterations> games) {
            foreach (PrisonerDilemmaSequenceOfIterations game in games) {
                if (game.IsaPlayerInThisGame (this))
                    return true;
            }
            return false;
        }

        public Animal (AnimalType animalType) 
        {
            this.animalType = animalType;
            SetInitialRandomPosition ();
        }


        public void AddScore(int addingScore) {
            this.score += addingScore;
            this.animalType.AddScore (addingScore);
        }

        public MoveStrategy MoveStrategy {
            get {
                return this.animalType.MoveStrategy;
            }
        }

        public AnimalType AnimalType {
            get {
                return this.animalType;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw (this.animalType.Image,drawRectangle,Color.White);
        }


        public void Update(GameTime gameTime, List<PrisonerDilemmaSequenceOfIterations> currentGames) {
            //if (!this.IsInaGame(currentGames) ) {
                adjustDirection ();
                adjustDirection ();

                position.X = position.X + directionWithSpeed.X * gameTime.ElapsedGameTime.Milliseconds;
                position.Y = position.Y + directionWithSpeed.Y * gameTime.ElapsedGameTime.Milliseconds;

                drawRectangle.X = (int)position.X;
                drawRectangle.Y = (int)position.Y;

                if (position.X >= Constants.WINDOW_WIDTH - Constants.RIGHT_MARGIN && direction.X > 0)
                    direction.X *= -1;

                if (position.X <= 0 && direction.X < 0)
                    direction.X *= -1;

                if (position.Y >= Constants.WINDOW_HEIGHT && direction.Y > 0) {
                    direction.Y *= -1;
                }

                if (position.Y <= 0 && direction.Y < 0) {
                    direction.Y *= -1;
                }
            //}
       }

        private void adjustDirection() {

            this.direction.X = this.direction.X + (float)(GlobalRandom.Instance().NextFloat () - 0.5) * Constants.DIRECTION_ADJUSTMENT_FACTOR;
            this.direction.Y = this.direction.Y + (float)(GlobalRandom.Instance().NextFloat () - 0.5) * Constants.DIRECTION_ADJUSTMENT_FACTOR;

            this.direction.Normalize ();

            directionWithSpeed.X = direction.X * Constants.ANIMAL_SPEED;
            directionWithSpeed.Y = direction.Y * Constants.ANIMAL_SPEED;
        }

        private void SetInitialRandomPosition () {
            isPlaying = false;

            position = new Vector2 ( GlobalRandom.Instance().NextInt(Constants.WINDOW_WIDTH-1) , GlobalRandom.Instance().NextInt(Constants.WINDOW_HEIGHT-1));

            drawRectangle = new Rectangle ((int)position.X, (int)position.Y, animalType.Image.Width, animalType.Image.Height);

            angle = GlobalRandom.Instance ().NextDouble () * (Math.PI * 2);

            direction = new Vector2 ((float)Math.Cos (angle), (float)Math.Sin (angle));
            directionWithSpeed.X = direction.X * Constants.ANIMAL_SPEED;
            directionWithSpeed.Y = direction.Y * Constants.ANIMAL_SPEED;
        }

        public bool CollideWith(Animal other) {
            return this.drawRectangle.Intersects (other.drawRectangle);
        }

    }
}




