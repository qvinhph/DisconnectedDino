using DisconnectedDino.Managers;
using DisconnectedDino.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisconnectedDino.Sprites
{
    public class FlyDino : Sprite
    {
        #region Fields

        private AnimationManager animationManager;

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

        public FlyDino(Animation animation)
        {
            this.animationManager = new AnimationManager(animation);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animationManager.Position = Position;

            animationManager.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            SetAnimation();
                      
            //To change the frame of animation
            animationManager.Update(gameTime);

            //To change the position of FlyDino
            base.Update(gameTime, sprites);
        }

        public void SetAnimation()
        {
            animationManager.Play();
        }

        #endregion

    }
}
