// Autoload-синглтон семьи: хранит активную Family, предоставляет доступ из любого скрипта,
// уведомляет системы о смерти персонажа и вымирании династии.
// FamilyMember — не Godot-тип, поэтому уведомления реализованы через C#-события, не Godot-сигналы.
using Godot;
using System;
using System.Collections.Generic;
using DayByDaySim.Characters;

namespace DayByDaySim.Gameplay
{
    public partial class FamilyManager : Node
    {
        public static FamilyManager Instance { get; private set; } = null!;

        public Family ActiveFamily { get; private set; } = new();

        // Уведомления о событиях семьи (C# события, не Godot-сигналы)
        public event Action<FamilyMember>? MemberDied;
        public event Action<FamilyMember>? MemberAdded;
        public event Action? FamilyExtinct;

        public override void _Ready()
        {
            Instance = this;
            GetNode<TimeManager>("/root/TimeManager").HourPassed += OnHourPassed;
        }

        // Применяет тик симулятора потребностей ко всем живым членам семьи
        private void OnHourPassed(int hour)
        {
            // Собираем умерших отдельно — нельзя вызывать KillMember во время итерации по AliveMembers
            var died = new List<string>();

            foreach (var member in ActiveFamily.AliveMembers)
            {
                if (NeedsSimulator.Tick(member))
                    died.Add(member.Id);
            }

            foreach (var id in died)
                KillMember(id);
        }

        // Заменить активную семью (вызывается при загрузке сейва или старте сценария)
        public void SetFamily(Family family)
        {
            ActiveFamily = family;
        }

        // Добавить нового члена семьи
        public void AddMember(FamilyMember member)
        {
            ActiveFamily.Members.Add(member);
            MemberAdded?.Invoke(member);
        }

        // Зафиксировать смерть персонажа и проверить вымирание династии
        public void KillMember(string memberId)
        {
            var member = ActiveFamily.FindById(memberId);
            if (member == null || !member.IsAlive)
                return;

            member.IsAlive = false;
            member.Life.HP = 0f;
            MemberDied?.Invoke(member);

            if (ActiveFamily.IsExtinct)
                FamilyExtinct?.Invoke();
        }

        // Получить персонажа по Id
        public FamilyMember? GetMember(string id) => ActiveFamily.FindById(id);

        // --- Тестовые данные для прототипа ---

        // Создать тестовую семью с двумя персонажами
        public Family CreateTestFamily()
        {
            var family = new Family { Name = "Морозовы" };

            var parent = new FamilyMember
            {
                IsPlayerControlled = true
            };
            parent.Identity.FirstName = "Алексей";
            parent.Identity.LastName = "Морозов";
            parent.Identity.Age = 38;
            parent.Employment.JobTitle = "Бухгалтер";
            parent.Employment.Employer = "ООО «Стройком»";
            parent.Employment.Salary = 65000m;
            parent.Finances.BankBalance = 120000m;
            parent.Finances.MonthlyIncome = 65000m;
            parent.Finances.MonthlyExpenses = 48000m;
            parent.Characteristics.Diligence = 72f;
            parent.Characteristics.Bureaucracy = 68f;
            parent.Characteristics.RiskAttitude = 30f;

            var child = new FamilyMember
            {
                IsPlayerControlled = true
            };
            child.Identity.FirstName = "Маша";
            child.Identity.LastName = "Морозова";
            child.Identity.Age = 15;
            child.Education.IsCurrentlyStudying = true;
            child.Characteristics.AnalyticalAbility = 80f;
            child.Characteristics.Creativity = 65f;
            child.Characteristics.Sociability = 58f;

            // Связь: отец — дочь
            parent.Relationships[child.Id] = new RelationshipData
            {
                Type = RelationshipType.Child,
                Trust = 85f,
                Respect = 70f
            };
            child.Relationships[parent.Id] = new RelationshipData
            {
                Type = RelationshipType.Parent,
                Trust = 80f,
                Respect = 75f
            };

            family.Members.Add(parent);
            family.Members.Add(child);
            return family;
        }
    }
}
