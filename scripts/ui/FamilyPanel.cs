// Панель семьи внизу экрана: горизонтальный ряд карточек живых персонажей.
// Autoload CanvasLayer, перестраивается при смерти или добавлении члена семьи.

using Godot;
using DayByDaySim.Gameplay;

namespace DayByDaySim.UI
{
    public partial class FamilyPanel : CanvasLayer
    {
        private HBoxContainer _cards;

        public override void _Ready()
        {
            Layer = 10;

            // Полупрозрачный фон во всю ширину нижней части экрана
            var bg = new PanelContainer();
            bg.AddThemeStyleboxOverride("panel", new StyleBoxFlat
            {
                BgColor             = new Color(0f, 0f, 0f, 0.55f),
                CornerRadiusTopLeft  = 6,
                CornerRadiusTopRight = 6
            });
            bg.AnchorLeft   = 0f;
            bg.AnchorRight  = 1f;
            bg.AnchorTop    = 1f;
            bg.AnchorBottom = 1f;
            bg.OffsetTop    = -148f;
            bg.OffsetBottom = 0f;
            AddChild(bg);

            var margin = new MarginContainer();
            margin.AddThemeConstantOverride("margin_left",   8);
            margin.AddThemeConstantOverride("margin_right",  8);
            margin.AddThemeConstantOverride("margin_top",    8);
            margin.AddThemeConstantOverride("margin_bottom", 8);
            bg.AddChild(margin);

            _cards = new HBoxContainer();
            _cards.AddThemeConstantOverride("separation", 8);
            margin.AddChild(_cards);

            var fm = FamilyManager.Instance;
            fm.MemberDied  += _ => Rebuild();
            fm.MemberAdded += _ => Rebuild();

            Rebuild();
        }

        // Пересобирает карточки по текущему списку живых членов семьи
        private void Rebuild()
        {
            foreach (var child in _cards.GetChildren())
                child.QueueFree();

            foreach (var member in FamilyManager.Instance.ActiveFamily.AliveMembers)
            {
                var card = new CharacterCard();
                card.Setup(member);
                _cards.AddChild(card);
            }
        }
    }
}
