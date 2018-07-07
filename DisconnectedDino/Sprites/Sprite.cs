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

        protected Rectangle gameBoundaries;

        #endregion

        #region Properties

        public Input Input;

        public float Speed;

        public Vector2 Velocity;

        public virtual Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

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

        public Sprite(Texture2D texture, Rectangle gameBoundaries)
        {
            this.texture = texture;
            this.gameBoundaries = gameBoundaries;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }

        public virtual void Move()
        {
            Velocity.X -= Speed;
        }

        public virtual void Update(GameTime gameTime, List<Sprite> gameObjects)
        {
            Move();
                        
            position.X += Velocity.X;

            Velocity = Vector2.Zero;
        }

        public virtual bool CheckCollision(List<Sprite> gameObjects)
        {
            return false;
        }

        #endregion
    }
}
