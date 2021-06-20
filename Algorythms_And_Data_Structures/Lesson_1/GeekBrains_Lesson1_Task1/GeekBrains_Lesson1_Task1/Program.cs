using System;

namespace GeekBrains_Lesson1_Task1
{
    //Надеин Никита, Урок_1 по курсу Алгоритмы и Структуры данных
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите целое число n: ");
            int n = Convert.ToInt32(Console.ReadLine());
            
            Function(n);
        }

        // Функция для определения является ли число простым.
        static void Function(int number)
        {
            int d = 0;
            int i = 2;

            // Цикл, пока i меньше заданного числа, выполняется условие
            while (i < number)
            {
                if (number % i == 0)
                {
                    d++;
                    i++;
                }

                else
                    i++;
            }

            if (d == 0)
                Console.WriteLine("Число простое");
            else
                Console.WriteLine("Число не простое");
        }
    }
}