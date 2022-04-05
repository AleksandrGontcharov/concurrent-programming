// https://leetcode.com/problems/print-foobar-alternately/
using System.Globalization;
using System.Runtime.ExceptionServices;

public class FooBar
{

    private readonly int n;

    public FooBar(int n)
    {
        this.n = n;
    }

    int _state = 0;

    public void Foo()
    {

        for (int i = 0; i < n; i++)
        {

            lock (this)
            {
                while (!(_state == 0))
                {
                    Monitor.Wait(this);
                }
            }

            Console.Write($"foo");

            lock (this)
            {
                _state = 1;
                Monitor.PulseAll(this);
            }

        }
    }

    public void Bar()
    {

        for (int i = 0; i < n; i++)
        {

            lock (this)
            {
                while (!(_state == 1))
                {
                    Monitor.Wait(this);
                }
            }

            Console.WriteLine($"bar");

            lock (this)
            {
                _state = 0;
                Monitor.PulseAll(this);
            }


        }
    }

    static void Main()
    {
        FooBar foo = new FooBar(10);
        // Display the number of command line arguments.
        Console.WriteLine("Enter one of 12, 21");
        Console.WriteLine();
        Console.Write("Input: ");
        string userInput = Console.ReadLine();

        string[] V = { "12", "21", };

        while (!V.Contains(userInput))
        {
            Console.WriteLine("Enter one of 12, 21");
            userInput = Console.ReadLine();
        };

        // Create threads and assign names

        Thread threadA = (userInput.ElementAt(0).ToString() == "1") ? new Thread(foo.Foo) : new Thread(foo.Bar);
        threadA.Name = "Thread A";

        Thread threadB = (userInput.ElementAt(1).ToString() == "1") ? new Thread(foo.Foo) : new Thread(foo.Bar);
        threadB.Name = "Thread B";


        threadA.Start();
        threadB.Start();
    }
}