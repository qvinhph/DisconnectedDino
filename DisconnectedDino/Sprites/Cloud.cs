using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisconnectedDino.Sprites
{
    public class Cloud : Sprite
    {
        #region Fields

        public new float Speed
        {
            get { return base.Speed - 4f; }
            set
            {
                base.Speed = value - 4f;
            }
        }

        #endregion

        #region Properties

        //None

        #endregion

        #region Methods

        public Cloud(Texture2D texture) : base(texture)
        {
        }

        #endregion
    }
}
