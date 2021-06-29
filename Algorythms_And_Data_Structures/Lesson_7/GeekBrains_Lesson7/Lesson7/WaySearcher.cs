using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson7
{

    /// <summary>
    /// Таблица для которой рассчитывается путь из левого-верхнего угла, в правый-нижний угол.
    /// </summary>
    class WaySearcher
    {

        #region ---- FIELDS & PROPERTIES ----

        /// <summary>Минимальная ширина поля</summary>
        public const int MIN_WIDTH = 2;
        /// <summary>Минимальная высота поля</summary>
        public const int MIN_HEIGHT = 2;
        /// <summary>Максимальная ширина поля</summary>
        public const int MAX_WIDTH = 9;
        /// <summary>Максимальная высота поля</summary>
        public const int MAX_HEIGHT = 9;

        /// <summary>Длина ячейки таблицы при печати</summary>
        public const int CELL_WIDTH = 5;

        /// <summary>Поле</summary>
        private int[,] field;

        /// <summary>Измерение массива поля для столбцоы</summary>
        private const int COLUMNS = 0;
        /// <summary>Измерение массива поля для рядов</summary>
        private const int ROWS = 1;

        /// <summary>Значение ячейки с препятствием</summary>
        private const int OBSTACLE = -1;

        /// <summary>Ширина поля</summary>
        public int Width
        {
            get
            {
                return field.GetLength(COLUMNS);
            }
        }

        /// <summary>Высота поля</summary>
        public int Height
        {
            get
            {
                return field.GetLength(ROWS);
            }
        }

        /// <summary>Задержка для визуализации алгоритма</summary>
        public int Delay { get; set; }

        #endregion

        #region ---- CONSTRUCTORS ----

        /// <summary>Конструктор</summary>
        /// <param name="width">Ширина поля</param>
        /// <param name="height">Высота поля</param>
        public WaySearcher(int width, int height)
        {
            field = new int[width, height];
            Delay = 0;
        }

        #endregion

        #region ---- WAY SEARCH METHODS ----

        /// <summary>Поиск количества путей ведущих в правую-нижнюю клетку без рекурсии</summary>
        public void SearchWays()
        {
            //int left = 0;
            field[0, 0] = 1;//Начальная клетка

            //Заполняем первый ряд
            for (int c = 1; c < Width; c++)
            {
                if (field[c, 0] != OBSTACLE)//Если в данной клетке нет препятствий
                {
                    //Значение слева от устанавливаемой клетки. Если там препятствие, то берем 0,
                    //иначе значение которое там хранится (будет либо 0, либо уже посчитанное количество путей)
                    int left = ((field[c - 1, 0] != OBSTACLE) ? field[c - 1, 0] : 0);
                    field[c, 0] = left;
                }
            }

            //Заполняем последующие ряды
            for (int r = 1; r < Height; r++)
            {
                //Заполнение первого столбца
                if (field[0, r] != OBSTACLE)
                {
                    //Значение сверху от устанавливаемой клетки (по аналогии с первым рядом)
                    int up = ((field[0, r - 1] != OBSTACLE) ? field[0, r - 1] : 0);
                    field[0, r] = up;
                }

                //Заполнение остальных столбцов
                for (int c = 1; c < Width; c++)
                {
                    if (field[c, r] != OBSTACLE)
                    {
                        //По аналогии с первым рядом, берутся значения из левого и верхнего соседа клетки
                        int left = ((field[c - 1, r] != OBSTACLE) ? field[c - 1, r] : 0);//Значение слева от устанавливаемой клетки
                        int up = ((field[c, r - 1] != OBSTACLE) ? field[c, r - 1] : 0);//Значение сверху от устанавливаемой клетки
                        field[c, r] = left + up;
                    }

                }
            }

        }


        /// <summary>Поиск количества путей ведущих в правую-нижнюю клетку рекурсивно</summary>
        public void SearchWaysRecurrent()
        {
            field[0, 0] = 1;//Начальная точка левый-верхний угол
            CellSearchWaysRecurrent(Width - 1, Height - 1);
        }

        /// <summary>Возвращает количество путей ведущих в клетку слева и сверху
        /// Если количество путей для клетки не посчитано, то производится рекурсивный подсчет
        /// с учетом соседей слева и сверху</summary>
        /// <param name="column">столбец в таблице</param>
        /// <param name="row">ряд в таблице</param>
        /// <returns>Количество путей ведущих в клетку слева и сверху</returns>
        private int CellSearchWaysRecurrent(int column, int row)
        {

            if (field[column, row] == OBSTACLE) return 0;
            if (field[column, row] == 0)
            {
                int up = (row != 0) ? CellSearchWaysRecurrent(column, row - 1) : 0;
                int left = (column != 0) ? CellSearchWaysRecurrent(column - 1, row) : 0;
                field[column, row] = up + left;
            }

            // ! DEBUG задержка для визуализации рекуррентного алгоритма
            if (Delay != 0)
            {
                PrintField();
                Console.SetCursorPosition(0, Height * 2 + 1);
                Console.Write($"Обрабатывается клетка: {column}, {row}          ");
                System.Threading.Thread.Sleep(Delay);
            }

            return field[column, row];
        }


        #endregion

        #region ---- FIELD MANAGING ----

        /// <summary>Ставит в указанную точку препятствие</summary>
        /// <param name="row">Номер строки</param>
        /// <param name="column">Номер столбца</param>
        /// <returns>
        /// true, если удалось добавить препятствие
        /// false, если в клетке уже есть препятствие или координаты выходят за пределы клетки</returns>
        public bool AddObstacle(int column, int row)
        {
            //проверяем координаты на правильность
            if (column < 0 || Width <= column) return false;
            if (row < 0 || Height <= row) return false;
            //Запрет на добавление препятствий в левый-верхний и правый-нижний углы
            if (column == 0 && row == 0) return false;
            if (column == Width - 1 && row == Height - 1) return false;

            //проверка на наличие препятсвия в заданной клетке
            if (field[column, row] == OBSTACLE) return false;

            //добавляем препятствие
            field[column, row] = OBSTACLE;
            ClearField(false, column, row);
            return true;
        }

        /// <summary>Очищает поле</summary>
        /// <param name="clearObstacles">true, если дополнительно нужно удалить препятствия с поля</param>
        /// <param name="startColumn">начальный столбец</param>
        /// <param name="startRow">начальный ряд</param>
        public void ClearField(bool clearObstacles = false, int startColumn = 0, int startRow = 0)
        {
            for (int r = startRow; r < Height; r++)
            {
                for (int c = startColumn; c < Width; c++)
                {
                    if (clearObstacles)
                        field[c, r] = 0;
                    else if (field[c, r] != OBSTACLE)
                        field[c, r] = 0;
                }
            }
        }


        #endregion

        #region ---- PRINT METHODS ----

        /// <summary>Печать поля окна в консоли</summary>
        /// <param name="clearCells">Очищать ли поле</param>
        public void PrintField()
        {

            //╔ ═ ╗ ╚ ║ ╝ ╠ ╣ ╦ ╩ ╬ █ - символы для рамки поля
            //╟ ╢ ╤ ╧ ┼ ─ │
            Console.Clear();

            for (int r = 0; r < Height; r++)
            {

                for (int c = 0; c < Width; c++)
                {
                    Console.SetCursorPosition(c * (CELL_WIDTH + 1), r * 2);
                    if (r == 0)
                    {
                        Console.Write(c == 0 ? "╔" : "╤");
                        WriteLine('═', CELL_WIDTH);
                        Console.Write(c == Width - 1 ? "╗" : "╤");
                    }
                    else
                    {
                        Console.Write(c == 0 ? "╟" : "┼");
                        WriteLine('─', CELL_WIDTH);
                        Console.Write(c == Width - 1 ? "╢" : "┼");
                    }
                    Console.SetCursorPosition(c * (CELL_WIDTH + 1), r * 2 + 1);
                    Console.Write(c == 0 ? "║" : "│");

                    if (field[c, r] == OBSTACLE)
                        WriteLine('█', CELL_WIDTH);
                    else if (field[c, r] == 0)
                        WriteLine(' ', CELL_WIDTH);
                    else
                        Console.Write($"{ field[c, r],CELL_WIDTH}");
                }
                Console.Write("║");
            }

            for (int c = 0; c < Width; c++)
            {
                Console.SetCursorPosition(c * (CELL_WIDTH + 1), Height * 2);
                Console.Write(c == 0 ? "╚" : "╧");
                WriteLine('═', CELL_WIDTH);
                Console.Write(c == Width - 1 ? "╝" : "╧");
            }

            Console.SetCursorPosition(0, Height * 2);
        }

        /// <summary>Выводит на экран последовательность символов</summary>
        /// <param name="symbol">Выводимый символ</param>
        /// <param name="length">Количество</param>
        private void WriteLine(char symbol, int length)
        {
            for (int i = 0; i < length; i++)
            {
                Console.Write(symbol);
            }
        }

        #endregion

    }


}