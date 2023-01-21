using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TwinstickFNA.Input;

namespace TwinstickFNA
{
    public class DevScene
    {

        public static readonly List<Rectangle> Platforms = new List<Rectangle>()
        {
            new Rectangle(50, 600, 1000, 75),
            new Rectangle(400, 300, 200, 50),
        };

        private Player _player;

        public DevScene()
        {
            _player = new Player();
        }
        
        public void Update()
        {
            _player.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Rectangle rect in Platforms)
                spriteBatch.Draw(GameContent.Pixel, rect, Color.Black);
            _player.Draw(spriteBatch);
        }

        public void HandleCollisions()
        {
        }
    }
}
