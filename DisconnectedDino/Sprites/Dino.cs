using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DisconnectedDino.Managers;
using DisconnectedDino.Models;
using Microsoft.Xna.Framework;
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

        private float acceleration;

        private float gravity;

        private bool isJumping = false;

        private bool isCrouching = false;

        private bool hasChangedPositionForCrouching;

        #endregion

        #region Properties

        public override Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;

                //Set where to draw frames of animation
                animationManager.Position = position;
            }
        }

        //public override Rectangle Rectangle
        //{
        //    get
        //    {
        //        return 
        //    }
        //}

        #endregion

        #region Methods

        public Dino(Texture2D texture) : base(texture)
        {
        }

        public Dino(Dictionary<string, Animation> animations)
        {
            this.animations = animations;
            this.animationManager = new AnimationManager(animations.First().Value);
            this.gravity = 20f;
            this.Speed = 5;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animationManager.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Move();

            SetAnimation();

            ProcessJumping();

            ProcessCrouching();

            animationManager.Update(gameTime);
        }

        private void ProcessCrouching()
        {
            if (isCrouching)
            {
                if (!hasChangedPositionForCrouching)
                {
                    //Set the new position to draw
                    var newY = position.Y + (animationManager.PreFrameHeight - animationManager.FrameHeight);
                    var newPos = new Vector2(position.X, newY);
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
                var newPos = new Vector2(position.X, position.Y - acceleration);
                Position = newPos;
                acceleration -= 1;
            }

            if (position.Y == 400)
            {
                //When the dino fall onto the ground
                isJumping = false;
            }
        }

        private void SetAnimation()
        {
            if (isJumping)
                animationManager.Play(animations["Jump"]);
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
                var newPos = new Vector2(position.X - Speed, position.Y);
                Position = newPos;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                var newPos = new Vector2(position.X + Speed, position.Y);
                Position = newPos;
            }

            //Jump
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && isJumping == false)
            {
                isJumping = true;
                acceleration = gravity;
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
                    var newY = position.Y - (animationManager.PreFrameHeight - animationManager.FrameHeight);
                    var newPos = new Vector2(position.X, newY);
                    Position = newPos;
                }
            }
        }

        #endregion
    }
}
