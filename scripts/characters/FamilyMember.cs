// Главная сущность персонажа: агрегирует все данные (идентичность, жизненные параметры,
// базовые потребности, черты, навыки, привычки, отношения, репутацию, правовой статус,
// финансы, образование, занятость). Не является Godot-нодой — чистая модель данных.
using System;
using System.Collections.Generic;

namespace DayByDaySim.Characters
{
    public class FamilyMember
    {
        public string Id { get; } = Guid.NewGuid().ToString();

        public Identity Identity { get; set; } = new();

        // Жизненные параметры: HP + MP. При достижении 0 — персонаж умирает.
        public LifeStats Life { get; set; } = new();

        // Базовые потребности: Nutrition / Energy / Mood / Stress.
        // При обнулении дренируют Life.HP или Life.MP, но не убивают напрямую.
        public Needs Needs { get; set; } = new();

        public Characteristics Characteristics { get; set; } = new();
        public List<Skill> Skills { get; set; } = new();
        public List<Habit> Habits { get; set; } = new();

        // Ключ — Id другого FamilyMember или NPC
        public Dictionary<string, RelationshipData> Relationships { get; set; } = new();

        public Reputation Reputation { get; set; } = new();
        public LegalStatus LegalStatus { get; set; } = new();
        public PersonalFinances Finances { get; set; } = new();
        public Education Education { get; set; } = new();
        public Employment Employment { get; set; } = new();

        public bool IsAlive { get; set; } = true;
        public bool IsPlayerControlled { get; set; } = false;

        // Удобный доступ к часто нужным данным
        public string FullName => Identity.FullName;
        public int Age => Identity.Age;

        // Найти навык по имени (без учёта регистра)
        public Skill? GetSkill(string name) =>
            Skills.Find(s => string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase));

        // Получить уровень навыка (0 если не изучен)
        public int GetSkillLevel(string name) => GetSkill(name)?.Level ?? 0;

        // Проверить наличие действующего документа
        public bool HasValidDocument(string type) =>
            LegalStatus.Documents.Exists(d =>
                string.Equals(d.Type, type, StringComparison.OrdinalIgnoreCase) &&
                d.Status == DocumentStatus.Valid);
    }
}
