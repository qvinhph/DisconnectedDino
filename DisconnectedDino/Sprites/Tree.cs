using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DisconnectedDino.Sprites
{
    public class Tree : Sprite
    {
        #region Fields

        static Random random = new Random();

        #endregion

        #region Properties



        #endregion

        #region Methods

        //public static Vector2 GetRandomPosition()
        //{
        //    var Position = new Vector2();
        //    Position.X = random.Ge
        //}

        #endregion

        public Tree()
        {

        }

        public Tree(Texture2D texture, Rectangle gameBoundaries) : base(texture, gameBoundaries)
        {
        }
    }
}
