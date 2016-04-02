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
        int prisonerDilemmaPlayingTime;
        private bool isPlaying = false;
        private Vector2 position;
        private Rectangle drawRectangle;
        private double angle;
        private Vector2 direction;
        private Vector2 directionWithSpeed;
        private AnimalType animalType;
        private PrisonerDilemmaSequenceOfIterations currentGame;
        private int score=0;

        private bool availableForGame = true;

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

        public PrisonerDilemmaSequenceOfIterations CurrentGame {
            set {
                this.currentGame = value;
            }

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

        public bool IsPlaying {
            
            private set {
                this.isPlaying = value;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
                get {
                    return this.isPlaying;
                }
        }

        public AnimalType AnimalType {
            get {
                return this.animalType;
            }
        }

        public bool AvailableForAGame {
            get {
                return this.availableForGame;
            }
            set {
                this.availableForGame = value;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw (this.animalType.Image,drawRectangle,Color.White);
        }


        public void Update(GameTime gameTime, List<PrisonerDilemmaSequenceOfIterations> currentGames) {
            if (!this.IsInaGame(currentGames) ) {
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
            }
       }


        public void Update(GameTime gameTime) {


            if (!IsPlaying) {
                adjustDirection ();

                position.X = position.X + directionWithSpeed.X * gameTime.ElapsedGameTime.Milliseconds;
                position.Y = position.Y + directionWithSpeed.Y * gameTime.ElapsedGameTime.Milliseconds;
                Console.WriteLine (gameTime.ElapsedGameTime.Milliseconds);
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
            } else {
                this.prisonerDilemmaPlayingTime += gameTime.ElapsedGameTime.Milliseconds; 
                if (this.prisonerDilemmaPlayingTime > Constants.PLAYING_TIME) {
                    StopPlaying ();
                }

//                if (this.prisonerDilemmaPlayingTime > Constants.UNAVAILABILITY_TIME) {
//                    AvailableForAGame = true;
//                }

            }
            if (this.prisonerDilemmaPlayingTime > Constants.UNAVAILABILITY_TIME) {
                AvailableForAGame = true;
            }
            Console.WriteLine ("exit"  + gameTime.ElapsedGameTime.Milliseconds);

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

        public void StartPlaying() {
            this.isPlaying = true;
            this.prisonerDilemmaPlayingTime = 0;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void StartPlayWith(Animal other,GameTime gameTime) {

            this.prisonerDilemmaPlayingTime = 0;
            this.IsPlaying = true;
            other.prisonerDilemmaPlayingTime = 0;
            other.IsPlaying = true;

        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void StopPlaying() {
            IsPlaying = false;
            this.prisonerDilemmaPlayingTime = 0;
        }

    }
}




