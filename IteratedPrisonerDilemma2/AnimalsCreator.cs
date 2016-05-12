using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace IteratedPrisonerDilemma2
{
    public class AnimalsCreator
    {
        private Random random;
        private List<Animal> animals;
        private List<AnimalType> animalTypes;

        public AnimalsCreator (ContentManager content)
        {
            this.random = new Random (System.DateTime.Now.Millisecond);

            AnimalType cooperator = new AnimalType ((x, y) => PlayMoves.Cooperate, content.Load<Texture2D> ("blue_circle"),"Cooperator");
            AnimalType defeater = new AnimalType (new MoveStrategy ((x, y) => PlayMoves.Defect), content.Load<Texture2D> ("green_circle"),"Defector");
            AnimalType randomAnimal = new AnimalType ((x, y) => {
                if (random.Next (2) < 1)
                    return PlayMoves.Cooperate;
                else
                    return PlayMoves.Defect;
            }, content.Load<Texture2D> ("violet_circle"),"Random");

            AnimalType titForTat = new AnimalType ((myMoves, yourMoves) => {
                if (yourMoves == null || yourMoves.Count <= 1)
                    return PlayMoves.Cooperate; else {
                     return (yourMoves.ElementAt(yourMoves.Count-1));
                }

            },content.Load <Texture2D>("yellow_circle"),"TitForTat");

//            AnimalType titForTat = new AnimalType ((x, y) => PlayMoves.Cooperate, content.Load<Texture2D> ("yellow_circle"),"TitForTat");

            animalTypes = new List<AnimalType> ();

            animals = new List<Animal> ();

            animalTypes.Add (titForTat);
            for (int i = 0; i < Constants.ANIMAL_PER_TYPE; i++) {
                animals.Add (new Animal (titForTat));
            }

            animalTypes.Add (defeater);
            for (int i = 0; i < Constants.ANIMAL_PER_TYPE; i++) {
                animals.Add (new Animal (defeater));
            }

            animalTypes.Add (randomAnimal);
            for (int i = 0; i < Constants.ANIMAL_PER_TYPE; i++) {
                animals.Add (new Animal (randomAnimal));
            }

            animalTypes.Add(cooperator);
            for (int i = 0; i < Constants.ANIMAL_PER_TYPE; i++) {
                animals.Add (new Animal (cooperator));
            }

            // shuffle
            List<Animal> shuffledAnimals = animals.OrderBy (x => random.Next ()).ToList();
            animals = shuffledAnimals;

        }


        public List<Animal> ListOfAnimals {
            get {
                return animals;
            }
        }

        public List<Animal> ListOfAnimalTypes {
            get {
                return animals;
            }
        }

        public List<Animal> Animals() {
            return animals;
        }

        public List<AnimalType> AnimalTypes() {
            return animalTypes;
        }

    }
}

