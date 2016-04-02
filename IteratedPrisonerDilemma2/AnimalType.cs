using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Numerics;

namespace IteratedPrisonerDilemma2
{
    public class AnimalType
    {
        private int score=0;
        private MoveStrategy moveStrategy;
        private Texture2D animalImage;
        private string animalTypeName;

//        public AnimalType (MoveStrategy moveStrategy, Texture2D animalImage)
//        {
//            this.moveStrategy = moveStrategy;
//            this.animalImage = animalImage;
//        }

        public string AnimalTypeName {
            get {
                return this.animalTypeName;
            }
        }

        public AnimalType (MoveStrategy moveStrategy, Texture2D animalImage, string animalTypeName)
        {
            this.animalTypeName = animalTypeName;
            this.moveStrategy = moveStrategy;
            this.animalImage = animalImage;
        }


        public Texture2D Image
        {
            get {
                return this.animalImage;
            }

        }

        public void AddScore(int scoreToAdd) {
            this.score+=scoreToAdd;
        }

        public MoveStrategy MoveStrategy {
            get {
                return this.moveStrategy;
            }
        }

        public int Score {
            get {
                return this.score;
            }
        }

    }
}

