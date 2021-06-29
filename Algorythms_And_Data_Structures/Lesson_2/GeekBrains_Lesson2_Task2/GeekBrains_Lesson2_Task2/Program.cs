using System;
using System.Collections.Generic;

namespace GeekBrains_Lesson2_Task2
{
    class Program
    {
        #region -------- Поля класса --------
        #region -- Строковые константы --

        /// <summary> Ключи для словаря аргументов командной строки </summary>
        enum Arguments
        {
            Help,
            Lines,
            LinesHelp
        }

        /// <summary> Словарь аргументов командной строки </summary>
        private static readonly Dictionary<Arguments, string> arguments = new Dictionary<Arguments, string>
        {
        { Arguments.Help, "-h"},
        { Arguments.Lines, "-l"},
        { Arguments.LinesHelp, "-l <int> - number of processes to show on screen. 0 - to show all"},
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
            EnterNumberToFind,
            PressAnyKey,
            From,
            To,
            NumbersList,
            Amount,
            Element,
            AtPosition

        }

        /// <summary> Словарь с сообщениями для пользователя </summary>
        private static readonly Dictionary<Messages, string> messages = new Dictionary<Messages, string>
        {
        { Messages.ChooseOption, "Выберите опцию:"},
        { Messages.EnterNumberToFind, "Введите число которое необходимо найти."},
        { Messages.PressAnyKey, "Нажмите любую клавишу."},
        { Messages.From, "от"},
        { Messages.To, "до"},
        { Messages.NumbersList, "Список элементов массива"},
        { Messages.Amount, "всего"},
        { Messages.Element, "Элемент "},
        { Messages.AtPosition, " находится в массиве под индексом: "}
        };

        /// <summary> Пункты главного меню, последний пункт выход из программы </summary>
        private static readonly string[] mainMenu = new string[]
        {
            "Строка вверх",
            "Строка вниз",
            "Страница вверх",
            "Страница вниз",
            "Найти индекс элемента по значению",
            "Выход"
        };

        #endregion

        #region -- Числовые константы --

        /// <summary> Количество строк из массива, одновременно отображающихся на экране </summary>
        private static readonly int LINES_ON_SCREEN = 10;
        /// <summary>Минимально возможное значение элемента массива</summary>
        private static readonly int MIN_NUMBER = 1;
        /// <summary>Разброс между случайными числами в массиве</summary>
        private static readonly int DELTA = 3;
        /// <summary>Количество элементов в массиве</summary>
        private static readonly int ELEMENTS = 1000;
        /// <summary> Длина поля с индексом элемента массива в таблице выводимой на экран</summary>
        private const int INDEX_FIELD_LENGTH = 3;
        /// <summary> Длина поля с элементом массива в таблице выводимой на экран</summary>
        private const int NUMBER_FIELD_LENGTH = 6;

        #endregion
        #endregion


        #region -------- Основной цикл программы --------
        static int Main(string[] args)
        {
            int lines = LINES_ON_SCREEN;//Количество процессво отображаемых на экране, далее может быть скорректировано.


            //Обработка аругментов командной строки
            if (args.Length != 0)
            {
                if (args[0] == arguments[Arguments.Help])//Вывод справки по аргументам
                {
                    Console.WriteLine(arguments[Arguments.LinesHelp]);
                    return 0;
                }
                else if (args[0] == arguments[Arguments.Lines])//Изменение количества процессов выводимых на экран
                {
                    try
                    {
                        int.TryParse(args[1], out lines);
                        if (lines < 0) lines = 0;
                    }
                    catch //Если аргумент не введен или введен неправильно, то устанавливаем количество строк максимальным
                    {
                        lines = 0;
                    }
                }
            }

            //Формируем сообщение главного меню
            string mainMenuMessage = messages[Messages.ChooseOption] + "\n";
            for (int i = 0; i < mainMenu.Length; i++)
                mainMenuMessage += $"{i + 1} - {mainMenu[i]}\n";

            //Создаем массив для работы
            int[] array = new int[ELEMENTS];
            Random rnd = new Random();

            //Заполняем массив случайными, но упорядоченными числами
            int number = MIN_NUMBER + rnd.Next(DELTA);
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = number;
                number += rnd.Next(DELTA) + 1;
            }

            int showIndex = 0;
            ScreenOutput(array, showIndex, lines);

