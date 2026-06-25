// Плавающее окно персонажа: перетаскивается за заголовок, показывает HP/MP и нужды.
// Клик по заголовку выводит окно на передний план через MoveToFront().

using Godot;
using DayByDaySim.Characters;

namespace DayByDaySim.UI
{
    public partial class CharacterWindow : PanelContainer
    {
        private FamilyMember _member;
        private ProgressBar _hpBar;
        private ProgressBar _mpBar;
        private ProgressBar _nutritionBar;
        private ProgressBar _energyBar;
        private ProgressBar _moodBar;
        private ProgressBar _stressBar;

        private bool _dragging;
        private Vector2 _dragOffset;

        public void Setup(FamilyMember member, Vector2 position)
        {
            _member  = member;
            Position = position;
            CustomMinimumSize = new Vector2(170f, 0f);

            // Стиль основного окна — тёмный фон с тонкой рамкой
            AddThemeStyleboxOverride("panel", new StyleBoxFlat
            {
                BgColor               = new Color(0.12f, 0.12f, 0.15f, 0.93f),
                BorderColor           = new Color(0.35f, 0.35f, 0.50f),
                BorderWidthLeft       = 1,
                BorderWidthRight      = 1,
                BorderWidthTop        = 1,
                BorderWidthBottom     = 1,
                CornerRadiusTopLeft   = 5,
                CornerRadiusTopRight  = 5,
                CornerRadiusBottomLeft  = 5,
                CornerRadiusBottomRight = 5,
            });

            var vbox = new VBoxContainer();
            vbox.AddThemeConstantOverride("separation", 0);
            AddChild(vbox);

            // --- Заголовок (область для перетаскивания) ---
            var header = new PanelContainer();
            header.AddThemeStyleboxOverride("panel", new StyleBoxFlat
            {
                BgColor              = new Color(0.22f, 0.24f, 0.35f, 1f),
                CornerRadiusTopLeft  = 5,
                CornerRadiusTopRight = 5,
            });
            header.MouseDefaultCursorShape = CursorShape.Drag;
            header.GuiInput += OnHeaderInput;
            vbox.AddChild(header);

            var headerPad = new MarginContainer();
            headerPad.AddThemeConstantOverride("margin_left",   8);
            headerPad.AddThemeConstantOverride("margin_right",  8);
            headerPad.AddThemeConstantOverride("margin_top",    5);
            headerPad.AddThemeConstantOverride("margin_bottom", 5);
            header.AddChild(headerPad);

            var nameLabel = new Label
            {
                Text = TruncateName(member.FullName),
                HorizontalAlignment = HorizontalAlignment.Left,
                AutowrapMode        = TextServer.AutowrapMode.Off,
            };
            headerPad.AddChild(nameLabel);

            // --- Контент ---
            var contentPad = new MarginContainer();
            contentPad.AddThemeConstantOverride("margin_left",   8);
            contentPad.AddThemeConstantOverride("margin_right",  8);
            contentPad.AddThemeConstantOverride("margin_top",    6);
            contentPad.AddThemeConstantOverride("margin_bottom", 6);
            vbox.AddChild(contentPad);

            var content = new VBoxContainer();
            content.AddThemeConstantOverride("separation", 4);
            contentPad.AddChild(content);

            _hpBar = AddStatRow(content, "HP", new Color(0.85f, 0.20f, 0.20f));
            _mpBar = AddStatRow(content, "MP", new Color(0.25f, 0.45f, 0.90f));

            content.AddChild(new HSeparator());

            var needsBox = new HBoxContainer();
            needsBox.AddThemeConstantOverride("separation", 4);
            content.AddChild(needsBox);

            _nutritionBar = AddNeedColumn(needsBox, "Еда",  new Color(0.20f, 0.80f, 0.30f));
            _energyBar    = AddNeedColumn(needsBox, "Сон",  new Color(0.90f, 0.80f, 0.10f));
            _moodBar      = AddNeedColumn(needsBox, "Наст", new Color(0.20f, 0.80f, 0.90f));
            _stressBar    = AddNeedColumn(needsBox, "Стр",  new Color(0.90f, 0.30f, 0.10f));
        }

        private void OnHeaderInput(InputEvent @event)
        {
            if (@event is InputEventMouseButton mb && mb.ButtonIndex == MouseButton.Left)
            {
                _dragging = mb.Pressed;
                if (_dragging)
                {
                    _dragOffset = GetGlobalMousePosition() - GlobalPosition;
                    MoveToFront();
                }
            }
            else if (@event is InputEventMouseMotion && _dragging)
            {
                GlobalPosition = GetGlobalMousePosition() - _dragOffset;
            }
        }

        public override void _Process(double delta)
        {
            if (_member == null) return;
            _hpBar.Value        = _member.Life.HP;
            _mpBar.Value        = _member.Life.MP;
            _nutritionBar.Value = _member.Needs.Nutrition;
            _energyBar.Value    = _member.Needs.Energy;
            _moodBar.Value      = _member.Needs.Mood;
            _stressBar.Value    = _member.Needs.Stress;
        }

        private static ProgressBar AddStatRow(VBoxContainer parent, string label, Color fillColor)
        {
            var row = new HBoxContainer();
            row.AddThemeConstantOverride("separation", 4);
            parent.AddChild(row);

            row.AddChild(new Label
            {
                Text = label,
                CustomMinimumSize = new Vector2(22f, 0f),
                VerticalAlignment = VerticalAlignment.Center,
            });

            var bar = MakeBar(fillColor, 0f, 12f);
            bar.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            row.AddChild(bar);
            return bar;
        }

        private static ProgressBar AddNeedColumn(HBoxContainer parent, string label, Color fillColor)
        {
            var col = new VBoxContainer();
            col.AddThemeConstantOverride("separation", 2);
            col.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            parent.AddChild(col);

            var lbl = new Label
            {
                Text = label,
                HorizontalAlignment = HorizontalAlignment.Center,
                AutowrapMode        = TextServer.AutowrapMode.Off,
            };
            lbl.AddThemeFontSizeOverride("font_size", 9);
            col.AddChild(lbl);

            var bar = MakeBar(fillColor, 0f, 8f);
            bar.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            col.AddChild(bar);
            return bar;
        }

        private static ProgressBar MakeBar(Color fillColor, float minWidth, float minHeight)
        {
            var bar = new ProgressBar
            {
                MinValue          = 0,
                MaxValue          = 100,
                Value             = 100,
                ShowPercentage    = false,
                CustomMinimumSize = new Vector2(minWidth, minHeight),
            };
            bar.AddThemeStyleboxOverride("fill",       new StyleBoxFlat { BgColor = fillColor });
            bar.AddThemeStyleboxOverride("background", new StyleBoxFlat { BgColor = new Color(0.15f, 0.15f, 0.15f) });
            return bar;
        }

        private static string TruncateName(string fullName)
        {
            if (fullName.Length <= 15) return fullName;
            var parts = fullName.Split(' ');
            return parts.Length >= 2 ? $"{parts[0]} {parts[1][0]}." : fullName[..15];
        }
    }
}
