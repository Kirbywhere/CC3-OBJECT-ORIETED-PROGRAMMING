using System;

class Calculator
{
    public int Add(int num1, int num2)
    {
        return num1 + num2;
    }

    public int Subtract(int num1, int num2)
    {
        return num1 - num2;
    }

    public int Multiply(int num1, int num2)
    {
        return num1 * num2;
    }
    public double Divide(int num1, int num2)
    {
        if (num2 == 0)
        {
            Console.WriteLine("Cannot divide by zero.");
            return 0;
        }

        return (double)num1 / num2;
    }

    public bool IsOdd(int num)
    {
        return num % 2 != 0;
    }

    public bool IsEven(int num)
    {
        return num % 2 == 0;
    }
}
class Program
{
    static void Main()
    {
        Calculator calculator = new Calculator();
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("1. Add Two Numbers");
            Console.WriteLine("2. Subtract Two Numbers");
            Console.WriteLine("3. Multiply Two Numbers");
            Console.WriteLine("4. Divide Two Numbers");
            Console.WriteLine("5. Check if a Number is Odd");
            Console.WriteLine("6. Check if a Number is Even");
            Console.WriteLine("7. Exit the Application");

            Console.Write("Enter your choice (1-7): ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Write("Enter first number: ");
                    int addNum1 = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Enter second number: ");
                    int addNum2 = Convert.ToInt32(Console.ReadLine());
                    int sum = calculator.Add(addNum1, addNum2);
                    Console.WriteLine($"Sum: {sum}");
                    break;
                case 7:
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 7.");
                    break;
            }
        }
    }
}
