// Autoload CanvasLayer: создаёт и пересоздаёт плавающие окна CharacterWindow
// для каждого живого члена семьи. Стартовые позиции — верхний левый угол, с отступом.

using Godot;
using DayByDaySim.Gameplay;

namespace DayByDaySim.UI
{
    public partial class FamilyPanel : CanvasLayer
    {
        // Горизонтальный шаг между стартовыми позициями окон
        private const float WindowStep = 185f;

        public override void _Ready()
        {
            Layer = 10;

            var fm = FamilyManager.Instance;
            fm.MemberDied  += _ => Rebuild();
            fm.MemberAdded += _ => Rebuild();

            Rebuild();
        }

        private void Rebuild()
        {
            foreach (var child in GetChildren())
                child.QueueFree();

            int i = 0;
            foreach (var member in FamilyManager.Instance.ActiveFamily.AliveMembers)
            {
                var window = new CharacterWindow();
                window.Setup(member, new Vector2(10f + i * WindowStep, 10f));
                AddChild(window);
                i++;
            }
        }
    }
}
