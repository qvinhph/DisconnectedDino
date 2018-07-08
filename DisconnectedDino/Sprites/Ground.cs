using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DisconnectedDino.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DisconnectedDino.Sprites
{
    public class Ground : Sprite
    {
        #region Fields

        private int groundWidth;
        private int groundHeight;
        private int currentFrame;
        private int totalWidth;

        #endregion

        #region Properties

        //None

        #endregion

        #region Methods

        public Ground(Texture2D texture)
        {
            this.texture = texture;
            groundWidth = 1120;
            groundHeight = Rectangle.Height;
            totalWidth = groundWidth;
        }

        public override void Update(GameTime gameTime, List<Sprite> gameObjects)
        {
            if (totalWidth + (int)Speed > texture.Width)
            {
                currentFrame = 0;
                totalWidth = groundWidth;
            }
            else
            {
                totalWidth += (int)Speed;
                currentFrame++;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture,
                             Position,
                             new Rectangle((int)(currentFrame * Speed),
                                           0,
                                           groundWidth,
                                           groundHeight),
                             Color.White);
        }

        #endregion
    }
}
