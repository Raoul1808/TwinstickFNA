using ImGuiNET;
using Microsoft.Xna.Framework;

namespace TwinstickFNA
{
    public static class GameVariables
    {
        public const int TileScale = 32;
        public const int AimLineLength = 32;

        private static float _maxFallSpeed = 20f;
        private static float _fallAcceleration = 1.2f;
        private static float _horizontalSpeed = 10f;
        private static float _jumpForce = 15f;
        private static float _recoilForce = 20f;
        
        public static float MaxFallSpeed => _maxFallSpeed;
        public static float FallAcceleration => _fallAcceleration;
        public static float HorizontalSpeed => _horizontalSpeed;
        public static float JumpForce => _jumpForce;
        public static float RecoilForce => _recoilForce;

        public static void ImGuiLayout()
        {
            ImGui.Begin("Game Variables");
            if (ImGui.CollapsingHeader("Player Variables"))
            {
                ImGui.DragFloat("Max Fall Speed", ref _maxFallSpeed, 0.1f, 0.1f, TileScale);
                ImGui.DragFloat("Fall Acceleration", ref _fallAcceleration, 0.1f, 0.1f, TileScale);
                ImGui.DragFloat("Horizontal Speed", ref _horizontalSpeed, 0.1f, 0.1f, TileScale);
                ImGui.DragFloat("Jump Force", ref _jumpForce, 0.1f, 0.1f, TileScale);
                ImGui.DragFloat("Recoil Force", ref _recoilForce, 0.1f, 0.1f, TileScale);
            }
            ImGui.End();
        }
    }
}
