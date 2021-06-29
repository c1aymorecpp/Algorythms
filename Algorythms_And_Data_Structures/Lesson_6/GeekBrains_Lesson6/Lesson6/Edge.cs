using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_06_01
{
    /// <summary>Ребро графа</summary>
    public class Edge
    {
        #region ---- FIELDS & PROPERTIES ----

        /// <summary>Вес ребра</summary>
        public int Weight { get; }

        /// <summary>Вершина с которой ребро связывает узел</summary>
        public Node ConnectedNode { get; }

        #endregion]

        #region ---- CONSTRUCTORS ----

        /// <summary>Конструктор</summary>
        /// <param name="connectedNode">Связанная вершина</param>
        /// <param name="weight">Вес ребра</param>
        public Edge(Node connectedNode, int weight)
        {
            ConnectedNode = connectedNode;
            Weight = weight;
        }

        #endregion
    }
}