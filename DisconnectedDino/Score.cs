using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisconnectedDino
{
    public class Score
    {
        public int Value;

        public Vector2 Position;

        private SpriteFont font;

        public Score(SpriteFont font)
        {
            this.font = font;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var stringToDraw = Value.ToString().PadLeft(8, '0');
            spriteBatch.DrawString(font, stringToDraw, Position, Color.DimGray);
        }

        /// <summary>
        /// Drawing score with text.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="text">The text appear before the score.</param>
        public void Draw(SpriteBatch spriteBatch, String text)
        {
            var stringNumber = Value.ToString().PadLeft(8, '0');
            var stringToDraw = text + stringNumber;
            spriteBatch.DrawString(font, stringToDraw, Position, Color.DimGray);
        }
    }
}
