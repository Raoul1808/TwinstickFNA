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
        private Vector2 _aim;
        private bool _isJumping;
        private bool _canJump;
        private bool _canDoubleJump;
        private bool _justShot;

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
            _justShot = false;
            Vector2 aim = InputManager.GetRightStick();
            if (aim.X != 0 && aim.Y != 0)
            {
                aim.Y *= -1f;
                _aim = aim;
                _aim.Normalize();
            }
            _velocity.X = InputManager.GetLeftStick().X * GameVariables.HorizontalSpeed;
            _velocity.Y += GameVariables.FallAcceleration;
            if (_velocity.Y >= GameVariables.MaxFallSpeed)
                _velocity.Y = GameVariables.MaxFallSpeed;
            if (InputManager.GetButtonPress(Buttons.LeftTrigger) && (_canJump || _canDoubleJump))
            {
                _velocity.Y = -GameVariables.JumpForce;
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
                _justShot = true;
                _velocity += _aim * -1f * GameVariables.RecoilForce;
            }
            
            if (_velocity.Y >= 0f)
                _isJumping = false;

            if (_isJumping && !InputManager.GetButton(Buttons.LeftTrigger))
            {
                _velocity.Y /= 2f;
                _isJumping = false;
            }
            
            ResolveCollisions();

            _position += _velocity;
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
        }

        private void OnLand(int targetPos)
        {
            _position.Y = targetPos - GameVariables.TileScale / 2;
            _velocity.Y = 0f;
            _isJumping = false;
            _canJump = true;
            _canDoubleJump = true;
        }

        private void ResolveCollisions()
        {
            foreach (Rectangle rect in DevScene.Platforms)
            {
                if (Bounds.Right + _velocity.X > rect.Left &&
                    Bounds.Left < rect.Left &&
                    Bounds.Bottom > rect.Top &&
                    Bounds.Top < rect.Bottom)
                {
                    // Touching left of platform
                    _position.X = rect.Left - GameVariables.TileScale / 2;
                    _velocity.X = 0f;
                }
                
                if (Bounds.Left + _velocity.X < rect.Right &&
                    Bounds.Right > rect.Right &&
                    Bounds.Bottom > rect.Top &&
                    Bounds.Top < rect.Bottom)
                {
                    // Touching right of platform
                    _position.X = rect.Right + GameVariables.TileScale / 2;
                    _velocity.X = 0f;
                }
                
                if (Bounds.Bottom + _velocity.Y > rect.Top &&
                    Bounds.Top < rect.Top &&
                    Bounds.Right > rect.Left &&
                    Bounds.Left < rect.Right)
                {
                    // Touching top of platform
                    OnLand(rect.Top);
                }
                
                if (Bounds.Top + _velocity.Y < rect.Bottom &&
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
