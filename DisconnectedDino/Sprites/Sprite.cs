using DisconnectedDino.Managers;
using DisconnectedDino.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisconnectedDino.Sprites
{
    public abstract class Sprite
    {
        #region Fields

        protected Texture2D texture;

        protected Vector2 position;

        #endregion

        #region Properties

        public Input Input;

        public float Speed = 3f;

        public Vector2 Velocity;

        public virtual Vector2 Position { get; set; }

        public virtual Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X,
                                     (int)position.Y,
                                     texture.Width, 
                                     texture.Height);
            }
        }

        #endregion

        #region Methods

        public Sprite() { }

        public Sprite(Texture2D texture)
        {
            this.texture = texture;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }

        public abstract void Move();

        public abstract void Update(GameTime gameTime, List<Sprite> sprites);

        

        #endregion
    }
}
