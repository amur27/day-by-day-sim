// Статический хелпер локализации: обёртка над TranslationServer.
// Использование: L10n.T("ui.close") → "Закрыть" / "Close"
//                L10n.T("event.fine", 5000) → "Штраф: 5 000 руб."
//                L10n.Plural("economy.ruble", 3) → "3 рубля"
// Ключи организованы по доменам через точку: domain.category.name
using Godot;

namespace DayByDaySim.Core
{
    public static class L10n
    {
        // Перевод по ключу. Если ключ не найден — TranslationServer вернёт сам ключ.
        public static string T(string key) =>
            TranslationServer.Translate(key);

        // Перевод с форматированием: L10n.T("event.eviction", playerName)
        public static string T(string key, params object[] args) =>
            string.Format(T(key), args);

        // Множественная форма: L10n.Plural("economy.ruble", 3) → "3 рубля"
        // В .po файле ключ должен иметь msgid_plural с тем же ключом и несколько msgstr[n].
        public static string Plural(string key, int n) =>
            string.Format(TranslationServer.PluralTranslate(key, key, n), n);

        // Сменить язык во время игры
        public static void SetLocale(string locale) =>
            TranslationServer.SetLocale(locale);

        public static string CurrentLocale =>
            TranslationServer.GetLocale();
    }
}
