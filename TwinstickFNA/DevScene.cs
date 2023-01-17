using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwinstickFNA.Input;

namespace TwinstickFNA
{
    public class DevScene
    {
        private Vector2 _position;
        private const int PLAYER_SQUARE_SCALE = 100;

        public DevScene()
        {
            _position = new Vector2(100, 100);
        }
        
        public void Update()
        {
            var vel = InputManager.GetLeftStick() * 2;
            vel.Y *= -1;
            _position += vel;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameContent.Pixel, new Rectangle((int)(_position.X - PLAYER_SQUARE_SCALE / 2), (int)(_position.Y - PLAYER_SQUARE_SCALE / 2), PLAYER_SQUARE_SCALE, PLAYER_SQUARE_SCALE), Color.Black);
        }
    }
}
