// Автозагружаемый CanvasLayer с меткой версии сборки в левом нижнем углу.
// Версия читается из project.godot → application/config/version.
// Layer 100 гарантирует отображение поверх любого игрового контента.

using Godot;

namespace DayByDaySim.UI
{
    public partial class VersionLabel : CanvasLayer
    {
        public override void _Ready()
        {
            Layer = 100;

            var version = ProjectSettings.GetSetting("application/config/version", "?.?.?").AsString();

            var label = new Label
            {
                Text         = $"v{version}",
                AnchorLeft   = 0f,
                AnchorTop    = 1f,
                AnchorRight  = 0f,
                AnchorBottom = 1f,
                OffsetLeft   = 8f,
                OffsetTop    = -28f,
                OffsetRight  = 150f,
                OffsetBottom = -8f,
            };

            // Ненавязчивый цвет — белый с низкой прозрачностью
            label.AddThemeColorOverride("font_color", new Color(1f, 1f, 1f, 0.35f));

            AddChild(label);
        }
    }
}
