using System;

namespace GeekBrains_Lesson2_Task1
{
    // Надеин Никита, Урок_2 по курсу Алгоритмы и Структуры данных
    class Program
    {
        static void Main(string[] args)
        {
            ILinkedList list = new TwoLinkedList();

            //Наполняем список различными значениями для теста
            list.AddNode(6);
            list.AddNode(7);
            list.AddNode(15);
            list.AddNode(1345);
            list.AddNode(65);
            list.AddNode(-9);
            list.AddNode(91);
            list.AddNode(320);

            //Выводим содержимое списка
            Console.WriteLine($"Количество элементов в списке: {list.GetCount()}\n");
            PrintList(list);
            Console.ReadLine();

            //сделаем разные манипуляции со списком
            Node testNode = list.FindNodeByIndex(4);
            list.AddNodeAfter(testNode, 1000);
            Console.WriteLine("Добавлен элемент со значением 1000, после элемента с индексом 4");
            PrintList(list);
            Console.ReadLine();


            list.RemoveFirst();
            list.RemoveLast();
            Console.WriteLine("Удалены первый и последний элементы списка");
            PrintList(list);
            Console.ReadLine();

            list.RemoveNode(5);
            Console.WriteLine("Удален элемент с индексом 5");
            PrintList(list);
            Console.ReadLine();

            testNode = list.FindNode(88);
            list.RemoveNode(testNode);
            Console.WriteLine("Удален элемент со значением 88");
            PrintList(list);
            Console.ReadLine();

            list.ClearList();
            Console.WriteLine("Список полностью очищен");
            PrintList(list);

            Console.ReadLine();
        }

        private static void PrintList(ILinkedList list)
        {
            Console.WriteLine("Полный список элементов по индексу:");
            for (int i = 0; i < list.GetCount(); i++)
            {
                Console.WriteLine(i + ":" + list.FindNodeByIndex(i).Value);
            }

            Console.WriteLine();
        }
    }
}