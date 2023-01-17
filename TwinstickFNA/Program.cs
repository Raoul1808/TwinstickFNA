using Microsoft.Xna.Framework;

namespace TwinstickFNA
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new MainGame())
                game.Run();
        }
    }
}
