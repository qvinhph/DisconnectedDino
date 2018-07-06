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
    public class Sprite
    {
        #region Fields

        protected AnimationManager _animationManager;

        protected Dictionary<string, Animation> _animations;

        protected Vector2 _position;

        protected Texture2D _texture;

        #endregion

        #region Properties

        public Input Input;

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;

                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }

        public float Speed = 1f;

        public Vector2 Velocity;

        #endregion

        #region Methods

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
        }

        public virtual void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Input.Up))
            {
                //Jumping
            }
            else if (Keyboard.GetState().IsKeyDown(Input.Down))
            {
                //Crouching or Go down fast
            }
        }

        public Sprite(Dictionary<string, Animation> animations)
        {
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);
        }

        public Sprite(Texture2D texture)
        {
            _texture = texture;
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Move();

            Position += Velocity;
            Velocity = Vector2.Zero;
        }  

        #endregion
    }
}
