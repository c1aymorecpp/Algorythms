using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_06_01
{
    class Program
    {
        #region -- STRING CONSTS --

        /// <summary> Ключи для словаря аргументов командной строки </summary>
        enum Arguments
        {
            Help,
            Seed,
            Elements,
            Delay,
            HelpText
        }

        /// <summary> Словарь аргументов командной строки </summary>
        private static readonly Dictionary<Arguments, string> arguments = new Dictionary<Arguments, string>
        {
        { Arguments.Help, "-h"},
        { Arguments.Seed, "-s"},
        { Arguments.Elements, "-e"},
        { Arguments.Delay, "-d"},
        { Arguments.HelpText,
                "-s <int> - seed for random number generator\n" +
                "-e <int> - max number of graph nodes\n" +
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
            NumbersList,
            Amount,
            Contain,
            NotContain,
            WhiteSpaceLine
        }

        /// <summary> Словарь с сообщениями для пользователя </summary>
        private static readonly Dictionary<Messages, string> messages = new Dictionary<Messages, string>
        {
        { Messages.ChooseOption, "Выберите опцию:"},
        { Messages.EnterNumber, "Введите число: "},
        { Messages.PressAnyKey, "Нажмите любую клавишу."},
        { Messages.From, "от"},
        { Messages.To, "до"},
        { Messages.NumbersList, "Содержимое графа"},
        { Messages.Amount, "всего"},
        { Messages.Contain, "Вершина присутствует в графе."},
        { Messages.NotContain, "Такой вершины нет в графе."},
        { Messages.WhiteSpaceLine, "        "}
        };

        /// <summary> Пункты главного меню, последний пункт выход из программы </summary>
        private static readonly string[] mainMenu = new string[]
        {
            "Поиск в ширину",
            "Поиск в глубину\n",
            "Работа с графом\n",
            "Выход"
        };

        /// <summary> Пункты главного меню, последний пункт выход из программы </summary>
        private static readonly string[] graphMenu = new string[]
        {
            "Добавить ребро",
            "Удалить ребро",
            "Поменять вершины местами",
            "Передвинуть вершину",
            "Создать случайный граф",
            "Вернуть стандартный граф",
            "Выход"
        };

        /// <summary> Пункты меню передвижения, последний пункт возврат </summary>
        private static readonly string[] moveMenu = new string[]
        {
            "Влево",
            "Вправо",
            "Вниз",
            "Вверх",
            "Выбрать вершину",
            "Выход"
        };

        #endregion

        #region -- NUMERIC CONSTS --

        /// <summary>Задержка отрисовки по умолчанию</summary>
        public const int DELAY = 500;

        /// <summary>Ширина окна консоли</summary>
        private const int CONSOLE_WINDOW_W = 88;
        /// <summary>Высота окна консоли</summary>
        private const int CONSOLE_WINDOW_H = 32;

        /// <summary>Минимальное значение числа хранимого в узле дерева</summary>
        private const int VALUE_MIN = 0;
        /// <summary>Максимальное значение числа хранимого в узле дерева</summary>
        private const int VALUE_MAX = 99;
        /// <summary>Количество узлов в дереве (для первоначального случайного заполнения)</summary>
        private const int ELEMENTS = 8;
        /// <summary>Максимальный вес ребра в графе</summary>
        private const int MAX_WEIGHT = 5;


        #endregion

        #region ---- FIELDS & PROPERTIES ----
        /// <summary>Генератор случайных чисел</summary>
        private static Random rnd;
        /// <summary>seed для генератора случайных чисел</summary>
        private static int seed = 0;

        /// <summary>Количество элементов в графе</summary>
        private static int elements = ELEMENTS;

        /// <summary>Задержка для визуализации алгоритма</summary>
        private static int delay = DELAY;


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
                        else if (args[i] == arguments[Arguments.Elements])//Изменение количества элементов
                        {
                            try
                            {
                                int.TryParse(args[i + 1], out elements);
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

            #region ---- GRAPH CREATIONS ----
            //Создаем граф
            Graph graph = new Graph();
            graph = CreateGraph(false, elements);

            #endregion

            #region ---- MAIN WORK CYCLE ----

            MainMenu(graph, mainMenu);

            #endregion

            return 0;
        }

        #region ---- WORK WITH GRAPH ----

        /// <summary>Вызов печати графаы</summary>
        /// <param name="graph"></param>
        /// <param name="isColorsClear"></param>
        private static void Print(Graph graph, bool isColorsClear = true)
        {
            Console.Clear();
            GraphPrinter.ClearText();
            GraphPrinter.Print(graph, isColorsClear);

        }

        /// <summary>Создает новый граф</summary>
        /// <param name="isRandom">true - случайный, false - стандартный (зарадкоженый)</param>
        /// <param name="elements">количество элементов в графе</param>
        /// <returns>Созданный граф</returns>
        private static Graph CreateGraph(bool isRandom, int elements = ELEMENTS)
        {
            Graph newGraph = new Graph();
            if (isRandom)
                newGraph = CreateRandomGraph(elements);
            else
                newGraph = CreatePredefinedGraph();

            newGraph.Delay = delay;

            return newGraph;
        }

        /// <summary>Создает случайный граф</summary>
        /// <param name="elements">Количество элементов</param>
        /// <returns>Созданный граф</returns>
        private static Graph CreateRandomGraph(int elements = ELEMENTS)
        {
            Graph newGraph = new Graph();

            //Добавляем в граф вершины
            for (int i = 0; i < elements; i++)
            {
                newGraph.AddNode(i);
            }

            GraphPrinter.CreateGraphInfo(newGraph);

            //Добавляем связи между вершинами
            for (int i = 0; i < elements; i++)
            {
                for (int j = 0; j < rnd.Next(1) + 2; j++)
                {
                    bool isGenerated = false;
                    while (!isGenerated)
                    {
                        int toNode = rnd.Next(elements);
                        if (toNode != i)
                            isGenerated = newGraph.AddEdge(i, toNode, rnd.Next(MAX_WEIGHT));
                        if (newGraph.Nodes[i].Edges.Count >= 2) isGenerated = true;
                    }
                }
            }
            return newGraph;
        }

        /// <summary>Создает заранее зарадкоженый граф (для наглядности ДЗ)</summary>
        /// <returns>Созданный граф</returns>
        private static Graph CreatePredefinedGraph()
        {
            const int ELEMENTS_PREDEF = 15;

            Graph newGraph = new Graph();

            //Добавляем в граф вершины
            for (int i = 0; i < ELEMENTS_PREDEF; i++)
            {
                newGraph.AddNode(i);
            }

            //Предварительное создание массива координат вершин
            GraphPrinter.CreateGraphInfo(newGraph);

            //Расставляем вершины так как нам нужно
            int[,] coords = {
                { 32, 0 }, { 16, 3 }, { 32, 4 }, { 48, 2 }, { 6, 7 }, { 24, 7 }, { 36, 8 }, { 54, 7 },
                { 7, 11 }, { 22, 12 }, { 36, 12 }, { 54, 12 }, { 18, 15 }, { 30, 15 }, { 34, 19 }};

            for (int i = 0; i < ELEMENTS_PREDEF; i++)
            {
                GraphPrinter.MoveNode(i, coords[i, 0], coords[i, 1], false);
            }

            //Добавляем ребра
            int[,] edges = {
                { 0, 1 }, { 0, 2 }, { 0, 3 }, { 1, 4 }, { 1, 5 }, { 2, 6 }, { 3, 6 }, { 3, 7 },
                { 4, 8 }, { 5, 9 }, { 5, 10 }, { 6, 10 }, { 6, 11 }, { 7, 11 }, { 8, 9 }, { 8, 12 },
                { 9, 13 }, { 10, 13 }, { 11, 14 }, { 13, 14 }, { 12, 14 }};

            for (int i = 0; i < edges.Length / 2; i++)
            {
                newGraph.AddEdge(edges[i, 0], edges[i, 1], rnd.Next(MAX_WEIGHT));
            }

            return newGraph;
        }

        #endregion

        #region ---- MENUS ----

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
        /// <param name="graph">Граф</param>
        /// <param name="menu">Содержание меню</param>
        private static void MainMenu(Graph graph, string[] menu)
        {
            string menuMessage = MenuMessage(menu);
            Print(graph);

            bool isExit = false;
            while (!isExit)
            {
                Print(graph);
                int input = NumberInput(menuMessage, 0, menu.Length - 1);
                switch (input)
                {
                    case 1://BFS
                        Print(graph);
                        bool isContain = graph.BFS(NumberInput(messages[Messages.EnterNumber], VALUE_MIN, VALUE_MAX, false));
                        Console.ForegroundColor = isContain ? ConsoleColor.Green : ConsoleColor.Red;
                        MessageWaitKey(isContain ? messages[Messages.Contain] : messages[Messages.NotContain]);
                        Print(graph);
                        break;
                    case 2://DFS
                        Print(graph);
                        isContain = graph.DFS(NumberInput(messages[Messages.EnterNumber], VALUE_MIN, VALUE_MAX, false));
                        Console.ForegroundColor = isContain ? ConsoleColor.Green : ConsoleColor.Red;
                        MessageWaitKey(isContain ? messages[Messages.Contain] : messages[Messages.NotContain]);
                        Print(graph);
                        break;
                    case 3://graph menu
                        Print(graph);
                        graph = GraphMenu(graph, graphMenu);
                        break;
                    case 0://exit
                        isExit = true;
                        break;
                }
            }
        }


        /// <summary>Меню работы с графом</summary>
        /// <param name="graph">Граф</param>
        /// <param name="menu">Содержание меню</param>
        private static Graph GraphMenu(Graph graph, string[] menu)
        {
            string menuMessage = MenuMessage(menu);
            Print(graph);
            bool isExit = false;
            while (!isExit)
            {
                int input = NumberInput(menuMessage, 0, menu.Length - 1);
                switch (input)
                {
                    case 1://add edge between two nodes
                        Print(graph);
                        int firstID = NumberInput(messages[Messages.EnterNumber], 0, graph.Count - 1, false);
                        int secondID = NumberInput(messages[Messages.EnterNumber], 0, graph.Count - 1, false);
                        int weight = NumberInput(messages[Messages.EnterNumber], 0, MAX_WEIGHT - 1, false);
                        graph.AddEdge(firstID, secondID, weight);
                        Print(graph);
                        break;
                    case 2://add edge between two nodes
                        Print(graph);
                        firstID = NumberInput(messages[Messages.EnterNumber], 0, graph.Count - 1, false);
                        secondID = NumberInput(messages[Messages.EnterNumber], 0, graph.Count - 1, false);
                        graph.RemoveEdge(firstID, secondID);
                        Print(graph);
                        break;
                    case 3://change two nodes
                        Print(graph);
                        firstID = NumberInput(messages[Messages.EnterNumber], 0, graph.Count - 1, false);
                        secondID = NumberInput(messages[Messages.EnterNumber], 0, graph.Count - 1, false);
                        GraphPrinter.ChangeTwoNodes(firstID, secondID);
                        Print(graph);
                        break;
                    case 4://move one node
                        Print(graph);
                        MoveMenu(graph, moveMenu);
                        break;
                    case 5://create random graph
                        Print(graph);
                        graph = CreateGraph(true, elements);
                        break;
                    case 6://return to default graph
                        Print(graph);
                        graph = CreateGraph(false);
                        break;
                    case 0://exit
                        isExit = true;
                        break;
                }
                Console.Clear();
                Print(graph);
            }
            return graph;

        }


        /// <summary>Меню передвижения вершины</summary>
        /// <param name="graph">Граф</param>
        /// <param name="menu">Содержание меню</param>
        private static void MoveMenu(Graph graph, string[] menu)
        {
            string menuMessage = MenuMessage(menu);
            int id = NumberInput(messages[Messages.EnterNumber], 0, graph.Count - 1, false);
            graph.FindNode(id).State = Node.Status.marked_to_process;
            Print(graph, false);
            bool isExit = false;
            while (!isExit)
            {
                int input = NumberInput(menuMessage, 0, menu.Length - 1);
                switch (input)
                {
                    case 1://Left
                        GraphPrinter.MoveNode(id, -1, 0);
                        break;
                    case 2://Right
                        GraphPrinter.MoveNode(id, 1, 0);
                        break;
                    case 3://Down
                        GraphPrinter.MoveNode(id, 0, 1);
                        break;
                    case 4://Up
                        GraphPrinter.MoveNode(id, 0, -1);
                        break;
                    case 5://change node
                        Print(graph);
                        id = NumberInput(messages[Messages.EnterNumber], 0, graph.Count - 1, false);
                        graph.FindNode(id).State = Node.Status.marked_to_process;
                        break;
                    case 0://exit
                        isExit = true;
                        break;
                }
                Console.Clear();
                Print(graph, false);
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