using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace Start_mpr {
    //
    //  mprWin dictionaries (en, ru, ua)
    //  key == enStr
    //  id - en: 0, ru: 1, ua: 2
    //
    static class Locale {
        static private StringDictionary en;
        static private StringDictionary ua;
        static private StringDictionary ru;
        //
        // Add values to dictionaries (EN == UA == RU)
        //
        static private void Add(string key) {
            Add(key, key, key);
        }
        //
        // Add values to dictionaries (UA == RU)
        //
        static private void Add(string key, string commonStr) {
            Add(key, commonStr, commonStr);
        }
        //
        // Add values to dictionaries
        //
        static private void Add(string key, string ruStr, string uaStr) {
            en.Add(key, key);
            ru.Add(key, ruStr);
            ua.Add(key, uaStr);
        }
        //
        //  Fill all dictionaries
        //
        static private void fillDictionaries() {
            Add(LocaleKeys.file, "Файл");
            Add(LocaleKeys.changeUser, "Сменить пользователя", "Змінити користувача");
            Add(LocaleKeys.exit, "Выход", "Вихід");
            Add(LocaleKeys.test, "Тест");
            Add(LocaleKeys.classicalTest, "Классический (30 - 160)", "Класичний (30 - 160)");
            Add(LocaleKeys.shortTest, "Короткий (80 - 160)");
            Add(LocaleKeys.practice, "Практика");
            Add(LocaleKeys.slowTest, "Медленная (30 - 40)", "Повільна (30 - 40)");
            Add(LocaleKeys.middleTest, "Средняя (90 - 100)", "Середня (90 - 100)");
            Add(LocaleKeys.fastTest, "Быстрая (150 - 160)", "Швидка (150 - 160)");
            Add(LocaleKeys.results, "Результаты", "Результати");
            Add(LocaleKeys.writeLastToDB, "Записать последний в базу данных", "Записати останній в базу даних");
            Add(LocaleKeys.showResults, "Посмотреть результаты", "Показати результати");
            Add(LocaleKeys.lastResult, "Посмотреть последний результат", "Показати останній результат");
            Add(LocaleKeys.info, "Инфо", "Інфо");
            Add(LocaleKeys.about, "О программе", "Про программу");
            Add(LocaleKeys.theory, "Общие сведения", "Загальні відомості");
            Add(LocaleKeys.userGuide, "Инструкция для пользователя", "Інструкція для користувача");
            Add(LocaleKeys.settings, "Настройки", "Налаштування");
            Add(LocaleKeys.dataBase, "База данных", "База даних");
            Add(LocaleKeys.sound, "Звук");
            Add(LocaleKeys.devMode, "Режим розработчика", "Режим розробника");
            Add(LocaleKeys.lang, "Язык", "Мова");
            Add(LocaleKeys.maxSpeedTest, "Максимальная скорость (160 - 180)", "Максимальна швидкість (160 - 180)");
            Add(LocaleKeys.uniformTest, "Равномерный (30 - 30)", "Рівномірний (30 - 30)");
        }
        // 
        // Return english dictionary
        //
        static public StringDictionary getEnDictionary() {
            return en;
        }
        // 
        // Return russian dictionary
        //
        static public StringDictionary getRuDictionary() {
            return ru;
        }
        // 
        // Return ukranian dictionary
        //
        static public StringDictionary getUaDictionary() {
            return ua;
        }
        // 
        // Return dictionary by id
        //
        static public StringDictionary getDictionaryById(int id) {
            switch (id) {
                case 0:
                    return en;
                case 1:
                    return ru;
                case 2:
                    return ua;
                default:
                    return en;
            }
        }
        // 
        // User guide
        //
        static public string getInfo() {
            return "en: 0\nru: 1\nua:2\n\nUse: dictionary[Locale.key]\n";
        }
        //
        // Constructor
        //
        static Locale() {
            en = new StringDictionary();
            ru = new StringDictionary();
            ua = new StringDictionary();
            fillDictionaries();
        }
    }

    static class LocaleKeys {
        //
        //  dictionary keys (Locale) 
        //
        public const string file = "File";
        public const string changeUser = "Change user";
        public const string exit = "Exit";
        public const string test = "Test";
        public const string classicalTest = "Classical (30 - 160)";
        public const string shortTest = "Short (80 - 160)";
        public const string practice = "Practice";
        public const string slowTest = "Slow (30 - 40)";
        public const string middleTest = "Middle (90 - 100)";
        public const string fastTest = "Fast (150 - 160)";
        public const string results = "Results";
        public const string writeLastToDB = "Write last result to data base";
        public const string showResults = "Show results";
        public const string lastResult = "Show last result";
        public const string info = "Info";
        public const string about = "About";
        public const string theory = "Overview";
        public const string userGuide = "User guide";
        public const string settings = "Settings";
        public const string dataBase = "Data Base";
        public const string sound = "Sound";
        public const string devMode = "Developer mode";
        public const string lang = "Language";
        public const string maxSpeedTest = "Max speed (160 - 180)";
        public const string uniformTest = "Uniform (30 - 30)";
    }
}
