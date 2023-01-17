using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TwinstickFNA.Input
{
    public static class InputManager
    {
        private static MouseState _oldMouse, _mouse;
        private static GamePadState _oldGamePad, _gamePad;

        static InputManager()
        {
            _oldMouse = default;
            _mouse = default;
            _oldGamePad = default;
            _gamePad = default;
        }

        public static void Update()
        {
            _oldMouse = _mouse;
            _oldGamePad = _gamePad;
            _mouse = Mouse.GetState();
            _gamePad = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);
        }

        public static bool GetMouseLeft() => _mouse.LeftButton == ButtonState.Pressed;
        public static bool GetMouseLeftClick() => _mouse.LeftButton == ButtonState.Pressed && _oldMouse.LeftButton == ButtonState.Released;
        public static Point GetMousePos() => new Point(_mouse.X, _mouse.Y);

        public static bool GetButton(Buttons button) => _gamePad.IsButtonDown(button);
        public static bool GetButtonPress(Buttons button) => _gamePad.IsButtonDown(button) && _oldGamePad.IsButtonUp(button);
        public static Vector2 GetLeftStick() => _gamePad.ThumbSticks.Left;
        public static Vector2 GetRightStick() => _gamePad.ThumbSticks.Right;
    }
}
