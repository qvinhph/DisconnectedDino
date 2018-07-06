using DisconnectedDino.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisconnectedDino.Managers
{
    /// <summary>
    /// Represent the object helping keep track of one animation of a time.
    /// </summary>
    public class AnimationManager
    {
        private Animation animation;

        private float timer; /*To keep track of the time since the last frame is changed*/

        public Vector2 Position { get; set; }

        public int FrameHeight { get { return animation.FrameHeight; } }

        public int FrameWidth { get { return animation.FrameWidth; } }

        public AnimationManager() { }

        public AnimationManager(Animation animation)
        {
            this.animation = animation;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(animation.Texture,
                             Position,
                             /*Get the single frame from sprite sheet*/
                             new Rectangle(animation.CurrentFrame * animation.FrameWidth,
                                           0,
                                           animation.FrameWidth,
                                           animation.FrameHeight),
                             Color.White);                        
        }

        public void Play(Animation animation)
        {
            this.animation.IsLooping = true;

            //Keep playing the same animation
            if (this.animation == animation)
                return;

            //Start the animation
            this.animation = animation;

            this.animation.CurrentFrame = 0;

            timer = 0;
        }

        public void Stop()
        {
            this.animation.IsLooping = false;

            this.timer = 0f;

            this.animation.CurrentFrame = 0;
        }

        public void Update(GameTime gameTime)
        {    
            if (animation.IsLooping == true)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (timer > animation.FrameSpeed)
                {
                    timer = 0f;

                    animation.CurrentFrame++;

                    if (animation.CurrentFrame >= animation.FrameCount)
                        animation.CurrentFrame = 0;
                }
            }            
        }
    }
}
