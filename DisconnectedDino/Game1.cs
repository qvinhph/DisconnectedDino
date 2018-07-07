using DisconnectedDino.Models;
using DisconnectedDino.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace DisconnectedDino
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle gameBoundaries;

        /// <summary>
        /// Index from 0 - 3 is low tree, the rest is high one.
        /// </summary>
        private List<Texture2D> treeTextures;

        private List<Sprite> gameObjects;

        private Ground ground;

        private Random random;

        private int spaceToAddTree;

        private int spaceFromTheLastTree;

        private Tree lastTree;

        private float remainingTimeToAddNewTree;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 600;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            random = new Random();

            remainingTimeToAddNewTree = 1f;

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Get the game boundaries
            gameBoundaries = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            //Load dictionary of all the animations of Dino
            var dinoAnimations = new Dictionary<string, Animation>()
            {
                {"Sleep", new Animation(Content.Load<Texture2D>("Player1/SleepingDino"), 1) },
                {"Run", new Animation(Content.Load<Texture2D>("Player1/RunningDino"), 2) },
                {"Jump", new Animation(Content.Load<Texture2D>("Player1/JumpingDino"), 1) },
                {"Crouch", new Animation(Content.Load<Texture2D>("Player1/CrouchingDino"), 2) },
            };

            //Load tree textures
            treeTextures = new List<Texture2D>()
            {
                Content.Load<Texture2D>("Obstacles/Trees/1LowTree"),
                Content.Load<Texture2D>("Obstacles/Trees/2LowTrees"),
                Content.Load<Texture2D>("Obstacles/Trees/3LowTrees"),
                Content.Load<Texture2D>("Obstacles/Trees/4LowTrees"),
                Content.Load<Texture2D>("Obstacles/Trees/1TallTree"),
                Content.Load<Texture2D>("Obstacles/Trees/2TallTrees"),
                Content.Load<Texture2D>("Obstacles/Trees/3TallTrees"),
                Content.Load<Texture2D>("Obstacles/Trees/4TallTrees"),
            };

            //Load game objects
            gameObjects = new List<Sprite>()
            {
                //Create Dino
                new Dino(dinoAnimations, gameBoundaries)
                {
                    Input = new Input()
                    {
                        Left = Keys.Left,
                        Right = Keys.Right,
                        Down = Keys.Down,
                        Up = Keys.Up
                    },

                    Position = new Vector2(80, 400),
                },
            };

            //Load ground
            ground = new Ground(Content.Load<Texture2D>("Background/Ground"))
            {
                Position = new Vector2(40, 475)
            };
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {            
            //Update the ground
            ground.Update(gameTime, gameObjects);

            //Update the trees - add the new tree and remove the passed tree
            if (onTimeToAddTree(gameTime))
            {
                lastTree = GetRandomTreeTexture(treeTextures);
                spaceFromTheLastTree = 0;
                gameObjects.Add(lastTree);
            }            

            //Update the game objects
            foreach (var sprite in gameObjects)
                sprite.Update(gameTime, gameObjects);

            base.Update(gameTime);
        }

        private bool onTimeToAddTree(GameTime gameTime)
        {
            bool result;

            if (remainingTimeToAddNewTree <= 0)
            {
                //Random time to add new tree
                remainingTimeToAddNewTree = GetRandomFloat(0.4f, 2.5f);
                result = true;
            }
            else
            {
                remainingTimeToAddNewTree -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                result = false;
            }

            return result;
        }

        private Tree GetRandomTreeTexture(List<Texture2D> treeTextures)
        {
            var index = random.Next(0, 7);
            var position = new Vector2();
            
            if (index <= 3)
            {
                //Is low tree
                position = new Vector2(gameBoundaries.Width, 422);
            }
            else
            {
                //Is high tree
                position = new Vector2(gameBoundaries.Width, 400);
            }

            var tree = new Tree(treeTextures[index], gameBoundaries)
            {
                Position = position
            };

            return tree;
        }

        private float GetRandomFloat(float min, float max)
        {
            var floatValue1 = random.NextDouble(); // Can be from 0.0 to 1.0
            var floatValue2 = random.NextDouble(); // Can be from 0.0 to 1.0

            var randomInteger = random.Next((int)min, (int)max);

            var randomFloat = randomInteger + floatValue1 * floatValue2;

            if (randomFloat > max)
                randomFloat = max;
            else if (randomFloat < min)
                randomFloat = min;

            return (float)randomFloat;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            //Draw the ground
            ground.Draw(spriteBatch);

            //Draw the game objects
            foreach (var sprite in gameObjects)
            {
                sprite.Draw(spriteBatch);
            }

            spriteBatch.End();            

            base.Draw(gameTime);
        }
    }
}
