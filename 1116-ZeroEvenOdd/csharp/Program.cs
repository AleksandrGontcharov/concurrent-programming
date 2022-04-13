// https://leetcode.com/problems/print-in-order/

namespace program;
public class ZeroEvenOdd {
    private int n;
    
    static readonly object _locker = new object();
    int _state = 1;
    
    public ZeroEvenOdd(int n) {
        this.n = n;
    }
    
    public void Zero(Action<int> printNumber) {
        
        for (int i = 1; i <= n; i++)
        {

            lock (this)
            {
                while (!(_state == 1))
                {
                    Monitor.Wait(this);
                }
            }

            printNumber(0);


            lock (this)
            {
                if (i % 2 == 1) {
                        _state = 3;
                    } else {
                        _state = 2;
                    }
                Monitor.PulseAll(this);
            }
        }
    }
    
     public void Even(Action<int> printNumber) {
        
        for (int i = 2; i <= n; i = i + 2)
        {

            lock (this)
            {
                while (!(_state == 2))
                {
                    Monitor.Wait(this);
                }
            }
            
            printNumber(i);
            
            lock (this)
            {
                _state = 1;
                Monitor.PulseAll(this);
            }

        }
        
    }
    
        public void Odd(Action<int> printNumber)   {

        for (int i = 1; i <= n; i = i + 2)
        {

            lock (this)
            {
                while (!(_state == 3))
                {
                    Monitor.Wait(this);
                }
            }

            printNumber(i);

            lock (this)
            {
                _state = 1;
                Monitor.PulseAll(this);
            }

        }
    }


    static void Main()
    {
        ZeroEvenOdd foo = new ZeroEvenOdd(9);
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

        void printNumber(int x)
        {
            Console.Write(x);
        }

        // Create threads and assign names

        Thread threadA = (userInput.ElementAt(0).ToString() == "1") ? new Thread(() => foo.Zero(printNumber)) : ((userInput.ElementAt(0).ToString() == "2") ? new  Thread(() => foo.Even(printNumber)) : new Thread(() => foo.Odd(printNumber)));
        threadA.Name = "Thread A";

        Thread threadB = (userInput.ElementAt(1).ToString() == "1") ? new  Thread(() => foo.Zero(printNumber)) : ((userInput.ElementAt(1).ToString() == "2") ? new Thread(() => foo.Even(printNumber)) : new Thread(() => foo.Odd(printNumber)));
        threadB.Name = "Thread B";

        Thread threadC = (userInput.ElementAt(2).ToString() == "1") ? new  Thread(() => foo.Zero(printNumber)) : ((userInput.ElementAt(2).ToString() == "2") ? new Thread(() => foo.Even(printNumber)) : new Thread(() => foo.Odd(printNumber)));
        threadC.Name = "Thread C";

        threadA.Start();
        threadB.Start();
        threadC.Start();
    }
};
