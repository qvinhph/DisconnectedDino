using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisconnectedDino
{
    public class StringText
    {
        public string Text;

        public Vector2 Position;

        private SpriteFont font;

        public StringText(SpriteFont font)
        {
            this.font = font;
        }

        /// <summary>
        /// Drawing text.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="text"></param>
        public void Draw(SpriteBatch spriteBatch, String text)
        {
            spriteBatch.DrawString(font, text, Position, Color.DimGray);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, Text, Position, Color.DimGray);
        }

        public void Draw(SpriteBatch spriteBatch, float scale)
        {
            spriteBatch.DrawString(font, Text, Position, Color.DimGray, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}
