using System;
using Microsoft.Xna.Framework;

namespace TwinstickFNA.ImGuiNet
{
    public static class ImGuiManager
    {
        private static ImGuiRenderer _imGuiRenderer;
        public static event Action OnLayout;

        public static void Initialize(Game game)
        {
            _imGuiRenderer = new ImGuiRenderer(game);
            _imGuiRenderer.RebuildFontAtlas();
        }

        public static void Layout(GameTime gameTime)
        {
            _imGuiRenderer.BeforeLayout(gameTime);
            OnLayout?.Invoke();
            _imGuiRenderer.AfterLayout();
        }
    }
}
