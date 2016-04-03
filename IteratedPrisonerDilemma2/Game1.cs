#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

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

        //private Animal animal;
        //private Animal animal2;
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
                        if (animals.ElementAt (i).CollideWith (animals.ElementAt (j))) {
                            var game = new PrisonerDilemmaSequenceOfIterations(Constants.NUM_OF_ITERATIONS_PER_GAME,animals.ElementAt(i),animals.ElementAt(j));
                            games.Add (game);
                        }
                    }
                }
            }


            // elaborate all current games:


            foreach (var game in games) {
                int[] scores = game.CumulateGamingScores ();
                game.Animal1.AddScore (scores [0]);
                game.Animal2.AddScore (scores [1]);
            }
            games = new List<PrisonerDilemmaSequenceOfIterations> ();

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
                spriteBatch.DrawString (spriteFont, animalType.AnimalTypeName+"  "+animalType.Score, new Vector2 (Constants.PANEL_INIT_X_POSITION + 4, Constants.PANEL_INIT_Y_POSITION + index), Color.Black);
                index += 40;
            }


            spriteBatch.End ();
            
            base.Draw (gameTime);

        }
    }
}

