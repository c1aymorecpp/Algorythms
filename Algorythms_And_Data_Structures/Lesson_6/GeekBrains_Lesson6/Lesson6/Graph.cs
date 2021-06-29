using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_06_01
{
    /// <summary>Граф</summary>
    public class Graph
    {
        #region ---- FIELDS & PROPERTIES ----
        /// <summary>Список вершин графа</summary>
        internal List<Node> Nodes { get; }

        /// <summary>Количество вершин в графе</summary>
        public int Count
        {
            get
            {
                return Nodes.Count;
            }
        }

        /// <summary>Отладочное поле, задержка для визуализации алгоритма поиска</summary>
        public int Delay
        {
            private get;
            set;
        }

        #endregion

        #region ---- CONSTRUCTORS ----

        /// <summary>Конструктор</summary>
        public Graph()
        {
            Nodes = new List<Node>();
            Delay = 0;
        }

        #endregion

        #region ---- WORK WITH NODES ----

        /// <summary>Добавление вершины</summary>
        /// <param name="id">ID вершины</param>
        public void AddNode(int id)
        {
            if (FindNode(id) == null)
                Nodes.Add(new Node(id, this));
        }

        #endregion

        #region ---- WORK WITH EDGES ----

        /// <summary>Добавление ребра</summary>
        /// <param name="firstID">Имя первой вершины</param>
        /// <param name="secondID">Имя второй вершины</param>
        /// <param name="weight">Вес ребра соединяющего вершины</param>
        /// <returns>true, если добавление прошло успешно</returns>
        public bool AddEdge(int firstID, int secondID, int weight)
        {
            Node firstNode = FindNode(firstID);
            Node SecondNode = FindNode(secondID);
            if (SecondNode != null && firstNode != null)
                if (!firstNode.CheckLinkToNode(secondID))
                {
                    firstNode.AddEdge(SecondNode, weight);
                    SecondNode.AddEdge(firstNode, weight);
                    return true;
                }
            return false;
        }

        /// <summary>Удаляет связь между вершинами с указанными ID</summary>
        /// <param name="firstID">Имя первой вершины</param>
        /// <param name="secondID">Имя второй вершины</param>
        public void RemoveEdge(int firstID, int secondID)
        {
            Node firstNode = FindNode(firstID);
            Node secondNode = FindNode(secondID);
            if (secondNode != null && firstNode != null)
            {
                for (int i = 0; i < firstNode.Edges.Count; i++)
                {
                    if (firstNode.Edges[i].ConnectedNode == secondNode)
                        firstNode.Edges.RemoveAt(i);
                }
                for (int i = 0; i < secondNode.Edges.Count; i++)
                {
                    if (secondNode.Edges[i].ConnectedNode == firstNode)
                        secondNode.Edges.RemoveAt(i);
                }

            }

        }

        #endregion

        #region ---- SEARCH METHODS ----

        /// <summary>
        /// Поиск (обход) графа в ширину
        /// </summary>
        /// <param name="id">Искомое значение</param>
        /// <returns>true, если вершина с таким значением есть в графе</returns>
        public bool BFS(int id)
        {
            GraphPrinter.ClearText();//Очищаем текстовый буфер печатальщика

            Queue<Node> bufer = new Queue<Node>();//Сюда будут заносится вершины для последующей проверки

            bufer.Enqueue(this.Nodes[0]);//Обход начинаем с 0-й вершины

            bool isFound = false;
            //Повторяем до тех пор пока не найдем или пока не кончатся вершины
            while (bufer.Count != 0 && !isFound)
            {
                Node element = bufer.Dequeue();//Достаем элемент из очереди
                if (element.ID == id) isFound = true;//Если это нужны элемент то радуемся
                //Выставляем флаг проверки того, что элемент обрабатывается или найден
                //ПУРПУРНЫЙ - элемент в процессе обработки
                //ЗЕЛЕНЫЙ - элемент обработан и совпадает с искомым значением
                element.State = isFound ? Node.Status.founded : Node.Status.in_process;
                ColorPrint($"Проверяемая вершина: {element.ID}");

                if (!isFound)//Если не нашли значение то добавляем сопряженные вершины в очередь на проверку
                {
                    foreach (Edge edge in element.Edges)
                    {
                        if (edge.ConnectedNode.State == Node.Status.not_processed)
                        {
                            //ЖЕЛТЫЙ - элемент помещается в очередь для последующей обработки
                            edge.ConnectedNode.State = Node.Status.marked_to_process;
                            bufer.Enqueue(edge.ConnectedNode);
                            ColorPrint($"Вершина {edge.ConnectedNode.ID} идет в очередь");
                        }
                    }
                    //КРАСНЫЙ - элемент обработан
                    element.State = Node.Status.processed;
                }
            }
            ColorPrint("Поиск завершен");
            Console.WriteLine();

            return isFound;
        }

        /// <summary>
        /// Поиск (обход) графа в глубину
        /// </summary>
        /// <param name="id">Искомое значение</param>
        /// <returns>true, если вершина с таким значением есть в графе</returns>
        public bool DFS(int id)
        {
            GraphPrinter.ClearText();//Очищаем текстовый буфер печатальщика
            bool isFound = DFS(id, this.Nodes[0]);
            ColorPrint("Поиск завершен");
            Console.WriteLine();
            return isFound;
        }

        /// <summary>
        /// Основной рекурсивны метод поиска (обхода) графа в глубину от заданной вершины
        /// </summary>
        /// <param name="id">Искомая вершина</param>
        /// <param name="element">Вершина от которой начинается поиск</param>
        /// <returns>true, если вершина с таким значением есть в графе</returns>
        private bool DFS(int id, Node element)
        {
            bool isFound = false;

            //Выставляем флаг проверки того, что элемент обрабатывается
            //ПУРПУРНЫЙ - элемент в процессе обработки
            element.State = Node.Status.in_process;

            if (element.ID != id)//Если не нашли значение то идем к следующему необработанному элементу
            {
                foreach (Edge edge in element.Edges)
                {
                    if (edge.ConnectedNode.State == Node.Status.not_processed)
                    {
                        ColorPrint($"Из вершины {element.ID} в {edge.ConnectedNode.ID}");
                        isFound = DFS(id, edge.ConnectedNode);
                        if (isFound) break;
                    }

                }
                if (!isFound)//Если элемент не найден, элемент помечается, как уже обработанный
                {
                    //КРАСНЫЙ - элемент уже обработан
                    ColorPrint("Шаг назад");
                    element.State = Node.Status.processed;
                }
            }
            else//Если это нужны элемент то радуемся
            {
                isFound = true;
                //ЗЕЛЕНЫЙ - элемент проверен и совпадает с искомым значением
                element.State = Node.Status.founded;
            }

            return isFound;
        }

        /// <summary>Неоптимизированный поиск вершины обычным перебором
        /// Добавлен в целях облегчения визуализации и работы с графом на учебном проекте</summary>
        /// <param name="id">ID вершины</param>
        /// <returns>Найденная вершина</returns>
        public Node FindNode(int id)
        {
            foreach (Node node in Nodes)
            {
                if (node.ID.Equals(id))
                {
                    return node;
                }
            }
            return null;
        }

        #endregion

        #region ---- PRINT METHODS ----

        /// <summary>Вывод дерева на экран с раскраской вершин
        /// Используется для визуализации алгоритмов обхода графа</summary>
        /// <param name="line">Строка идущая в текстовый буфер</param>
        private void ColorPrint(string line = null)
        {
            Console.Clear();
            if (line != null)
                GraphPrinter.AddText(line);
            GraphPrinter.Print(this, false);
            System.Threading.Thread.Sleep(Delay);//небольшая задержка для наглядности работы алгоритма
        }

        #endregion
    }
}