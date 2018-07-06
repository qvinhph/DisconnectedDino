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

        protected AnimationManager animationManager;

        //A dictionary help mapping the specified act with the according animation.
        protected Dictionary<string, Animation> animations;

        private float acceleration;

        private float gravity;

        private bool isJumping = false;

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

        #endregion

        public Dino(Texture2D texture) : base(texture)
        { 
        }

        public Dino(Dictionary<string, Animation> animations)
        {
            this.animations = animations;
            this.animationManager = new AnimationManager(animations.First().Value);
            this.gravity = 20f;
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

            animationManager.Update(gameTime);
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
            else
                animationManager.Play(animations["Run"]);
        }

        public override void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                var newPos = new Vector2(position.X -5f, position.Y);
                Position = newPos;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                var newPos = new Vector2(position.X + 5f, position.Y);
                Position = newPos;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && isJumping == false)
            {
                isJumping = true;
                acceleration = gravity;
            }
        }
    }
}
