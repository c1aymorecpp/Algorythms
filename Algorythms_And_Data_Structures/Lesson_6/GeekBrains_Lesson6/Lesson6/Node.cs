using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_06_01
{
    /// <summary>Вершина графа</summary>
    public class Node
    {
        #region ---- FIELDS & PROERTIES ----

        /// <summary>Состояния вершины графа</summary>
        public enum Status : int
        {
            processed = (int)ConsoleColor.Red,
            not_processed = (int)ConsoleColor.Gray,
            in_process = (int)ConsoleColor.Magenta,
            marked_to_process = (int)ConsoleColor.Yellow,
            founded = (int)ConsoleColor.Green
        }

        /// <summary>Граф к которому принадлежит вершина</summary>
        public Graph GraphParent { get; }

        /// <summary>ID вершины</summary>
        public int ID { get; }

        /// <summary>Список ребер</summary>
        public List<Edge> Edges { get; }

        /// <summary>Состояние вершины графа</summary>
        public Status State
        {
            get;
            internal set;
        }

        #endregion

        #region ---- CONSTRUCTORS ----

        /// <summary>Конструктор</summary>
        /// <param name="id">ID вершины</param>
        public Node(int id, Graph graph)
        {
            ID = id;
            Edges = new List<Edge>();
            GraphParent = graph;
            State = Status.not_processed;
        }

        #endregion

        #region ---- WORK WITH EDGES ----

        /// <summary>Добавить ребро</summary>
        /// <param name="node">Вершина</param>
        /// <param name="weight">Вес</param>
        public void AddEdge(Node node, int weight)
        {
            AddEdge(new Edge(node, weight));
        }

        /// <summary>Добавляет ребро</summary>
        /// <param name="newEdge">Ребро</param>
        private void AddEdge(Edge newEdge)
        {
            Edges.Add(newEdge);
        }

        /// <summary>Проверяет есть ли у данной вершины связь с вершиной с указанным ID
        /// Нужен для избегания дублирования ребер</summary>
        /// <param name="id">ID вершины связь с которой нужно проверить</param>
        /// <returns>true, если связь есть</returns>
        public bool CheckLinkToNode(int id)
        {
            foreach (Edge edge in Edges)
            {
                if (edge.ConnectedNode.ID == id)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}