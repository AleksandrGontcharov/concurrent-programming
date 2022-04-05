﻿// https://leetcode.com/problems/print-in-order/

namespace program;
public class ZeroEvenOdd
{
    private readonly int n;

    public ZeroEvenOdd(int n)
    {
        this.n = n;
        Console.WriteLine($"Initialized with n = {this.n}");
    }

    static readonly object _locker = new object();
    int _state = 1;

    public void Zero()
    {
        for (int i = 1; i <= n; i++)
        {

            lock (this)
            {
                while (!(_state == 1))
                {
                    Monitor.Wait(this);
                }
            }

            Console.WriteLine($"0 - {Thread.CurrentThread.Name}");

            lock (this)
            {
                _state = 2;
                Monitor.PulseAll(this);
            }
        }
    }

    public void Even()
    {

        for (int i = 1; i <= n; i++)
        {

            lock (this)
            {
                while (!(_state == 2))
                {
                    Monitor.Wait(this);
                }
            }
            if (i % 2 == 0)
            {
                Console.WriteLine($"{i} - {Thread.CurrentThread.Name}");
            }   

            lock (this)
            {
                _state = 3;
                Monitor.PulseAll(this);
            }

        }
    }

    public void Odd()
    {

        for (int i = 1; i <= n; i++)
        {

            lock (this)
            {
                while (!(_state == 3))
                {
                    Monitor.Wait(this);
                }
            }

            if (i % 2 == 1)
            {
                Console.WriteLine($"{i} - {Thread.CurrentThread.Name}");
            } 
            lock (this)
            {
                _state = 1;
                Monitor.PulseAll(this);
            }

        }
    }

    static void Main()
    {
        ZeroEvenOdd foo = new ZeroEvenOdd(5);
        // Display the number of command line arguments.
        Console.WriteLine("Enter one of 123, 132, 321, 312, 213, 231");
        Console.WriteLine("This will determine the thread assigned to the printing of 'first', 'second' and 'third'");
        Console.WriteLine();
        Console.Write("Input: ");
        string userInput = Console.ReadLine();

        string[] V = { "123", "132", "213", "231", "321", "312" };

        while (!V.Contains(userInput))
        {
            Console.WriteLine("Enter one of 123, 132, 321, 312, 213, 231");
            userInput = Console.ReadLine();
        };

        // Create threads and assign names

        Thread threadA = (userInput.ElementAt(0).ToString() == "1") ? new Thread(foo.Zero) : ((userInput.ElementAt(0).ToString() == "2") ? new Thread(foo.Even) : new Thread(foo.Odd));
        threadA.Name = "Thread A";

        Thread threadB = (userInput.ElementAt(1).ToString() == "1") ? new Thread(foo.Zero) : ((userInput.ElementAt(1).ToString() == "2") ? new Thread(foo.Even) : new Thread(foo.Odd));
        threadB.Name = "Thread B";

        Thread threadC = (userInput.ElementAt(2).ToString() == "1") ? new Thread(foo.Zero) : ((userInput.ElementAt(2).ToString() == "2") ? new Thread(foo.Even) : new Thread(foo.Odd));
        threadC.Name = "Thread C";

        threadA.Start();
        threadB.Start();
        threadC.Start();
    }
}