using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TwinstickFNA.ImGuiNet;
using TwinstickFNA.Input;

namespace TwinstickFNA
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private DevScene _scene;

        public const int ScreenWidth = 1280;
        public const int ScreenHeight = 720;
        
        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            ImGuiManager.Initialize(this);
            ImGuiManager.OnLayout += GameVariables.ImGuiLayout;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GameContent.Initialize(GraphicsDevice);
            _scene = new DevScene();
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            if (InputManager.GetButtonPress(Buttons.Start))
                Exit();
            _scene.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _scene.Draw(_spriteBatch);
            _spriteBatch.End();

            ImGuiManager.Layout(gameTime);
            
            base.Draw(gameTime);
        }
    }
}
