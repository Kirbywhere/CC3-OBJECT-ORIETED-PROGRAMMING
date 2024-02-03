using System;
class Table
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter a number:");
        int x = Convert.ToInt32(Console.ReadLine());
        for (int i = 1; i <= x; i++)
        {
            for (int j = 1; j <= x; j++)
            {
                Console.Write(i * j + "\t");
            }
            Console.Write("\n");
        }
        Console.ReadLine();
    }
}



***CODE 2***


using System;
class HelloWorld
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter a number:");
        int x = Convert.ToInt32(Console.ReadLine());

        for (int i = 1; i <= x; i++)
        {
            for (int j = 1; j <= x; j++)
            {
                Console.Write(i * j + "\t");
            }
            Console.Write("\n");
        }

        Console.ReadLine();
    }
}
