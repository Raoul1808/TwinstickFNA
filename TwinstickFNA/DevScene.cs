using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TwinstickFNA.Input;

namespace TwinstickFNA
{
    public class DevScene
    {
        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _aim;
        private bool _isJumping = false;
        private bool _canJump = false;
        private bool _canDoubleJump = false;

        private List<Rectangle> _platforms = new List<Rectangle>()
        {
            new Rectangle(50, 600, 1000, 75),
            new Rectangle(400, 300, 200, 50),
        };

        private const int TileScale = 32;
        private const int AimLineLength = 32;

        private const float MaxFallSpeed = 20f;
        private const float GravityConstant = 1.2f;
        private const float HorizontalSpeed = 10f;
        private const float JumpForce = -15f;
        private const float RecoilForce = 20f;

        private Rectangle _bounds =>
            new Rectangle(
                (int) (_position.X - TileScale / 2f),
                (int) (_position.Y - TileScale / 2f),
                TileScale,
                TileScale
            );

        public DevScene()
        {
            _position = new Vector2(TileScale * 3, TileScale);
            _velocity = new Vector2(0);
            _aim = new Vector2(1f, 0f);
        }
        
        public void Update()
        {
            Vector2 aim = InputManager.GetRightStick();
            if (aim.X != 0 && aim.Y != 0)
            {
                aim.Y *= -1f;
                _aim = aim;
                _aim.Normalize();
            }
            _velocity.X = InputManager.GetLeftStick().X * HorizontalSpeed;
            _velocity.Y += GravityConstant;
            if (_velocity.Y >= MaxFallSpeed)
                _velocity.Y = MaxFallSpeed;
            if (InputManager.GetButtonPress(Buttons.LeftTrigger) && (_canJump || _canDoubleJump))
            {
                _velocity.Y = JumpForce;
                _isJumping = true;
                if (!_canJump && _canDoubleJump)
                {
                    _canDoubleJump = false;
                }
                if (_canJump)
                {
                    _canJump = false;
                }
            }

            if (InputManager.GetButtonPress(Buttons.RightTrigger))
            {
                _velocity += _aim * -1f * RecoilForce;
            }
            
            if (_velocity.Y >= 0f)
                _isJumping = false;

            if (_isJumping && !InputManager.GetButton(Buttons.LeftTrigger))
            {
                _velocity.Y /= 2f;
                _isJumping = false;
            }

            HandleCollisions();
            _position += _velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameContent.Pixel, _bounds, Color.Blue);
            foreach (Rectangle rect in _platforms)
                spriteBatch.Draw(GameContent.Pixel, rect, Color.Black);
            spriteBatch.DrawLine(_position, _position + _aim * AimLineLength, Color.Red, 2);
        }

        public void HandleCollisions()
        {
            foreach (Rectangle rect in _platforms)
            {
                if (_bounds.Right + _velocity.X > rect.Left &&
                    _bounds.Left < rect.Left &&
                    _bounds.Bottom > rect.Top &&
                    _bounds.Top < rect.Bottom)
                {
                    // Touching left of platform
                    _position.X = rect.Left - TileScale / 2;
                    _velocity.X = 0f;
                }
                
                if (_bounds.Left + _velocity.X < rect.Right &&
                    _bounds.Right > rect.Right &&
                    _bounds.Bottom > rect.Top &&
                    _bounds.Top < rect.Bottom)
                {
                    // Touching right of platform
                    _position.X = rect.Right + TileScale / 2;
                    _velocity.X = 0f;
                }
                
                if (_bounds.Bottom + _velocity.Y > rect.Top &&
                    _bounds.Top < rect.Top &&
                    _bounds.Right > rect.Left &&
                    _bounds.Left < rect.Right)
                {
                    // Touching top of platform
                    _position.Y = rect.Top - TileScale / 2;
                    _velocity.Y = 0f;
                    _isJumping = false;
                    _canJump = true;
                    _canDoubleJump = true;
                }
                
                if (_bounds.Top + _velocity.Y < rect.Bottom &&
                    _bounds.Bottom > rect.Bottom &
                    _bounds.Right > rect.Left &&
                    _bounds.Left < rect.Right)
                {
                    // Touching bottom of platform
                    _position.Y = rect.Bottom + TileScale / 2;
                    _velocity.Y = 0f;
                    _isJumping = false;
                }
            }
        }
    }
}
