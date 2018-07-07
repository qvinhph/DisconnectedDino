using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DisconnectedDino.Managers;
using DisconnectedDino.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DisconnectedDino.Sprites
{
    public class Dino : Sprite
    {
        #region Fields

        private AnimationManager animationManager;

        //A dictionary help mapping the specified act with the according animation.
        private Dictionary<string, Animation> animations;

        private Dictionary<string, SoundEffect> soundEffects;

        private float acceleration;

        private float gravity;

        private bool isJumping = false;

        private bool isCrouching = false;

        private bool hasChangedPositionForCrouching;

        private bool isAlive;

        #endregion

        #region Properties

        public override Vector2 Position
        {
            get { return animationManager.Position; }
            set
            {
                position = value;

                //Set where to draw frames of animation
                animationManager.Position = position;
            }
        }

        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X,
                                     (int)Position.Y,
                                     animationManager.FrameWidth,
                                     animationManager.FrameHeight);
            }
        }

        #endregion

        #region Methods

        public Dino(Dictionary<string, Animation> animations, 
            Dictionary<string, SoundEffect> soundEffects, Rectangle gameBoundaries)
        {
            this.animations = animations;
            this.animationManager = new AnimationManager(animations.First().Value);
            this.gravity = 20f;
            this.Speed = 5;
            this.gameBoundaries = gameBoundaries;
            this.soundEffects = soundEffects;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animationManager.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime, List<Sprite> gameObjects)
        {
            Move();

            isAlive = !CheckCollision(gameObjects);

            SetAnimation();

            ProcessJumping();

            ProcessCrouching();

            ProcessBoundaries();

            animationManager.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObjects"></param>
        /// <returns>True if has collisition. Otherwise false.</returns>
        public override bool CheckCollision(List<Sprite> gameObjects)
        {
            foreach (var sprite in gameObjects)
            {
                if (sprite is Dino || sprite is Cloud || sprite is Ground)
                    continue;

                if (this.Rectangle.Intersects(sprite.Rectangle))
                    return true;
            }

            return false;
        }

        private void SetAnimation()
        {
            if (!isAlive)
            {
                animationManager.Play(animations["Dead"]);
                soundEffects["DeadSound"].Play();
            }
            else if (isJumping)
            {
                animationManager.Play(animations["Jump"]);
            }
            else if (isCrouching)
                animationManager.Play(animations["Crouch"]);
            else
                animationManager.Play(animations["Run"]);
        }

        public override void Move()
        {
            //Run
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                var newPos = new Vector2(Position.X - Speed - 5, Position.Y);
                Position = newPos;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                var newPos = new Vector2(Position.X + Speed, Position.Y);
                Position = newPos;
            }

            //Jump
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && isJumping == false)
            {
                isJumping = true;
                acceleration = gravity;

                var instance = soundEffects["JumpSound"].CreateInstance();
                instance.Play();
            }

            //Crouch
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if (!isCrouching && !isJumping)
                {
                    isCrouching = true;
                }
            }

            //Remove crouching
            if (isCrouching)
            {
                if (Keyboard.GetState().IsKeyUp(Keys.Down))
                {
                    isCrouching = false;
                    hasChangedPositionForCrouching = false;

                    //Return the default position
                    //Because the Dino hasn't be set to Running yet 
                    //So the PreFrameHeight is still the height of Running Dino, not the Crouching Dino
                    var newY = Position.Y - (animationManager.PreFrameHeight - animationManager.FrameHeight);
                    var newPos = new Vector2(Position.X, newY);
                    Position = newPos;
                }
            }
        }

        /// <summary>
        /// Keep the Dino in the screen
        /// </summary>
        private void ProcessBoundaries()
        {
            var validPositionX = MathHelper.Clamp(Position.X, 0, gameBoundaries.Right - animationManager.FrameWidth);
            var newPos = new Vector2(validPositionX, Position.Y);

            Position = newPos;
        }

        private void ProcessCrouching()
        {
            if (isCrouching)
            {
                if (!hasChangedPositionForCrouching)
                {
                    //Set the new position to draw
                    var newY = Position.Y + (animationManager.PreFrameHeight - animationManager.FrameHeight);
                    var newPos = new Vector2(Position.X, newY);
                    Position = newPos;
                    hasChangedPositionForCrouching = true;
                }                
            }
        }

        private void ProcessJumping()
        {
            if (isJumping == true)
            {
                //This will process when the Dino is on air.
                var newPos = new Vector2(Position.X, Position.Y - acceleration);
                Position = newPos;
                acceleration -= 1;
            }

            if (Position.Y == 400)
            {
                //When the dino reach the ground
                isJumping = false;
            }
        }

        #endregion
    }
}
