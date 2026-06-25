// Симулятор потребностей: рассчитывает почасовой дренаж Needs и его влияние на LifeStats.
// Вызывается FamilyManager при каждом сигнале HourPassed из TimeManager.
// Потребности не убивают напрямую — только через дренаж HP/MP.

using System;
using DayByDaySim.Characters;

namespace DayByDaySim.Gameplay
{
    public static class NeedsSimulator
    {
        // Дренаж потребностей за игровой час (базовые значения без модификаторов)
        private const float NutritionDrain = 3.0f;  // без еды ~33 часа до нуля
        private const float EnergyDrain    = 4.0f;  // без сна ~25 часов до нуля
        private const float MoodDrain      = 1.5f;  // ~67 часов естественного спада
        private const float StressDecay    = 0.5f;  // стресс снижается сам по себе при отсутствии раздражителей

        // Порог критического состояния для Nutrition / Energy / Mood
        private const float CriticalNeed = 10f;

        // Пороги стресса
        private const float StressHigh     = 80f;
        private const float StressCritical = 95f;

        // Урон HP/MP при критических потребностях (за игровой час)
        private const float NutritionHpDamage      = 5f;
        private const float EnergyHpDamage         = 3f;
        private const float EnergyMpDamage         = 2f;
        private const float MoodMpDamage           = 4f;
        private const float StressHighMpDamage     = 2f;
        private const float StressCriticalMpDamage = 5f;

        // Применяет один часовой тик к персонажу. Возвращает true если персонаж умер в этот тик.
        public static bool Tick(FamilyMember member)
        {
            var needs = member.Needs;
            var life  = member.Life;

            // Дренаж потребностей
            needs.Nutrition = MathF.Max(0f, needs.Nutrition - NutritionDrain);
            needs.Energy    = MathF.Max(0f, needs.Energy    - EnergyDrain);
            needs.Mood      = MathF.Max(0f, needs.Mood      - MoodDrain);
            needs.Stress    = MathF.Max(0f, needs.Stress    - StressDecay);

            // Влияние критических потребностей на HP/MP
            if (needs.Nutrition < CriticalNeed)
                life.HP -= NutritionHpDamage;

            if (needs.Energy < CriticalNeed)
            {
                life.HP -= EnergyHpDamage;
                life.MP -= EnergyMpDamage;
            }

            if (needs.Mood < CriticalNeed)
                life.MP -= MoodMpDamage;

            if (needs.Stress > StressCritical)
                life.MP -= StressCriticalMpDamage;
            else if (needs.Stress > StressHigh)
                life.MP -= StressHighMpDamage;

            // Зажим в пределах [0, 100]
            life.HP = Math.Clamp(life.HP, 0f, 100f);
            life.MP = Math.Clamp(life.MP, 0f, 100f);

            return life.IsDead;
        }
    }
}
