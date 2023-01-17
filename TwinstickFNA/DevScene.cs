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
        private const int PLAYER_SQUARE_SCALE = 100;

        public DevScene()
        {
            _position = new Vector2(100, 100);
            _velocity = new Vector2(0);
        }
        
        public void Update()
        {
            _velocity.X = InputManager.GetLeftStick().X * 5f;
            _velocity.Y += 1f;
            if (_velocity.Y >= 10f)
                _velocity.Y = 10f;
            if (InputManager.GetButtonPress(Buttons.A))
                _velocity.Y = -20f;
            _position += _velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameContent.Pixel, new Rectangle((int)(_position.X - PLAYER_SQUARE_SCALE / 2), (int)(_position.Y - PLAYER_SQUARE_SCALE / 2), PLAYER_SQUARE_SCALE, PLAYER_SQUARE_SCALE), Color.Black);
        }
    }
}
