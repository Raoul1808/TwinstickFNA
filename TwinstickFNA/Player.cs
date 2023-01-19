using System;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TwinstickFNA.ImGuiNet;
using TwinstickFNA.Input;

namespace TwinstickFNA
{
    public class Player
    {
        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _recoil;
        private Vector2 _targetSpeed;
        private Vector2 _aim;
        private bool _isJumping;
        private bool _canJump;
        private bool _canDoubleJump;
        private bool _justShot;
        private bool _onGround;

        private Rectangle Bounds =>
            new Rectangle(
                (int) (_position.X - GameVariables.TileScale / 2f),
                (int) (_position.Y - GameVariables.TileScale / 2f),
                GameVariables.TileScale,
                GameVariables.TileScale
            );

        public Player()
        {
            _position = new Vector2(100, 100);
            ImGuiManager.OnLayout += GuiLayout;
        }
        
        public void Update()
        {
            // Horizontal movement
            bool left = InputManager.GetButton(Buttons.LeftThumbstickLeft);
            bool right = InputManager.GetButton(Buttons.LeftThumbstickRight);
            if (left)
                _targetSpeed.X = -GameVariables.HorizontalSpeed;
            if (right)
                _targetSpeed.X = GameVariables.HorizontalSpeed;
            if (!left && !right)
                _targetSpeed.X = 0;

            // Horizontal Acceleration
            int direction = Math.Sign(_targetSpeed.X - _velocity.X);
            _velocity.X += GameVariables.HorizontalAcceleration * direction;
            if (Math.Sign(_targetSpeed.X - _velocity.X) != direction)
                _velocity.X = _targetSpeed.X;

            _velocity.Y += GameVariables.FallAcceleration;
            if (_velocity.Y > GameVariables.MaxFallSpeed)
                _velocity.Y = GameVariables.MaxFallSpeed;

            // Jump
            if (InputManager.GetButtonPress(Buttons.LeftTrigger) && (_canJump || _canDoubleJump))
            {
                _velocity.Y = -GameVariables.JumpForce;
                _isJumping = true;
                if (_canDoubleJump && !_canJump)
                {
                    _canDoubleJump = false;
                }
                if (_canJump)
                {
                    _canJump = false;
                }
            }
            
            // Jump checks
            if (_velocity.Y >= 0)
                _isJumping = false;
            if (_isJumping && !InputManager.GetButton(Buttons.LeftTrigger))
            {
                _isJumping = false; 
                _velocity.Y /= 2f;
            }
            
            // Aim
            var aim = InputManager.GetRightStick();
            if (aim.X != 0 && aim.Y != 0)
            {
                _aim = aim;
                _aim.Normalize();
                _aim.Y *= -1f;
            }

            if (_recoil != Vector2.Zero)
            {
                Vector2 recoilSign = new Vector2(Math.Sign(_recoil.X), Math.Sign(_recoil.Y));
                _recoil -= GameVariables.RecoilDissipation * recoilSign;
                if (Math.Sign(_recoil.X) != (int)recoilSign.X)
                    _recoil.X = 0;
                if (Math.Sign(_recoil.Y) != (int)recoilSign.Y)
                    _recoil.Y = 0;
            }
            
            // Shooting
            if (InputManager.GetButtonPress(Buttons.RightTrigger))
            {
                _recoil = -_aim * GameVariables.RecoilForce;
            }
            
            ResolveCollisions();
            _position += _velocity + _recoil;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameContent.Pixel, Bounds, Color.Blue);
            spriteBatch.DrawLine(_position, _position + _aim * GameVariables.AimLineLength, Color.Red, 2);
        }

        private void GuiLayout()
        {
            ImGui.Begin("Player");
            ImGui.Text("Pos X: " + _position.X);
            ImGui.Text("Pos Y: " + _position.Y);
            ImGui.Text("Vel X: " + _velocity.X);
            ImGui.Text("Vel Y: " + _velocity.Y);
            BoolYesNo("Is Jumping: ", _isJumping);
            BoolYesNo("Can Jump: ", _canJump);
            BoolYesNo("Can Double Jump: ", _canDoubleJump);
            BoolYesNo("Just Shot: ", _justShot);
            BoolYesNo("On Ground: ", _onGround);
            ImGui.End();
        }

        private void BoolYesNo(string text, bool b)
        {
            ImGui.Text(text + (b ? "Yes" : "No"));
        }

        private void OnCeilingBonk(int targetPos)
        {
            _position.Y = targetPos + GameVariables.TileScale / 2;
            _velocity.Y = 0f;
            _isJumping = false;
            _targetSpeed.Y = 0f;
            _recoil.Y = 0f;
        }

        private void OnLand(int targetPos)
        {
            _onGround = true;
            _position.Y = targetPos - GameVariables.TileScale / 2;
            _velocity.Y = 0f;
            _isJumping = false;
            _canJump = true;
            _canDoubleJump = true;
            _targetSpeed.Y = 0f;
            _recoil.Y = 0f;
        }

        private void ResolveCollisions()
        {
            Vector2 vel = _recoil + _velocity;
            foreach (Rectangle rect in DevScene.Platforms)
            {
                if (Bounds.Right + vel.X > rect.Left &&
                    Bounds.Left < rect.Left &&
                    Bounds.Bottom > rect.Top &&
                    Bounds.Top < rect.Bottom)
                {
                    // Touching left of platform
                    _position.X = rect.Left - GameVariables.TileScale / 2;
                    _velocity.X = 0f;
                    _targetSpeed.X = 0f;
                    _recoil.X = 0f;
                }
                
                if (Bounds.Left + vel.X < rect.Right &&
                    Bounds.Right > rect.Right &&
                    Bounds.Bottom > rect.Top &&
                    Bounds.Top < rect.Bottom)
                {
                    // Touching right of platform
                    _position.X = rect.Right + GameVariables.TileScale / 2;
                    _velocity.X = 0f;
                    _targetSpeed.X = 0f;
                    _recoil.X = 0f;
                }
                
                if (Bounds.Bottom + vel.Y > rect.Top &&
                    Bounds.Top < rect.Top &&
                    Bounds.Right > rect.Left &&
                    Bounds.Left < rect.Right)
                {
                    // Touching top of platform
                    OnLand(rect.Top);
                }
                
                if (Bounds.Top + vel.Y < rect.Bottom &&
                    Bounds.Bottom > rect.Bottom &
                    Bounds.Right > rect.Left &&
                    Bounds.Left < rect.Right)
                {
                    // Touching bottom of platform
                    OnCeilingBonk(rect.Bottom);
                }
            }
        }
    }
}
