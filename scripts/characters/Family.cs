// Семья: контейнер для персонажей, история династии и общие активы.
// Конец игры наступает когда IsExtinct == true и наследников нет.
using System.Collections.Generic;

namespace DayByDaySim.Characters
{
    public class Family
    {
        public string Name { get; set; } = "Семья";
        public List<FamilyMember> Members { get; set; } = new();
        public List<string> DynastyHistory { get; set; } = new();

        // Живые члены семьи (под контролем игрока или нет)
        public List<FamilyMember> AliveMembers =>
            Members.FindAll(m => m.IsAlive);

        // Персонажи, которыми может управлять игрок
        public List<FamilyMember> PlayableMembers =>
            Members.FindAll(m => m.IsAlive && m.IsPlayerControlled);

        // Игра заканчивается когда не осталось живых членов семьи
        public bool IsExtinct => !Members.Exists(m => m.IsAlive);

        // Найти персонажа по Id
        public FamilyMember? FindById(string id) =>
            Members.Find(m => m.Id == id);
    }
}
