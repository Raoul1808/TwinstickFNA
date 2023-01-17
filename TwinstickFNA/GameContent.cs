using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwinstickFNA
{
    public static class GameContent
    {
        private static GraphicsDevice _graphicsDevice;

        public static Texture2D Pixel { get; private set; }

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            Pixel = new Texture2D(_graphicsDevice, 1, 1);
            Pixel.SetData(new[] {Color.White});
        }
    }
}
