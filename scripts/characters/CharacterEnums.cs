// Перечисления для системы персонажей: типы отношений, полярность привычек, статусы правовых сущностей.
namespace DayByDaySim.Characters
{
    public enum RelationshipType
    {
        Spouse,         // Супруг/супруга
        Parent,         // Родитель
        Child,          // Ребёнок
        Sibling,        // Брат/сестра
        Relative,       // Дальний родственник
        Friend,         // Друг
        Colleague,      // Коллега
        Boss,           // Начальник
        Subordinate,    // Подчинённый
        Neighbor,       // Сосед
        Mentor,         // Наставник
        Mentee,         // Ученик
        UsefulContact,  // Полезный контакт
        Enemy           // Враг
    }

    public enum HabitPolarity
    {
        Positive,   // Полезная привычка
        Negative    // Вредная привычка
    }

    public enum DocumentStatus
    {
        Valid,      // Действителен
        Expired,    // Просрочен
        Revoked     // Аннулирован
    }

    public enum LegalCaseType
    {
        Civil,          // Гражданское дело
        Criminal,       // Уголовное дело
        Administrative  // Административное дело
    }

    public enum LegalCaseStatus
    {
        Open,        // Открыто
        InProgress,  // В процессе
        Closed,      // Закрыто
        Enforcement  // Стадия исполнения
    }

    public enum FineStatus
    {
        Unpaid,    // Не оплачен
        Paid,      // Оплачен
        Overdue,   // Просрочен
        Contested  // Оспаривается
    }

    public enum EducationLevel
    {
        None,           // Нет образования
        Primary,        // Начальное
        Secondary,      // Среднее
        Vocational,     // Среднее специальное
        Incomplete,     // Неполное высшее
        Higher,         // Высшее (бакалавр/специалист)
        Postgraduate    // Аспирантура / учёная степень
    }
}
