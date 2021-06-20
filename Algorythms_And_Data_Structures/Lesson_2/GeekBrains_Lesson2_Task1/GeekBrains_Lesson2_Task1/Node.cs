namespace GeekBrains_Lesson2_Task1
{
    public class Node
        {
            public int Value { get; set; }
            public Node NextNode { get; set; }
            public Node PrevNode { get; set; }

            public Node(int value)
            {
                Value = value;
            }

        }
}