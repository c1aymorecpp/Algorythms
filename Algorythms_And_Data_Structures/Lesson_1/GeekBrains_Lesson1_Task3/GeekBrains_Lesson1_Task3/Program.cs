using System;

namespace GeekBrains_Lesson1_Task3
{
    // Надеин Никита, Урок_1 по курсу Алгоритмы и Структуры данных
    
    class Program
    {
        //Вычисление числа Фибоначчи методом Рекурсии
        private static ulong Fibonacchi(int n)
        {
            if (n > 1)
                return (ulong)(Fibonacchi(n - 1) + Fibonacchi(n - 2));
            else
                return (ulong)n;

            //Конструкцию выше можно заменить на одну строку с использованием тернарного оператора:
            //return n > 1 ? (ulong)(FibStandart(n - 1) + FibStandart(n - 2)) : (ulong)n;
        }
        
        //Вычисление числа Фибоначчи при помощи цикла 
        private static ulong FibonacchiCycle(int n)
        {
            
            ulong x1 = 1;
            ulong x0 = 1;

            for (int i = 2; i < n; i++)
            {
                x1 = x0 + x1;
                x0 = x1 - x0;
            }
            return x1;
        }
    }
}