            //Основной цикл
            bool isExit = false;
            while (!isExit)
            {
                int input = NumberInput(mainMenuMessage, 1, mainMenu.Length);
                switch (input)
                {
                    case 1://line up
                        showIndex--;
                        showIndex = CheckSetLimits(showIndex, array.Length, lines);
                        break;
                    case 2://line down
                        showIndex++;
                        showIndex = CheckSetLimits(showIndex, array.Length, lines);
                        break;
                    case 3://page up
                        showIndex -= lines;
                        showIndex = CheckSetLimits(showIndex, array.Length, lines);
                        break;
                    case 4://page down
                        showIndex += lines;
                        showIndex = CheckSetLimits(showIndex, array.Length, lines);
                        break;
                    case 5://refrsh process list
                        ScreenOutput(array, showIndex, lines);
                        int index = 0;//Индекс массива
                        int answer = NumberInput(messages[Messages.EnterNumberToFind], MIN_NUMBER - 1, int.MaxValue, false);
                        index = BinarySearch(answer, array);
                        if (index == -1)
                            Console.WriteLine(errors[Errors.ItemNotFound]);
                        else
                        {
                            Console.WriteLine(messages[Messages.Element] + answer + messages[Messages.AtPosition] + index);
                            showIndex = index;
                        }
                        MessageWaitKey(string.Empty);
                        break;
                    case 6://exit
                        isExit = true;
                        break;
                }
                ScreenOutput(array, showIndex, lines);
            }

            return 0;
        }

        #endregion


        #region -------- Бинарный поиск --------

        /// <summary>Бинарный поиск в массиве </summary>
        /// <param name="SearchValue">Значение для поиска</param>
        /// <param name="array">Массив в котором ищется значение</param>
        /// <returns>Индекс найденного элемента, либо -1 если элемент не найден</returns>
        private static int BinarySearch(int SearchValue, int[] array)
        {
            int head = 0; //Начальный индекс границы поиска
            int tail = array.Length - 1; //Конечный индекс границы поиска
            int index = -1; //Возвращаемый индекс

            bool isSearchEnd = false; //Флаг окончания поиска
            //Основной цикл проверки, будет выполнятся O(logN) раз, 
            //все что за его пределами, константные операции и могут быть отброшены.
            while (!isSearchEnd && head <= tail) 
            {
                int mid = (tail + head) / 2; //Взяли серединку
                if (array[mid] == SearchValue) //Если значение совпадает, то элемент найден
                {
                    isSearchEnd = true; //Поиск закончен
                    index = mid; //Нужный индекс
                }
                else if (array[mid] > SearchValue) //Если не найден, то сдвигаем одну из границ в зависимости от значения текущего элемента
                    tail = mid - 1;
                else
                    head = mid + 1;
            }
            return index;

        }
        #endregion


        #region -------- Вспомогательные методы --------

        /// <summary>
        /// Выводит на экран список процессов с очисткой консоли
        /// </summary>
        /// <param name="array">Массив содержащий список процессов</param>
        /// <param name="number">Идекс масива с которого надо выводить список на экран</param>
        /// <param name="lines">Количество строк отображаемых на экране, 0 - если все</param>
        private static void ScreenOutput(int[] array, int number, int lines)
        {
            if (lines == 0 || lines > array.Length) lines = array.Length;
         
            //number--;
            if (number < 0) number = 0;
            if (number + lines > array.Length) number = array.Length - lines;

            //Рассчет позиции для отметки на скроллбаре
            int scrollPosition = (number * lines / (array.Length - lines + 1));
                        
            Console.Clear();
            Console.WriteLine($"{messages[Messages.NumbersList]} ({messages[Messages.Amount]} {array.Length})");
            Console.WriteLine(" Index  Number");
            Console.WriteLine("+-----+--------+");
            for (int i = number; i < number + lines; i++)
            {

                Console.Write(string.Format($"" +
                    $"| {i,INDEX_FIELD_LENGTH:d} " +
                    $"| {array[i],NUMBER_FIELD_LENGTH} " +
                    (scrollPosition == i - number ? '#' : '|') +  "\n"));
            }
            Console.WriteLine("+-----+--------+");
        }

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
                        Console.WriteLine(errors[Errors.RepeatInputError]);
            }
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


        /// <summary>
        /// Проверка индекса на вхождение в число количества процессов
        /// с учетом количества процессов отображаемых на экране
        /// </summary>
        /// <param name="number">Индекс массива процессов</param>
        /// <param name="length">Длина массива процессов</param>
        /// <param name="lines">Количество строк списка отображаемых на экране</param>
        /// <returns>Скорректированный индекс входящий в нужный интервал</returns>
        private static int CheckSetLimits(int number, int length, int lines)
        {
            if (number < 0) number = 0;
            if (number + lines > length) number = length - lines;
            return number;
        }

        #endregion

    }

}