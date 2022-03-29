// https://leetcode.com/problems/print-in-order/


public class Foo
{

    public Foo()
    {

    }

    static readonly object _locker = new object();

    int _state = 1;

    public void First()
    {
      lock (_locker)
        {
            while (!(_state == 1))
            {
                Monitor.Wait(_locker);
            }
        }

        Console.WriteLine($"first - {Thread.CurrentThread.Name}");

        lock (_locker) 
        {
            _state = 2; 
            Monitor.PulseAll(_locker); 
        }
    }

    public void Second()
    {
        lock (_locker)
        {
            while (!(_state == 2))
            {
                Monitor.Wait(_locker);
            }
        }

        Console.WriteLine($"second - {Thread.CurrentThread.Name}");


        lock (_locker)
        {
            _state = 3;
            Monitor.PulseAll(_locker);
        }
    }

    public void Third()
    {
        lock (_locker)
        {
            while (!(_state == 3))
                {
                Monitor.Wait(_locker);
            }
        }

        Console.WriteLine($"third - {Thread.CurrentThread.Name}");

        lock (_locker) 
        {
            _state = 1; 
            Monitor.PulseAll(_locker); 
        }    
    }

    static void Main()
    {
        Foo foo = new Foo();
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

        Thread threadA = (userInput.ElementAt(0).ToString() == "1") ? new Thread(foo.First) : ((userInput.ElementAt(0).ToString() == "2") ? new Thread(foo.Second) : new Thread(foo.Third));
        threadA.Name = "Thread A";

        Thread threadB = (userInput.ElementAt(1).ToString() == "1") ? new Thread(foo.First) : ((userInput.ElementAt(1).ToString() == "2") ? new Thread(foo.Second) : new Thread(foo.Third));
        threadB.Name = "Thread B";

        Thread threadC = (userInput.ElementAt(2).ToString() == "1") ? new Thread(foo.First) : ((userInput.ElementAt(2).ToString() == "2") ? new Thread(foo.Second) : new Thread(foo.Third));
        threadC.Name = "Thread C";

        threadA.Start();
        threadB.Start();
        threadC.Start();
    }
}