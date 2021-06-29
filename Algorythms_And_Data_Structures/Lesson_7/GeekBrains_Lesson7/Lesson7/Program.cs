using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson7
{
    class Program
    {
        #region ---- STRING CONSTS ----

        /// <summary> Ключи для словаря аргументов командной строки </summary>
        enum Arguments
        {
            Help,
            Seed,
            Delay,
            HelpText
        }

        /// <summary> Словарь аргументов командной строки </summary>
        private static readonly Dictionary<Arguments, string> arguments = new Dictionary<Arguments, string>
        {
        { Arguments.Help, "-h"},
        { Arguments.Seed, "-s"},
        { Arguments.Delay, "-d"},
        { Arguments.HelpText,
                "-s <int> - seed for random number generator\n" +
                "-d <int> - visualization delay"},
        };

        /// <summary> Ключи для словаря с сообщениями об ошибках </summary>
        enum Errors
        {
            ItemNotFound,
            RepeatInputError,
        }

        /// <summary> Словарь с сообщениями об ошибках </summary>
        private static readonly Dictionary<Errors, string> errors = new Dictionary<Errors, string>
        {
        { Errors.ItemNotFound, "Элемент не найден."},
        { Errors.RepeatInputError, "Ошибка. Повторите ввод."}
        };

        /// <summary> Ключи для словаря с ссобщениями для пользователя </summary>
        enum Messages
        {
            ChooseOption,
            EnterNumber,
            PressAnyKey,
            From,
            To,
            Amount,
            WhiteSpaceLine,
            EnterColumn,
            EnterRow,
            EnterWidth,
            EnterHeight
        }

        /// <summary> Словарь с сообщениями для пользователя </summary>
        private static readonly Dictionary<Messages, string> messages = new Dictionary<Messages, string>
        {
        { Messages.ChooseOption, "Выберите опцию:"},
        { Messages.EnterNumber, "Введите число: "},
        { Messages.PressAnyKey, "Нажмите любую клавишу."},
        { Messages.From, "от"},
        { Messages.To, "до"},
        { Messages.Amount, "всего"},
        { Messages.WhiteSpaceLine, "        "},
        { Messages.EnterColumn, "Введите номер столбца: "},
        { Messages.EnterRow, "Введите номер ряда: "},
        { Messages.EnterWidth, "Введите ширину поля: "},
        { Messages.EnterHeight, "Введите высоту поля: "}
        };

        /// <summary> Пункты главного меню, последний пункт выход из программы </summary>
        private static readonly string[] mainMenu = new string[]
        {
            "Подсчет количества путей без рекурсии",
            "Подсчет количества путей через рекурсию\n",
            "Создать новое поле\n",
            "Добавить препятствие на поле по координатам",
            "Добавить случайное препятствие на поле\n",
            "Очистить рассчеты",
            "Очистить поле от препятствий\n",
            "Выход"
        };

        #endregion

        #region ---- NUMERIC CONSTS ----

        /// <summary>Задержка отрисовки по умолчанию</summary>
        public const int DELAY = 0;

        /// <summary>Ширина окна консоли</summary>
        private const int CONSOLE_WINDOW_W = 88;
        /// <summary>Высота окна консоли</summary>
        private const int CONSOLE_WINDOW_H = 36;

        #endregion

        #region ---- FIELDS & PROPERTIES ----
        /// <summary>Генератор случайных чисел</summary>
        private static Random rnd;
        /// <summary>seed для генератора случайных чисел</summary>
        private static int seed = 0;

        /// <summary>Задержка для визуализации алгоритма</summary>
        private static int delay = DELAY;

        /// <summary>Поле в котором ведется подсчет путей</summary>
        private static WaySearcher waySearcher;

        #endregion

        static int Main(string[] args)
        {
            #region ---- INIT ----

            //Изменяем размер окна консоли, чтобы влезали все художества с графом
            Console.SetWindowSize(CONSOLE_WINDOW_W, CONSOLE_WINDOW_H);

            //Обработка аругментов командной строки
            if (args.Length != 0)
            {
                if (args[0] == arguments[Arguments.Help])//Вывод справки по аргументам
                {
                    Console.WriteLine(arguments[Arguments.HelpText]);
                    return 0;
                }
                else
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i] == arguments[Arguments.Seed])//Изменение seed'a
                        {
                            try
                            {
                                int.TryParse(args[i + 1], out seed);
                            }
                            catch
                            {
                            }
                        }
                        else if (args[i] == arguments[Arguments.Delay])//Изменение задержки визуализации
                        {
                            try
                            {
                                int.TryParse(args[i + 1], out delay);
                            }
                            catch
                            {
                            }
                        }

                    }


            }

            if (seed != 0)
                rnd = new Random(seed);
            else
                rnd = new Random();//2

            //Формируем сообщение главного меню
            string mainMenuMessage = MenuMessage(mainMenu);

            #endregion

            #region ---- SEARCHER MAKING ----

            waySearcher = new WaySearcher(WaySearcher.MAX_WIDTH, WaySearcher.MAX_HEIGHT);
            waySearcher.Delay = delay;

            #endregion

            #region ---- MAIN WORK CYCLE ----

            MainMenu(mainMenu);

            #endregion

            return 0;
        }

        #region ---- MENUS ----

        /// <summary>Выводит на экран поле в котором ведется подсчет</summary>
        private static void Print()
        {
            waySearcher.PrintField();
            Console.WriteLine();
        }


        /// <summary>Формирует строку содержащую пункты меню для вывода на экран</summary>
        /// <param name="menu">Массив строк с пунктами меню</param>
        /// <returns>Строку содержащую пункты меню</returns>
        private static string MenuMessage(string[] menu)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"\n{messages[Messages.ChooseOption]}\n");
            for (int i = 0; i < menu.Length; i++)
                stringBuilder.Append($"{(i < menu.Length - 1 ? i + 1 : 0)} - {menu[i]}\n");

            return stringBuilder.ToString();
        }


        /// <summary>Главное меню</summary>
        /// <param name="menu">Массив содержащий пункты меню</param>
        private static void MainMenu(string[] menu)
        {
            string menuMessage = MenuMessage(menu);

            bool isExit = false;
            while (!isExit)
            {
                Print();
                int input = NumberInput(menuMessage, 0, menu.Length - 1);
                switch (input)
                {
                    case 1://search ways
                        waySearcher.SearchWays();
                        break;
                    case 2://search ways
                        waySearcher.SearchWaysRecurrent();
                        break;
                    case 3://create new field
                        Print();
                        int width = NumberInput(messages[Messages.EnterWidth], WaySearcher.MIN_WIDTH, WaySearcher.MAX_WIDTH, false);
                        int height = NumberInput(messages[Messages.EnterHeight], WaySearcher.MIN_HEIGHT, WaySearcher.MAX_HEIGHT, false);
                        waySearcher = new WaySearcher(width, height);
                        break;
                    case 4://add obstacle
                        Print();
                        int column = NumberInput(messages[Messages.EnterColumn], 0, waySearcher.Width - 1, false);
                        int row = NumberInput(messages[Messages.EnterRow], 0, waySearcher.Height - 1, false);
                        waySearcher.AddObstacle(column, row);
                        break;
                    case 5://add random obstacle
                        bool isAdded = false;
                        while (!isAdded)
                        {
                            column = rnd.Next(waySearcher.Width);
                            row = rnd.Next(waySearcher.Height);
                            isAdded = waySearcher.AddObstacle(column, row);
                        }
                        break;
                    case 6://delete calculations
                        waySearcher.ClearField(false);
                        break;
                    case 7://delete obstacles
                        waySearcher.ClearField(true);
                        break;
                    case 0://exit
                        isExit = true;
                        break;
                }

            }
        }


        #endregion

        #region ---- ADDITIONAL METHODS ----

        /// <summary>
        /// Метод запрашивает у пользователя целое int число.
        /// </summary>
        /// <param name="message">Сообщение для пользователя</param>
        /// <param name="min">Минимальное значение ввода</param>
        /// <param name="max">Максимальное значение ввода</param>
        /// <param name="isOneDigit">Запрашивать одну цифру или несколько</param>
        /// <returns>Введенное пользователем целое число больше нуля.</returns>
        private static int NumberInput(string message, int min, int max, bool isOneDigit = true)
        {
            bool isInputCorrect = false; //флаг проверки
            int input = 0;
            Console.WriteLine($"{message}({messages[Messages.From]} {min} {messages[Messages.To]} {max})");
            while (!isInputCorrect) //Цикл будет повторятся, пока вводимое число не пройдет все проверки
            {
                if (isOneDigit)
                    isInputCorrect = int.TryParse(Console.ReadKey().KeyChar.ToString(), out input);
                else
                    isInputCorrect = int.TryParse(Console.ReadLine(), out input);

                if (isInputCorrect && (input < min || input > max))
                    isInputCorrect = false;

                if (!isInputCorrect)
                    if (isOneDigit)
                        try
                        {
                            Console.CursorLeft--;//Если ввели что-то не то, то просто возвращаем курсор на прежнее место
                        }
                        catch
                        {
                            //В случае ошибки, ввода каких-либо управляющих символов или попытках выхода курсора
                            //за пределы консоли, просто ничего не делаем и остаемся на месте
                        }
                    else
                    {
                        Console.WriteLine(errors[Errors.RepeatInputError]);
                        try
                        {
                            Console.CursorLeft = 0;//Если ввели что-то не то, то просто возвращаем курсор на прежнее место
                            Console.CursorTop -= 2;
                            Console.Write(messages[Messages.WhiteSpaceLine]);
                            Console.CursorLeft = 0;
                        }
                        catch
                        {
                            //В случае ошибки, ввода каких-либо управляющих символов или попытках выхода курсора
                            //за пределы консоли, просто ничего не делаем и остаемся на месте
                        }
                    }
            }
            Console.WriteLine();
            return input;
        }


        /// <summary> Выводит на экран сообщение и ждет нажатия любой клавиши </summary>
        /// <param name="message">Сообщение для пользователя</param>
        private static void MessageWaitKey(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine(messages[Messages.PressAnyKey]);
            Console.ReadKey();
        }
        #endregion

    }
}