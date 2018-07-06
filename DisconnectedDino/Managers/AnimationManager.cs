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
        private Animation _animation;

        private float _timer; /*To keep track of the time since the last frame is changed*/

        public Vector2 Position { get; set; }

        public AnimationManager(Animation animation)
        {
            _animation = animation;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_animation.Texture,
                             Position,
                             /*Get the single frame from sprite sheet*/
                             new Rectangle(_animation.CurrentFrame * _animation.Framewidth,
                                           0,
                                           _animation.FrameHeight,
                                           _animation.Framewidth),
                             Color.White);
        }

        public void Play(Animation animation)
        {
            //Keep playing the same animation
            if (_animation == animation)
                return;

            _animation = animation;

            _animation.CurrentFrame = 0;

            _timer = 0;
        }

        public void Stop()
        {
            _timer = 0f;
            _animation.CurrentFrame = 0;
        }

        public void Update(GameTime gameTime)
        {            
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0f;

                _animation.CurrentFrame++;

                if (_animation.CurrentFrame >= _animation.FrameCount)
                    _animation.CurrentFrame = 0;
            }
        }
    }
}
