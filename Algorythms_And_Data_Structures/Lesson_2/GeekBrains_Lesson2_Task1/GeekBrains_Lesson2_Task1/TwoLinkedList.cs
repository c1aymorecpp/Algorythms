namespace GeekBrains_Lesson2_Task1
{
    public class TwoLinkedList : ILinkedList
    {
        private Node startNode;
        private Node endNode;
        private int count = 0;



        /// <summary>Добавляет новый элемент в конец списка</summary>
        /// <param name="value">Значение для нового элемента</param>
        public void AddNode(int value)
        {
            if (count != 0)
            {
                endNode.NextNode = new Node(value);
                endNode.NextNode.PrevNode = endNode;
                endNode = endNode.NextNode;
            }
            else
            {
                startNode = new Node(value);
                endNode = startNode;
            }

            count++;
        }

        /// <summary>Добавляет новый элемент после указанного</summary>
        /// <param name="node">Элемент после которого нужно добавить новый</param>
        /// <param name="value">Значение нового элемента</param>
        public void AddNodeAfter(Node node, int value)
        {
            if (CheckNode(node)) //Если заданный элемент в списке есть, то работаем
            {
                if (node == endNode) //Если элемент последний в списке, то просто добавляем его в конец
                {
                    AddNode(value);
                }
                else //Если не последний, то вставляем его между элементами
                {
                    Node nextNode = node.NextNode; //Сохраняем следующую ноду для дальнейшего использования
                    node.NextNode = new Node(value); //Создаем новую ноду после найденной
                    node.NextNode.NextNode = nextNode; //У новой ноды устанавливаем ссылку на следующую
                    node.NextNode.PrevNode = node; //И на предыдущую
                    nextNode.PrevNode = node.NextNode; //Обновляем ссылку на предыдущую ноду у ранее сохраненной
                    count++; //Обновляем количество
                }
            }
        }

        /// <summary>Ищет в списке элемент с заданным значением</summary>
        /// <param name="searchValue">Значение для поиска</param>
        /// <returns>Первый найденный элмент или null если его нет в списке</returns>
        public Node FindNode(int searchValue)
        {
            Node currentNode = startNode;

            bool isSearchEnd = false;
            while (!isSearchEnd)
            {
                if (currentNode != null)
                    if (currentNode.Value == searchValue)
                        isSearchEnd = true;
                    else
                        currentNode = currentNode.NextNode;
                else
                    isSearchEnd = true;
            }

            return currentNode;
        }

        /// <summary>Возвращает количество элементов в списке</summary>
        /// <returns>Количество элементов в списке</returns>
        public int GetCount()
        {
            return count;
        }

        /// <summary> Удаляет из списка элемент с указанным индексом </summary>
        /// <param name="index">Индекс элемента, который нужно удалить из списка</param>
        public void RemoveNode(int index)
        {
            RemoveNode(FindNodeByIndex(index));
        }

        /// <summary>Удаляет из списка указанный элемент</summary>
        /// <param name="node">Элемент который нужно удалить из списка</param>
        public void RemoveNode(Node node)
        {
            if (node != null && CheckNode(node))
            {
                if (node == startNode) RemoveFirst();
                else if (node == endNode) RemoveLast();
                else
                {
                    node.PrevNode.NextNode = node.NextNode;
                    node.NextNode.PrevNode = node.PrevNode;
                    count--;
                }
            }
        }

        /// <summary>Находит элемент в списке по указанному индексу </summary>
        /// <param name="index">Индекс искомого элемента</param>
        /// <returns>Элемент с указанным индексом или null если такого нет</returns>
        public Node FindNodeByIndex(int index)
        {
            Node currentNode = null;
            if (index >= 0 && index < count)
            {
                if (index <= count / 2)
                {
                    currentNode = startNode;
                    int i = 0;
                    while (i < index)
                    {
                        currentNode = currentNode.NextNode;
                        i++;
                    }
                }
                else
                {
                    currentNode = endNode;
                    int i = count - 1;
                    while (i > index)
                    {
                        currentNode = currentNode.PrevNode;
                        i--;
                    }
                }
            }

            return currentNode;
        }

        /// <summary>Удаляет первый элемент из списка</summary>
        public void RemoveFirst()
        {
            if (count > 1)
            {
                startNode.NextNode.PrevNode = null;
                startNode = startNode.NextNode;
                count--;
            }
            else if (count == 0)
            {
                ClearList();
            }
        }

        /// <summary>Удаляет последний элемент из списка</summary>
        public void RemoveLast()
        {
            if (count > 1)
            {
                endNode.PrevNode.NextNode = null;
                endNode = endNode.PrevNode;
                count--;
            }
            else if (count == 0)
            {
                ClearList();
            }
        }

        /// <summary>Очищает список</summary>
        public void ClearList()
        {
            startNode = null;
            endNode = null;
            count = 0;
        }

        /// <summary> Проверяет есть ли такой элемент в списке </summary>
        /// <param name="node">проверяемый элемент</param>
        /// <returns>true, если такой элемент в списке есть</returns>
        public bool CheckNode(Node node)
        {
            bool isNodeInList = false;
            if (count != 0)
            {
                int i = 0;
                Node currentNode = startNode;
                while (!isNodeInList && i < count)
                {
                    isNodeInList = currentNode == node;
                    currentNode = currentNode.NextNode;
                    i++;
                }
            }

            return isNodeInList;
        }
    }
}