using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace IteratedPrisonerDilemma2
{
    public class AnimalsContainerRef
    {
        private Random random;
        Animal animal1;
        Animal animal2;
        Animal animal3;
        Animal animal4;
        private List<Animal> animals;

        private Dictionary<AnimalType,List<Animal>> animalTypesAnimalMap;


        public AnimalsContainerRef (ContentManager content)
        {
            this.random = new Random (System.DateTime.Now.Millisecond);

            AnimalType cooperator = new AnimalType (new MoveStrategy((x, y) => PlayMoves.Cooperate), content.Load<Texture2D> ("blue_circle"),"Cooperator");
            AnimalType defeater = new AnimalType (new MoveStrategy ((x, y) => PlayMoves.Defeat), content.Load<Texture2D> ("green_circle"),"Defeater");
            AnimalType randomAnimal = new AnimalType ((x, y) => {
                if (random.Next (2) < 1)
                    return PlayMoves.Cooperate;
                else
                    return PlayMoves.Defeat;
            }, content.Load<Texture2D> ("violet_circle"),"Random");

            AnimalType titForTat = new AnimalType ((myMoves, yourMoves) => {
                if (yourMoves == null || yourMoves.Count <= 1)
                    return PlayMoves.Cooperate;
                else {
                    return (yourMoves.ElementAt(yourMoves.Count-1));
                }

            },content.Load <Texture2D>("yellow_circle"),"TitForTat");


            animals = new List<Animal> ();

            //            animal1 = new Animal(cooperator);
            //            animal2 = new Animal(defeater);
            //            animal3 = new Animal (randomAnimal);
            //            animal4 = new Animal (titForTat);
            //            animals.Add (animal1);
            //            animals.Add (animal2);
            //            animals.Add (animal3);
            //            animals.Add (animal4);


            //            for (int i = 0; i < 10; i++) {
            //                animals.Add (new Animal (cooperator));
            //            }

            for (int i = 0; i < 10; i++) {
                animals.Add (new Animal (defeater));
            }

            //            for (int i = 0; i < 10; i++) {
            //                animals.Add (new Animal (randomAnimal));
            //            }
            for (int i = 0; i < 10; i++) {
                animals.Add (new Animal (titForTat));
            }


        }


    }
}

