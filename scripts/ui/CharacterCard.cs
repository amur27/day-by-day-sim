// Мини-карточка персонажа: имя, полосы HP/MP и 4 индикатора потребностей.
// Значения обновляются каждый кадр через _Process без аллокаций.

using Godot;
using DayByDaySim.Characters;

namespace DayByDaySim.UI
{
    public partial class CharacterCard : PanelContainer
    {
        private FamilyMember _member;

        private ProgressBar _hpBar;
        private ProgressBar _mpBar;
        private ProgressBar _nutritionBar;
        private ProgressBar _energyBar;
        private ProgressBar _moodBar;
        private ProgressBar _stressBar;

        public void Setup(FamilyMember member)
        {
            _member = member;
            CustomMinimumSize = new Vector2(165f, 0f);

            var vbox = new VBoxContainer();
            vbox.AddThemeConstantOverride("separation", 4);
            AddChild(vbox);

            // Имя
            var name = new Label
            {
                Text = TruncateName(member.FullName),
                HorizontalAlignment = HorizontalAlignment.Center,
                AutowrapMode = TextServer.AutowrapMode.Off
            };
            vbox.AddChild(name);

            var sep = new HSeparator();
            vbox.AddChild(sep);

            // HP и MP
            var statsBox = new VBoxContainer();
            statsBox.AddThemeConstantOverride("separation", 3);
            vbox.AddChild(statsBox);

            _hpBar = AddStatRow(statsBox, "HP", new Color(0.85f, 0.20f, 0.20f));
            _mpBar = AddStatRow(statsBox, "MP", new Color(0.25f, 0.45f, 0.90f));

            var sep2 = new HSeparator();
            vbox.AddChild(sep2);

            // Потребности
            var needsBox = new HBoxContainer();
            needsBox.AddThemeConstantOverride("separation", 4);
            vbox.AddChild(needsBox);

            _nutritionBar = AddNeedColumn(needsBox, "Еда",  new Color(0.20f, 0.80f, 0.30f));
            _energyBar    = AddNeedColumn(needsBox, "Сон",  new Color(0.90f, 0.80f, 0.10f));
            _moodBar      = AddNeedColumn(needsBox, "Наст", new Color(0.20f, 0.80f, 0.90f));
            _stressBar    = AddNeedColumn(needsBox, "Стр",  new Color(0.90f, 0.30f, 0.10f));
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

            var lbl = new Label
            {
                Text = label,
                CustomMinimumSize = new Vector2(22f, 0f),
                VerticalAlignment = VerticalAlignment.Center
            };
            row.AddChild(lbl);

            var bar = MakeBar(fillColor, 0f, 13f);
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
                AutowrapMode = TextServer.AutowrapMode.Off
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
                MinValue = 0,
                MaxValue = 100,
                Value    = 100,
                ShowPercentage = false,
                CustomMinimumSize = new Vector2(minWidth, minHeight)
            };
            bar.AddThemeStyleboxOverride("fill",       new StyleBoxFlat { BgColor = fillColor });
            bar.AddThemeStyleboxOverride("background", new StyleBoxFlat { BgColor = new Color(0.15f, 0.15f, 0.15f) });
            return bar;
        }

        // Алексей Морозов → Алексей М.
        private static string TruncateName(string fullName)
        {
            if (fullName.Length <= 15) return fullName;
            var parts = fullName.Split(' ');
            return parts.Length >= 2 ? $"{parts[0]} {parts[1][0]}." : fullName[..15];
        }
    }
}
