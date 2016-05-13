#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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

        //private Animal animal; //private Animal animal2;
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
            csvFile = new StreamWriter (@Constants.CSV_FILE_OUTPUT_PATH + "PDSimulator.csv");
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


            // check for collisions for free animals and make them play

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

            // elaborate all current games:

            foreach (var game in games) {
                int[] scores = game.Play ();
                game.Animal1.AddScore (scores [0]);
                game.Animal2.AddScore (scores [1]);
            }

            WeakerPlayerMayBecomeOfTheTypeOfTheStronger ();

            games = new List<PrisonerDilemmaSequenceOfIterations>();

            //Console.Out.WriteLine (gameTime.TotalGameTime);
            if (gameTime.TotalGameTime.TotalSeconds - lastLog >= Constants.INTERVAL_FOR_LOG_IN_SECONDS) {
                lastLog = gameTime.TotalGameTime.TotalSeconds;

                foreach (var animaltype in animalTypes) {
                    int numAnimalOfCurrentType = animals.Count (x => x.AnimalType == animaltype);
                    int averageScore = (numAnimalOfCurrentType == 0 ? 0 : (animaltype.Score/numAnimalOfCurrentType));
                    csvFile.WriteLine (gameTime.TotalGameTime.TotalSeconds+", "+animaltype.AnimalTypeName+", "+ numAnimalOfCurrentType+", "+ animaltype.Score+", "+(int)averageScore);
                }

            }

            base.Update (gameTime);
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


        private void WeakerPlayerMayBecomeOfTheTypeOfTheStronger() {

            if (random.Next (10) == 1) {

                // the weaker transforms in the type of the stronger

                int weakerScore = animals [0].Score;
                Animal weakerAnimal = animals [0];

                foreach (var animal in animals) {
                    if (animal.Score < weakerScore) {
                        weakerScore = animal.Score;
                        weakerAnimal = animal;
                    }
                }

                int highestScore = animals [0].Score;
                Animal strongerAnimal = animals [0];

                foreach (var animal in animals) {
                    if (animal.Score > highestScore) {
                        highestScore = animal.Score;
                        strongerAnimal = animal;
                    }
                }

                weakerAnimal.AnimalType = strongerAnimal.AnimalType;
            }

        }

    }
}

