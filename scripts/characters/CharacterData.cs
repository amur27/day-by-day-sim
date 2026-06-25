// Вспомогательные классы данных персонажа: идентичность, жизненные параметры, базовые потребности,
// 18 черт, навыки, привычки, отношения, репутация, правовой статус, финансы, образование, занятость.
using System;
using System.Collections.Generic;

namespace DayByDaySim.Characters
{
    // Неизменяемые данные личности: имя, возраст, описание внешности.
    public class Identity
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int Age { get; set; } = 18;
        public string Appearance { get; set; } = "";
        public string FullName => $"{FirstName} {LastName}".Trim();
    }

    // Жизненные параметры: если HP или MP достигают 0 — персонаж умирает.
    // HP дренируют: голод, истощение, болезни, травмы.
    // MP дренируют: депрессия, высокий стресс, изоляция, тяжёлые потери.
    public class LifeStats
    {
        public float HP { get; set; } = 100f;   // Физическое здоровье (физический крах → смерть)
        public float MP { get; set; } = 100f;   // Духовное здоровье / счастье (духовный крах → смерть)

        public bool IsDead => HP <= 0f || MP <= 0f;
    }

    // Базовые потребности (0–100). При обнулении начинают медленно дренировать HP или MP.
    // Сами по себе к смерти не приводят — только через жизненные параметры.
    public class Needs
    {
        // Питание: при Nutrition ≈ 0 → дренаж HP
        public float Nutrition { get; set; } = 100f;

        // Сон / энергия: при Energy ≈ 0 → дренаж HP + MP (истощение бьёт по обоим)
        public float Energy { get; set; } = 100f;

        // Настроение: при Mood ≈ 0 → дренаж MP
        public float Mood { get; set; } = 70f;

        // Стресс: накапливается от долгов, конфликтов, перегрузок.
        // При Stress > 80 → дренаж MP; при Stress > 95 → возможны события (срыв, болезнь)
        public float Stress { get; set; } = 0f;
    }

    // 18 врождённых черт (0–100).
    // 0–20 = избегает / ненавидит, 41–60 = нейтрально, 81–100 = определяет личность.
    public class Characteristics
    {
        public float Diligence { get; set; } = 50f;          // Трудолюбие
        public float Bureaucracy { get; set; } = 50f;        // Бюрократизм
        public float LawAttitude { get; set; } = 50f;        // Отношение к закону
        public float ShoppingAttitude { get; set; } = 50f;   // Отношение к шоппингу
        public float AnalyticalAbility { get; set; } = 50f;  // Аналитические способности
        public float ComfortAttitude { get; set; } = 50f;    // Отношение к комфорту
        public float PrestigeAttitude { get; set; } = 50f;   // Отношение к престижу
        public float Creativity { get; set; } = 50f;         // Креативность
        public float SubstanceAttitude { get; set; } = 50f;  // Отношение к алкоголю/стимуляторам
        public float SportAttitude { get; set; } = 50f;      // Любовь к спорту
        public float TechAttitude { get; set; } = 50f;       // Отношение к технологиям
        public float HygieneAttitude { get; set; } = 50f;    // Отношение к гигиене
        public float Sociability { get; set; } = 50f;        // Любовь к общению
        public float Spirituality { get; set; } = 50f;       // Духовность
        public float FoodAttitude { get; set; } = 50f;       // Любовь к еде
        public float SleepAttitude { get; set; } = 50f;      // Любовь ко сну
        public float DrivingAttitude { get; set; } = 50f;    // Любовь к езде
        public float RiskAttitude { get; set; } = 50f;       // Отношение к риску
    }

    // Приобретённый навык. Открытый список — создаётся динамически по имени.
    public class Skill
    {
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";   // Интеллектуальный / Творческий / Физический...
        public int Level { get; set; } = 0;          // Уровень (порог для задач)
        public float XP { get; set; } = 0f;          // Накопленный опыт до следующего уровня
    }

    // Привычка: сформированный паттерн поведения, меняет вероятность выбора действий.
    public class Habit
    {
        public string Name { get; set; } = "";
        public HabitPolarity Polarity { get; set; }
        public float Strength { get; set; } = 0.5f;  // 0–1: насколько укоренилась
        public string Description { get; set; } = "";
    }

    // Данные отношений с конкретным персонажем. Все значения –100..100 кроме Compatibility.
    public class RelationshipData
    {
        public RelationshipType Type { get; set; }
        public float Trust { get; set; } = 50f;            // Доверие
        public float Respect { get; set; } = 50f;          // Уважение
        public float Tension { get; set; } = 0f;           // Напряжение
        public float Influence { get; set; } = 0f;         // Влияние одного на другого
        public float Compatibility { get; set; } = 50f;    // Совместимость характеров
        public float Resentment { get; set; } = 0f;        // Обида
        public float Obligation { get; set; } = 0f;        // Долг / обязательство
        public float SocialReputation { get; set; } = 50f; // Репутация внутри соц. круга
    }

    // Репутация в трёх сферах (0–100).
    public class Reputation
    {
        public float Professional { get; set; } = 50f;  // Профессиональная
        public float Business { get; set; } = 50f;      // Деловая
        public float Social { get; set; } = 50f;        // Социальная
    }

    // Документ: паспорт, ИНН, свидетельство, трудовая книжка...
    public class Document
    {
        public string Type { get; set; } = "";
        public string Issuer { get; set; } = "";
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DocumentStatus Status { get; set; } = DocumentStatus.Valid;
    }

    // Судебное или административное дело.
    public class LegalCase
    {
        public LegalCaseType Type { get; set; }
        public string Subject { get; set; } = "";
        public DateTime FilingDate { get; set; }
        public LegalCaseStatus Status { get; set; } = LegalCaseStatus.Open;
        public List<string> PossibleOutcomes { get; set; } = new();
        public bool LawyerRequired { get; set; } = false;
    }

    // Штраф за административное нарушение или просрочку.
    public class Fine
    {
        public string Reason { get; set; } = "";
        public decimal Amount { get; set; }
        public string Issuer { get; set; } = "";
        public DateTime DueDate { get; set; }
        public FineStatus Status { get; set; } = FineStatus.Unpaid;
    }

    // Долговое обязательство: кредит, займ. Просрочка ведёт к изъятию залога или суду.
    public class Debt
    {
        public string Creditor { get; set; } = "";
        public decimal Principal { get; set; }          // Основная сумма
        public float InterestRate { get; set; }         // Годовая процентная ставка
        public decimal MonthlyPayment { get; set; }     // Ежемесячный платёж
        public int RemainingMonths { get; set; }
        public string Collateral { get; set; } = "";    // Залог (пустая строка = без залога)
        public string PenaltyOnDefault { get; set; } = ""; // Последствия при просрочке
    }

    // Агрегатор правового состояния персонажа.
    public class LegalStatus
    {
        public List<Document> Documents { get; set; } = new();
        public List<LegalCase> Cases { get; set; } = new();
        public List<Fine> Fines { get; set; } = new();
        public bool HasCriminalRecord { get; set; } = false;
    }

    // Личные финансы: наличные, счёт, долги, штрафы, доходы и расходы за месяц.
    public class PersonalFinances
    {
        public decimal Cash { get; set; } = 0m;
        public decimal BankBalance { get; set; } = 0m;
        public List<Debt> Debts { get; set; } = new();
        public List<Fine> Fines { get; set; } = new();
        public decimal MonthlyIncome { get; set; } = 0m;
        public decimal MonthlyExpenses { get; set; } = 0m;
        public decimal NetMonthly => MonthlyIncome - MonthlyExpenses;
    }

    // Образование и текущая учёба.
    public class Education
    {
        public EducationLevel Level { get; set; } = EducationLevel.Secondary;
        public List<string> Institutions { get; set; } = new();
        public List<string> Qualifications { get; set; } = new();
        public bool IsCurrentlyStudying { get; set; } = false;
    }

    // Занятость: должность, работодатель, зарплата. IsEmployed вычисляется автоматически.
    public class Employment
    {
        public string? JobTitle { get; set; }
        public string? Employer { get; set; }
        public decimal Salary { get; set; } = 0m;
        public bool IsEmployed => JobTitle != null;
    }
}
