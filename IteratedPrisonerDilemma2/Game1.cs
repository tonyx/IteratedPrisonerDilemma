
#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics.Contracts;

#endregion

namespace IteratedPrisonerDilemma2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>

    public delegate PlayMoves MoveStrategy(List<PlayMoves> myHistoryMoves, List<PlayMoves> opponentHistoryMoves);
    public enum PlayMoves  {
        Cooperate,
        Defect
    };


    public class Game1 : Game   
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        Random random;
        List<Animal> animals;
        List<AnimalType>animalTypes;
        double lastLog;
        StreamWriter csvFile;

        private List<PrisonerDilemmaSequenceOfIterations> games;

        public Game1 ()
        {
            random = new Random (System.DateTime.Now.Millisecond);
            graphics = new GraphicsDeviceManager (this);
            Content.RootDirectory = "Content";                
            graphics.IsFullScreen = false;        
            this.IsMouseVisible = true;

            graphics.PreferredBackBufferHeight = Constants.WINDOW_HEIGHT;
            graphics.PreferredBackBufferWidth = Constants.WINDOW_WIDTH;
 
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize ()
        {
            // TODO: Add your initialization logic here
            games = new List<PrisonerDilemmaSequenceOfIterations>();
            lastLog = -Constants.INTERVAL_FOR_LOG_IN_SECONDS;
            csvFile = new StreamWriter (@Constants.CSV_FILE_OUTPUT_PATH + DateTime.Now.ToString().Replace("/","_").Replace(" ","_").Replace(":","_")+"_PDSimulator.csv");

            csvFile.WriteLine("timebox, name, populationsize, totalscore, averagescore");

            csvFile.AutoFlush = true;

            base.Initialize ();
                
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent ()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch (GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("Arial_12");

            AnimalsCreator animalsCreator = new AnimalsCreator (Content);
            animals = animalsCreator.Animals ();
            animalTypes = animalsCreator.AnimalTypes ();

            //TODO: use this.Content to load your game content here 
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update (GameTime gameTime)
        {
            // For Mobile devices, this logic will close the Game when the Back button is pressed
            // Exit() is obsolete on iOS
            #if !__IOS__
            if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState ().IsKeyDown (Keys.Escape)) {
                Exit ();
            }
            #endif
            // TODO: Add your update logic here            


            foreach (var animal in animals)  {
                animal.Update(gameTime,games);
            }

            foreach (var game in games) {
                game.UpdateGametime (gameTime);
            }


            for (int i = 0; i<animals.Count-1;i++) {
                for (int j = i+1; j<animals.Count;j++) {
                   if (!animals.ElementAt(i).IsInaGame(games)&&!animals.ElementAt(j).IsInaGame(games)) {
                        if (animals.ElementAt (i).CollideWith (animals.ElementAt (j))&& (animals.ElementAt(i).PreviousOpponent!=animals.ElementAt(j).AnimalId)) {
                            var game = new PrisonerDilemmaSequenceOfIterations(Constants.NUM_OF_ITERATIONS_PER_GAME,animals.ElementAt(i),animals.ElementAt(j));
                            games.Add (game);
                        }
                    }
                }
            }

            foreach (var game in games) {
                int[] scores = game.Play ();
                game.Animal1.AddScore (scores [0]);
                game.Animal2.AddScore (scores [1]);
            }

            WeakerAnimalMutatesAccordingToReplDynamics ();

            games = new List<PrisonerDilemmaSequenceOfIterations>();

            WriteToLogIfItIsEnabled(gameTime);

            base.Update (gameTime);
        }



        private void WriteToLogIfItIsEnabled(GameTime gameTime)
        {
            if (Constants.WRITE_TO_LOG)
            {
                if (gameTime.TotalGameTime.TotalSeconds - lastLog >= Constants.INTERVAL_FOR_LOG_IN_SECONDS)
                {
                    lastLog = gameTime.TotalGameTime.TotalSeconds;

                    foreach (var animaltype in animalTypes)
                    {
                        int numAnimalOfCurrentType = animals.Count(x => x.AnimalType == animaltype);
                        int averageScore = (numAnimalOfCurrentType == 0 ? 0 : (animaltype.Score / numAnimalOfCurrentType));
                        csvFile.WriteLine(gameTime.TotalGameTime.TotalSeconds + ", " + animaltype.AnimalTypeName + ", " + numAnimalOfCurrentType + ", " + animaltype.Score + ", " + (int)averageScore);
                    }
                }
            }
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw (GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
        
            //TODO: Add your drawing code here
            spriteBatch.Begin();

            foreach (var animal in animals)  {
                animal.Draw (spriteBatch);
            }

            int index = 0;
            foreach (var animalType in animalTypes) {

                int numAnimalOfCurrentType = animals.Count (x => x.AnimalType == animalType);

                spriteBatch.Draw (animalType.Image, new Vector2 (Constants.PANEL_INIT_X_POSITION, Constants.PANEL_INIT_Y_POSITION + index),Color.White);
                spriteBatch.DrawString (spriteFont, animalType.AnimalTypeName + "  " + animalType.Score+" " +  numAnimalOfCurrentType + " "+
                    (numAnimalOfCurrentType!=0?animalType.Score/numAnimalOfCurrentType:0), 
                    new Vector2 (Constants.PANEL_INIT_X_POSITION + 14 , Constants.PANEL_INIT_Y_POSITION + index), Color.Black);
                index += 40;
            }

            spriteBatch.End ();
            
            base.Draw (gameTime);
        } 



        //private double proportionOfAnimalPresence(AnimalType animalType) {
        //    int nunAnimalOfTheType = animals.FindAll (x => x.AnimalType == animalType).Count;
        //    int totalAnimals = animals.Count;
        //    return (double)nunAnimalOfTheType / (double)totalAnimals;
        //}


        private double ProportionOfAnimalTypeInCurrentPopulation(AnimalType animalType)
        {
            int totalNumberOfAnimals = this.animals.Count;
            return (double)this.animals.FindAll(x => x.AnimalType == animalType).Count / (double)totalNumberOfAnimals;
        }

        private double AverageFitnessForAnimalOfType(AnimalType animalType)
        {
            var animalsOfThatType = this.animals.FindAll(x => x.AnimalType == animalType);
            int fitnessForType = animalsOfThatType.Sum(x => x.Score);
            //int numberOfAnimalOfTheType = this.animals.FindAll(x => x.AnimalType == animalType).Count;

            int numberOfAnimalOfTheType = animalsOfThatType.Count;
            return (double)fitnessForType / (double)numberOfAnimalOfTheType;
        }

        private double TotalProportionTimesFitness()
        {
            double accumul = 0;
            foreach (var innerAnimalType in this.animalTypes)
            {
                accumul += AverageFitnessForAnimalOfType(innerAnimalType)*ProportionOfAnimalTypeInCurrentPopulation(innerAnimalType);
            }
            return accumul;
        }

        private double ProbabilityOfMutationToType(AnimalType animalType)
        {
            return (double)ProportionOfAnimalTypeInCurrentPopulation(animalType) * AverageFitnessForAnimalOfType(animalType) /
                (double)TotalProportionTimesFitness();
        }

        // keep
        private Dictionary<AnimalType, double> ProbabilityTransitionVector()
        {
            Dictionary<AnimalType, double> toReturn = new Dictionary<AnimalType, double>();
            double denominator = TotalProportionTimesFitness();

            foreach (var animalType in this.animalTypes)
            {
                toReturn.Add(animalType, (double)(ProportionOfAnimalTypeInCurrentPopulation(animalType) * AverageFitnessForAnimalOfType(animalType)) / (double)denominator);
            }
            Contract.Ensures(toReturn.Values.Sum() - 1 < 0.1 || toReturn.Values.Sum() > -0.01);
            return toReturn;
        }


        private double relativeFitness(AnimalType animalType) {
            int totalScore = animalTypes.Sum (x => x.Score);
            int animalTypeScore = animalTypes.FindAll (x => x == animalType).Sum (x => x.Score);
            return (double)animalTypeScore / (double)(totalScore);
        }


        private void WeakerAnimalMutatesAccordingToReplDynamics() 
        {
            if (random.Next(100) == 1)
            {
                var orderAnimals = animals.OrderBy(x => x.Score);
                var weakerAnimal = orderAnimals.First();

                // weakerAnimalMutates

                var transitionProbabilities = ProbabilityTransitionVector();
                var requiresSumIsApprox1 = transitionProbabilities.Values.Sum();
                if (requiresSumIsApprox1 - 1 > 0.1 || requiresSumIsApprox1 < 0.01)
                    throw new ApplicationException("error in prerequisite of matrix");

                var cumulativeProbabilities = CumulativeProbabilities(transitionProbabilities);
                double nextRand = random.NextDouble();
                var keys = cumulativeProbabilities.Keys;
                var currentKey = keys.ElementAt(0);
                foreach (var mKey in keys)
                {
                    if (nextRand > cumulativeProbabilities[mKey])
                    {
                        break;
                    }
                    else {
                        currentKey = mKey;
                    }
                }
                weakerAnimal.AnimalType = currentKey;
                var animalsOfThatType = animals.FindAll(x => x.AnimalType == currentKey);
                int averageScoreofType = animalsOfThatType.Sum(x => x.Score)/animalsOfThatType.Count;
                weakerAnimal.Score = averageScoreofType;
            }
        }



        /// <summary>
        /// will transform a dictionary <animaltype,probability>, to a dictionary <animaltype,limit_interval>
        /// example: (type1,0.3), (type2,0.3),(type4,0.4) is transformed into: (type1,0.3), (type2,0.6),(type3,1.0)
        /// </summary>
        /// <returns>the dictionary rearranged according to the example in summary</returns>
        /// <param name="probValues">Prob values.</param>
        private Dictionary<AnimalType,double> CumulativeProbabilities(Dictionary<AnimalType,double> probValues) {
            List<double> cumulatives = new List<double> ();
            Dictionary<AnimalType,double>.ValueCollection values = probValues.Values;
            Dictionary<AnimalType,double>.KeyCollection keys = probValues.Keys;

            Dictionary<AnimalType,double> toReturn = new  Dictionary<AnimalType,double> ();

            double prec = 0.0;
            int index = 0;
            foreach (var value in values) {
                toReturn.Add (keys.ElementAt (index), value + prec);
                prec = value + prec;
                index++;
            }
            return toReturn;
        }
    }
}