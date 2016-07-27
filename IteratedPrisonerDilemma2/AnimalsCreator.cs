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

            AnimalType defector = new AnimalType (new MoveStrategy ((x, y) => PlayMoves.Defect), content.Load<Texture2D> ("green_circle"),"Defector");
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

            AnimalType titForTwoTats = new AnimalType ((myMoves, yourMoves) => {
                if (yourMoves == null || yourMoves.Count <= 1)
                    return PlayMoves.Cooperate;
                else {
                    if (yourMoves.ElementAt (yourMoves.Count - 1) == PlayMoves.Defect && (yourMoves.ElementAt (yourMoves.Count - 2) == PlayMoves.Defect))
                        return PlayMoves.Defect;
                    else
                        return PlayMoves.Cooperate;
                }

            }, content.Load<Texture2D> ("brown_circle"), "TFtwoT");


            animalTypes = new List<AnimalType> ();

            animals = new List<Animal> ();

            animalTypes.Add (titForTat);
            for (int i = 0; i < (Constants.ANIMAL_PER_TYPE); i++) {
                animals.Add (new Animal (titForTat));
            }


            animalTypes.Add(defector); 
            for (int i = 0; i < Constants.ANIMAL_PER_TYPE; i++) {
                animals.Add (new Animal (defector))
            }

            animalTypes.Add (randomAnimal);
            for (int i = 0; i < Constants.ANIMAL_PER_TYPE; i++) {
                animals.Add (new Animal (randomAnimal));
            }

            animalTypes.Add(cooperator);
            for (int i = 0; i < Constants.ANIMAL_PER_TYPE; i++) {
                animals.Add (new Animal (cooperator));
            }

            animalTypes.Add(titForTwoTats);
            for (int i = 0; i < (Constants.ANIMAL_PER_TYPE); i++) {
                animals.Add (new Animal (titForTwoTats));
            }

            // shuffle
            List<Animal> shuffledAnimals = animals.OrderBy (x => random.Next ()).ToList();
            animals = shuffledAnimals;

        }

        public List<Animal> Animals() {
            return animals;
        }

        public List<AnimalType> AnimalTypes() {
            return animalTypes;
        }
    }
}

