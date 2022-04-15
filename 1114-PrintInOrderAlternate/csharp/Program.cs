// Problem Description:
// Given a class 
// public class Foo {
//   public void first() { print("first"); }
//   public void second() { print("second"); }
// }

// The same instance of Foo will be passed to an even number of threads. Half of the threads
// will call the first method, while the other half will call the second method.
// Modify the program such that the output is "firstsecondfirstsecond...".

// So for example, if the number of threads is 4, the result should be "firstsecondfirstsecond"
// So for example, if the number of threads is 6, the result should be "firstsecondfirstsecondfirstsecond"
// So for example, if the number of threads is 8, the result should be "firstsecondfirstsecondfirstsecondfirstsecond"

// and so on


using System.Globalization;
using System.Runtime.ExceptionServices;

namespace FirstSecond;
public class Foo
{

    public Foo()
    {

    }

    static readonly object _locker = new object();

    int _state = 1;
    bool _firstwasprinted = false;
    bool _secondwasprinted = true;


    public void First()
    {
        lock (_locker)
        {
            while (!(_state == 1 && _secondwasprinted))
            {
                Console.WriteLine($"    {Thread.CurrentThread.Name} acquiring lock");
                Monitor.Wait(_locker);
                Console.WriteLine($"    {Thread.CurrentThread.Name} releasing lock");
                if (_firstwasprinted)
                {
                    Console.WriteLine($"        {Thread.CurrentThread.Name} pulsing one before reacquiring lock");
                    Monitor.Pulse(_locker);
                }
            }
            _state = 2;
        }

        Console.WriteLine($"first - {Thread.CurrentThread.Name}");

        lock (_locker)
        {
            _firstwasprinted = true;
            _secondwasprinted = false;
            Console.WriteLine($"        {Thread.CurrentThread.Name} pulsing one at the end");
            Monitor.Pulse(_locker);
        }
    }

    public void Second()
    {
        lock (_locker)
        {
            while (!(_state == 2 && _firstwasprinted))
            {
                Console.WriteLine($"    {Thread.CurrentThread.Name} acquiring lock");
                Monitor.Wait(_locker);
                if (_secondwasprinted)
                {
                    Console.WriteLine($"        {Thread.CurrentThread.Name} pulsing one before reacquiring lock");
                    Monitor.Pulse(_locker);
                }
            }
            _state = 1;
        }

        Console.WriteLine($"second - {Thread.CurrentThread.Name}");

        lock (_locker)
        {
            _firstwasprinted = false;
            _secondwasprinted = true;
            Console.WriteLine($"        {Thread.CurrentThread.Name} pulsing one at the end");
            Monitor.Pulse(_locker);
        }
    }


    static void Main()
    {
        Foo foo = new Foo();

        
        Thread threadA1 = new Thread(foo.First);
        Thread threadA2 = new Thread(foo.First);
        Thread threadA3 = new Thread(foo.First);
        Thread threadA4 = new Thread(foo.First);
        Thread threadB1 = new Thread(foo.Second);
        Thread threadB2 = new Thread(foo.Second);
        Thread threadB3 = new Thread(foo.Second);
        Thread threadB4 = new Thread(foo.Second);


        threadA1.Name = "threadA1";
        threadA2.Name = "threadA2";
        threadA3.Name = "threadA3";
        threadA4.Name = "threadA4";
        threadB1.Name = "threadB1";
        threadB2.Name = "threadB2";
        threadB3.Name = "threadB3";
        threadB4.Name = "threadB4";

        threadB4.Start();

        threadA1.Start();
        threadA2.Start();
        threadA3.Start();

        threadB1.Start();
        threadB2.Start();
        threadB3.Start();
        threadA4.Start();

    }
}