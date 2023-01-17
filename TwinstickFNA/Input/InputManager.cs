using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TwinstickFNA.Input
{
    public static class InputManager
    {
        private static GamePadState _oldGamePad, _gamePad;

        static InputManager()
        {
            _oldGamePad = default;
            _gamePad = default;
        }

        public static void Update()
        {
            _oldGamePad = _gamePad;
            _gamePad = GamePad.GetState(PlayerIndex.One);
        }

        public static bool GetButton(Buttons button) => _gamePad.IsButtonDown(button);
        public static bool GetButtonPress(Buttons button) => _gamePad.IsButtonDown(button) && _oldGamePad.IsButtonUp(button);
        public static Vector2 GetLeftStick() => _gamePad.ThumbSticks.Left;
        public static Vector2 GetRightStick() => _gamePad.ThumbSticks.Right;
    }
}
