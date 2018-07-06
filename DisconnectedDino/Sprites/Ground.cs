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
        //private int groundHeight = ;
        
        #endregion

        #region Properties

        //None

        #endregion

        #region Methods

        public Ground(Texture2D texture) : base(texture)
        {
        }

        public override void Move()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
