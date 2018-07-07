using DisconnectedDino.Models;
using DisconnectedDino.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace DisconnectedDino
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        #region Fields

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle gameBoundaries;

        /// <summary>
        /// Index from 0 - 3 is low tree, the rest is high one.
        /// </summary>
        private List<Texture2D> treeTextures;

        private List<Sprite> gameObjects;

        private List<Vector2> flyDinoPositions;

        private List<Vector2> cloudPositions;

        private Animation flyDinoAnimation;

        private Random random;

        private int spaceFromTheLastTree;

        private int rightSpaceToAddTree;

        private float remainingTimeToAddFlyDino;

        private float remainingTimeToAddCloud;

        private bool gameOver;

        private float timer;

        private float timeSpan;

        private float gameSpeed;

        private Score currentScore;

        private Score highScore;

        private StringText gameOverText;

        private StringText instructionText;

        private Song backgroundSong;

        private Dictionary<string, Animation> dinoAnimations;

        private Dictionary<string, SoundEffect> dinoSoundEffects;

        #endregion

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
            //Load background music
            //backgroundMusic = Content.Load<Song>("SuperMarioBros");
            backgroundSong = Content.Load<Song>("Sound/Song");
            MediaPlayer.Play(backgroundSong);

            //Load font
            currentScore = new Score(Content.Load<SpriteFont>("FontScore"))
            {
                Position = new Vector2(900, 55),
                Value = 0
            };

            highScore = new Score(Content.Load<SpriteFont>("FontScore"))
            {
                Position = new Vector2(900, 20),
                Value = 0
            };

            gameOverText = new StringText(Content.Load<SpriteFont>("FontText"))
            {
                Text = "G A M E     O V E R",
                Position = new Vector2(150, 160),
            };

            instructionText = new StringText(Content.Load<SpriteFont>("FontText"))
            {
                Text = "- enter to retry -",
                Position = new Vector2(380, 280),
            };

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            random = new Random();
            timer = 0;
            timeSpan = 20;
            gameSpeed = 10;
            spaceFromTheLastTree = 0;
            rightSpaceToAddTree = 1000;

            //Initial the remaining time to add fly dino
            remainingTimeToAddFlyDino = 20f;

            //Initial the remaining time to add cloud
            remainingTimeToAddCloud = 0f;

            //Get the game boundaries
            gameBoundaries = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            //Load the animation of Fly Dino
            flyDinoAnimation = new Animation(Content.Load<Texture2D>("Obstacles/FlyDino/FlyDino"), 2);

            //Create list to choose randomly from to set for FlyDino
            flyDinoPositions = new List<Vector2>
            {
                new Vector2(gameBoundaries.Width, 150),
                new Vector2(gameBoundaries.Width, 380),
                new Vector2(gameBoundaries.Width, 360),
            };

            //Create list to choose randomly from to set for Cloud
            cloudPositions = new List<Vector2>
            {
                new Vector2(gameBoundaries.Width, 50),
                new Vector2(gameBoundaries.Width, 100),
                new Vector2(gameBoundaries.Width, 160),
                new Vector2(gameBoundaries.Width, 210),
                new Vector2(gameBoundaries.Width, 260),
            };

            //Load dictionary of all the animations of Dino
            dinoAnimations = new Dictionary<string, Animation>()
            {
                {"Sleep", new Animation(Content.Load<Texture2D>("Player1/SleepingDino"), 1) },
                {"Run", new Animation(Content.Load<Texture2D>("Player1/RunningDino"), 2) },
                {"Jump", new Animation(Content.Load<Texture2D>("Player1/JumpingDino"), 1) },
                {"Crouch", new Animation(Content.Load<Texture2D>("Player1/CrouchingDino"), 2) },
                {"Dead", new Animation(Content.Load<Texture2D>("Player1/DeadDino"), 1) },
            };

            dinoSoundEffects = new Dictionary<string, SoundEffect>()
            {
                {"JumpSound", Content.Load<SoundEffect>("Sound/JumpSound")},
                {"DeadSound", Content.Load<SoundEffect>("Sound/DeadSound")},
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
                new Dino(dinoAnimations, dinoSoundEffects, gameBoundaries)
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

                new Ground(Content.Load<Texture2D>("Background/Ground"))
                {
                    Position = new Vector2(40, 475)
                },
            };
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            //None
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (gameOver && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Restart();
                return;
            }
            else if (gameOver)
                return;

            //Increase speed time by time
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > timeSpan && gameSpeed < 15)
            {
                gameSpeed += 2;
                timeSpan = timer * 2;
            }

            //Add the cloud
            if (onTimeToAddCloud(gameTime))
            {
                var cloud = new Cloud(Content.Load<Texture2D>("Background/Cloud"))
                {
                    Position = GetRandomCloudPosition()
                };

                gameObjects.Add(cloud);
            }

            //Add the new tree
            if (onTimeToAddTree(gameTime))
            {
                var tree = GetRandomTreeTexture(treeTextures);

                gameObjects.Add(tree);
            }

            //Add the new flydino
            if (onTimeToAddFlyDino(gameTime))
            {
                var flyDino = new FlyDino(flyDinoAnimation)
                {
                    Position = GetRandomFlyDinoPosition(),
                };

                gameObjects.Add(flyDino);
            }            

            //Update the game objects
            for (int i = 0; i < gameObjects.Count; i++)
            {
                //
                //Update the game speed
                //
                if (gameObjects[i] is FlyDino)
                {
                    /*The FlyDino will be faster.*/
                    gameObjects[i].Speed = gameSpeed + gameSpeed * 0.4f;
                }
                else if (gameObjects[i] is Cloud)
                {
                    /*The Cloud will be slower than everything. It make sense in real life :) */
                    gameObjects[i].Speed = gameSpeed - gameSpeed * 0.3f;
                }
                else
                    gameObjects[i].Speed = gameSpeed;

                gameObjects[i].Update(gameTime, gameObjects);


                //
                //Remove the out-screen object
                //
                if (gameObjects[i] is Tree || gameObjects[i] is FlyDino || gameObjects[i] is Cloud)
                {
                    if (gameObjects[i].Position.X + gameObjects[i].Rectangle.Width <= 0)
                    {
                        gameObjects.RemoveAt(i);
                        i--;
                    }
                }

                //
                //Check collision
                //
                if (gameObjects[i] is Dino)
                    if (gameObjects[i].CheckCollision(gameObjects))
                    {
                        gameOver = true;
                        MediaPlayer.Stop();
                    }
            }

            //
            // Calculate scrore
            //
            currentScore.Value = (int)timer;
            if (currentScore.Value > highScore.Value)
            {
                highScore.Value = currentScore.Value;
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            
            //Draw the game objects
            foreach (var sprite in gameObjects)
            {
                sprite.Draw(spriteBatch);
            }

            highScore.Draw(spriteBatch, "BEST   ");
            currentScore.Draw(spriteBatch, "    ME   ");

            if (gameOver)
            {
                gameOverText.Draw(spriteBatch);
                instructionText.Draw(spriteBatch, 0.5f);
            }
                
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region Essential methods

        private void Restart()
        {
            gameObjects.Clear();

            //Re-create gameObjects
            gameObjects = new List<Sprite>()
            {
                //Create Dino
                new Dino(dinoAnimations, dinoSoundEffects, gameBoundaries)
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

                new Ground(Content.Load<Texture2D>("Background/Ground"))
                {
                    Position = new Vector2(40, 475)
                },
            };

            //Default initial value
            timer = 0;
            timeSpan = 20;
            gameSpeed = 10;
            currentScore.Value = 0;
            remainingTimeToAddCloud = 0f;
            remainingTimeToAddFlyDino = 20f;
            spaceFromTheLastTree = 0;
            rightSpaceToAddTree = 1000;
            gameOver = false;
            MediaPlayer.Play(backgroundSong);
        }

        private bool onTimeToAddFlyDino(GameTime gameTime)
        {
            bool result;

            if (remainingTimeToAddFlyDino <= 0)
            {
                remainingTimeToAddFlyDino = GetRandomFloat(6, 12);
                result = true;
            }
            else
            {
                //Countdown
                remainingTimeToAddFlyDino -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                result = false;
            }

            return result;
        }

        private bool onTimeToAddTree(GameTime gameTime)
        {
            bool result;

            if (rightSpaceToAddTree <= spaceFromTheLastTree)
            {
                //Random new right space to add new tree
                rightSpaceToAddTree = random.Next(400, 1100);
                spaceFromTheLastTree = 0;
                result = true;
            }
            else
            {
                //spaceFromTheLastTree = (int)(gameBoundaries.Width - lastTree.Position.X);
                spaceFromTheLastTree += (int)gameSpeed;
                result = false;
            }

            return result;
        }

        private bool onTimeToAddCloud(GameTime gameTime)
        {
            bool result;

            if (remainingTimeToAddCloud <= 0)
            {
                remainingTimeToAddCloud = GetRandomFloat(0.5f, 2f);
                result = true;
            }
            else
            {
                //Countdown
                remainingTimeToAddCloud -= (float)gameTime.ElapsedGameTime.TotalSeconds;
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

            var tree = new Tree(treeTextures[index])
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

        private Vector2 GetRandomFlyDinoPosition()
        {
            int index = random.Next(0, flyDinoPositions.Count - 1);
            return flyDinoPositions[index];
        }

        private Vector2 GetRandomCloudPosition()
        {
            int index = random.Next(0, cloudPositions.Count - 1);
            return cloudPositions[index];
        }

        #endregion
    }
}
