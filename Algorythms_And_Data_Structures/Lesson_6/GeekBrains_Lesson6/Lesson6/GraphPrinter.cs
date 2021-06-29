using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_06_01
{
    /// <summary>Выводит в консоль содержимое заданного графа</summary>
    public static class GraphPrinter
    {
        #region ---- NUMERIC CONSTANTS ----

        /// <summary>Задержка отрисовки по умолчанию</summary>
        public const int DELAY = 0;

        /// <summary>Ширина поля на котором рисуется граф</summary>
        private const int WIDTH = 60;
        /// <summary>Высота поля на котором рисуется граф</summary>
        private const int HEIGHT = 19;

        /// <summary>Координата X центра окружности на которой лежат вершины графа</summary>
        private static readonly int CX = WIDTH / 2;
        /// <summary>Координата Y центра окружности на которой лежат вершины графа</summary>
        private static readonly int CY = HEIGHT / 2;

        /// <summary>Коэффициент масштабирования по оси X, для контроля овальности</summary>
        private static readonly float MX = 2.5f;
        /// <summary>Коэффициент масштабирования по оси X, для контроля овальности</summary>
        private static readonly float MY = 0.9f;

        /// <summary>Радиус окружности на которой лежат вершины графа</summary>
        private const int RADIUS = 9;

        #endregion

        #region ---- FIELDS & PROPERTIES ----

        /// <summary>Массив содержащий допонительную информацию о вершинах дерева
        /// для вывода их на экран</summary>
        private static NodeInfo[] info = new NodeInfo[1];

        /// <summary>Текстовый буфер для вывода дополнительной информации</summary>
        private static List<string> text = new List<string>();

        /// <summary>Дополнительная информация об узле, для вывода его на экран</summary>
        class NodeInfo
        {
            /// <summary>Указатель на вершину</summary>
            public Node node;
            /// <summary>Текст выводимый на экран</summary>
            public string text;
            /// <summary>Координата X на экране</summary>
            public int x;
            /// <summary>Координата Y на экране</summary>
            public int y;
        }
        #endregion

        #region ---- WORK WITH NODEINFO ----
        /// <summary>Изменяет положение вершины графа для вывода на экран</summary>
        /// <param name="nodeID">ID вершины графа</param>
        /// <param name="dx">смещение по X</param>
        /// <param name="dy">смещение по Y</param>
        /// <param name="relative">
        /// true, если изменение координат отрносительное
        /// false, если изменение координат абсолютное (от левого верхного угла)</param>
        public static void MoveNode(int nodeID, int dx, int dy, bool relative = true)
        {
            //Ищем нужный узел
            int index = 0;
            for (int i = 0; i < info.Length; i++)
            {
                if (info[i].node.ID == nodeID) index = i;
            }
            //Меняем координаты
            if (relative)
            {
                info[index].x += dx;
                info[index].y += dy;
            }
            else
            {
                info[index].x = dx;
                info[index].y = dy;
            }

            //Проверка на выход за границы
            if (info[index].x < info[index].text.Length / 2) info[index].x = info[index].text.Length / 2;
            if (info[index].y < 0) info[index].y = 0;
            if (info[index].x > WIDTH - info[index].text.Length / 2) info[index].x = WIDTH - info[index].text.Length / 2;
            if (info[index].y > HEIGHT - 1) info[index].y = HEIGHT - 1;
        }

        /// <summary>Меняет местами две вершины на экране</summary>
        /// <param name="firstID">ID первой вершины</param>
        /// <param name="secondID">ID второй вершины</param>
        public static void ChangeTwoNodes(int firstID, int secondID)
        {
            int firstIndex = 0;
            int secondIndex = 0;
            //Ищем нужные вершины
            for (int i = 0; i < info.Length; i++)
            {
                if (info[i].node.ID == firstID) firstIndex = i;
                if (info[i].node.ID == secondID) secondIndex = i;
            }
            //Меняем местами координаты
            (info[firstIndex].x, info[secondIndex].x) = (info[secondIndex].x, info[firstIndex].x);
            (info[firstIndex].y, info[secondIndex].y) = (info[secondIndex].y, info[firstIndex].y);
        }

        /// <summary>Заполняет массив с дополнительной информацией о графе
        /// для печати на экране</summary>
        /// <param name="graph">Печатаемый граф</param>
        /// <param name="textFormat">Формат вывода вершин на экран</param>
        public static void CreateGraphInfo(Graph graph, string textFormat = "[00]")
        {
            info = new NodeInfo[graph.Count];

            //Рассчет положения узлов графа на экране
            double alpha = (2 * Math.PI / graph.Count);//Угловой шаг для отрисовки узлов

            for (int i = 0; i < graph.Count; i++)
            {
                int x = (int)(Math.Cos(-alpha * i - Math.PI / 2) * RADIUS * MX) + CX;//Координата X
                int y = (int)(Math.Sin(alpha * i - Math.PI / 2) * RADIUS * MY) + CY;//Координата Y
                info[i] = new NodeInfo { node = graph.Nodes[i], text = graph.Nodes[i].ID.ToString(textFormat), x = x, y = y };
            }
        }

        #endregion

        #region ---- MAIN PRINT METHODS ----

        /// <summary>Выводит граф и связи между его узлами на экран</summary>
        /// <param name="graph">Граф выводимый на экран</param>
        /// <param name="clearColors">Очищать ли цвета вершин</param>
        /// <param name="delay">Задержка вывода акран</param>
        /// <param name="textFormat">Формат вывода вершин на экран</param>
        public static void Print(Graph graph, bool clearColors = true, int delay = DELAY, string textFormat = "[00]")
        {
            if (info[0] == null) CreateGraphInfo(graph, textFormat);

            Console.ForegroundColor = ConsoleColor.Gray;
            PrintClearField();

            //Вывод на экран связей между вершинами и самих вершин
            for (int i = 0; i < info.Length; i++)
            {
                for (int j = 0; j < info[i].node.Edges.Count; j++)
                {
                    int toNode = info[i].node.Edges[j].ConnectedNode.ID;
                    ConsoleColor color = (ConsoleColor)(info[i].node.Edges[j].Weight + 9);
                    PrintLine(info[i], info[toNode], color);
                    System.Threading.Thread.Sleep(delay);//небольшая задержка для наглядности работы алгоритма
                }
            }
            PrintNodes(info, clearColors);
            PrintLegend();
            PrintText();
        }

        /// <summary>Выводит на экран подсказку о соответствии цветов вершин и ребер</summary>
        private static void PrintLegend()
        {
            Console.SetCursorPosition(CX, HEIGHT + 1);
            Console.Write("Обозначения");

            //Обозначения вершин
            Console.SetCursorPosition(CX, HEIGHT + 2);
            Console.Write("вершин:");

            ConsoleColor[] legendColors = {
                (ConsoleColor)Node.Status.not_processed,
                (ConsoleColor)Node.Status.in_process,
                (ConsoleColor)Node.Status.processed,
                (ConsoleColor)Node.Status.marked_to_process,
                (ConsoleColor)Node.Status.founded};

            string[] legendLabels = {
                "не обработана",
                "обрабатывается",
                "обработана",
                "помечена для обработки",
                "содержит нужное значение" };

            for (int i = 0; i < legendLabels.Length; i++)
            {
                Console.SetCursorPosition(CX, HEIGHT + 3 + i);
                Console.BackgroundColor = legendColors[i];
                Console.Write("   ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("-" + legendLabels[i]);
            }

            //Обозначения ребер
            Console.SetCursorPosition(CX, HEIGHT + 8);
            Console.Write("весов:");
            for (int i = 0; i < 5; i++)
            {
                Console.SetCursorPosition(CX + i * 3, HEIGHT + 9);
                ConsoleColor color = (ConsoleColor)(i + 9);
                Console.BackgroundColor = color;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write($" {i} ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.SetCursorPosition(0, HEIGHT);

        }

        /// <summary>Печатает на экране пустое поле затирая информацию</summary>
        private static void PrintClearField()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            for (int r = 0; r < HEIGHT; r++)
            {
                for (int c = 0; c < WIDTH; c++)
                {
                    Console.Write(".");
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        #endregion

        #region ---- EDGES PRINT METHODS ----

        /// <summary>Рисует связь между двумя узлами и сами узлы
        /// Рисование линии сделано по модификации алгоритма Брезенхэма</summary>
        /// <param name="info0">Первый узел</param>
        /// <param name="info1">Второй узел</param>
        /// <param name="color">Цвет узлов (не связи)</param>
        private static void PrintLine(NodeInfo info0, NodeInfo info1, ConsoleColor color = ConsoleColor.Gray)
        {
            int x0 = info0.x;
            int y0 = info0.y;
            int x1 = info1.x;
            int y1 = info1.y;
            //Меняем кординаты местами, так чтобы рисовка всегда шла сверху вниз
            if (y1 < y0)
            {
                (x0, x1) = (x1, x0);
                (y0, y1) = (y1, y0);
            }
            //Дельты координат концов отрезка
            int dx = (x1 > x0) ? (x1 - x0) : (x0 - x1);
            int dy = y1 - y0;
            //Направления приращения
            int sx = (x1 >= x0) ? (1) : (-1);
            int sy = 1;
            string symbol = "_";
            Console.ForegroundColor = color;
            if (dy < dx)//Если по горизонтали расстояние больше чем по вертикали
            {
                int d = (dy << 1) - dx;
                int d1 = dy << 1;
                int d2 = (dy - dx) << 1;
                int x = x0 + sx;
                int y = y0;
                for (int i = 1; i <= dx; i++)
                {
                    symbol = "_";
                    if (d > 0)
                    {
                        d += d2;
                        y += sy;
                        symbol = (x0 < x1) ? "\\" : "/";
                    }
                    else
                        d += d1;
                    PrintSymbol(x, y, symbol);
                    x += sx;
                }
            }
            else//Если по вертикали расстояние больше чем по горизонтали
            {
                int d = (dx << 1) - dy;
                int d1 = dx << 1;
                int d2 = (dx - dy) << 1;
                int x = x0;
                int y = y0 + sy;
                for (int i = 1; i <= dy; i++)
                {
                    symbol = "|";
                    if (d > 0)
                    {
                        d += d2;
                        x += sx;
                        symbol = (x0 < x1) ? "\\" : "/";
                    }
                    else
                        d += d1;
                    PrintSymbol(x, y, symbol);
                    y += sy;
                }
            }
            Console.ForegroundColor = ConsoleColor.Gray;

        }

        /// <summary>
        /// Печатает на экране единичный символ в заданных координатах
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="symbol"></param>
        private static void PrintSymbol(int x, int y, string symbol = "*")
        {
            Console.SetCursorPosition(x, y);
            Console.Write(symbol);
            Console.SetCursorPosition(0, HEIGHT);

        }

        #endregion

        #region ---- NODES PRINT METHODS ----

        /// <summary>
        /// Печатае на экран все вершины дерева
        /// </summary>
        /// <param name="info">Массив с информацие о вершинах графа и их координатах на экране</param>
        /// <param name="clearColors">
        /// true, если нужно очистить цвета всех вершин
        /// false, если печатать нужно с подсветкой алгоритма поиска</param>
        private static void PrintNodes(NodeInfo[] info, bool clearColors)
        {
            for (int i = 0; i < info.Length; i++)
            {
                if (clearColors) info[i].node.State = Node.Status.not_processed;
                PrintNode(info[i]);
            }

        }

        /// <summary>Печатает на экране содержимое вершины графа</summary>
        /// <param name="nodeInfo"></param>
        private static void PrintNode(NodeInfo nodeInfo)
        {
            Console.SetCursorPosition(nodeInfo.x - nodeInfo.text.Length / 2, nodeInfo.y);
            Console.ForegroundColor = (ConsoleColor)nodeInfo.node.State;
            Console.Write(nodeInfo.text);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(0, HEIGHT);
        }

        #endregion

        #region ---- WORK WITH TEXT BUFFER ----

        /// <summary>Очищает текстовый буфер</summary>
        public static void ClearText()
        {
            text.Clear();
        }

        /// <summary>Добавляет текстовую строку в текстовый буфер</summary>
        /// <param name="line"></param>
        public static void AddText(string line)
        {
            text.Add(line);
        }

        /// <summary>Выводит тектовый буфер на экран</summary>
        public static void PrintText()
        {
            for (int i = 0; i < text.Count; i++)
            {
                Console.SetCursorPosition(WIDTH, i);
                Console.WriteLine(text[i]);
                Console.SetCursorPosition(0, HEIGHT);

            }
        }

        #endregion
    }

}