
namespace FizzBuzzMultithreaded_two;
public class FizzBuzz {
    private int n;

    private int i = 1;

    static readonly object _locker = new object();

    public FizzBuzz(int n) {
        this.n = n;
    }

    public void Fizz(Action printFizz) {
        WordSubroutine((k) => printFizz(),  (k) =>  ((k % 3 == 0) && !(k % 5 == 0)));
    }

    public void Buzz(Action printBuzz) {
        WordSubroutine((k) => printBuzz(), (k) => (!(k % 3 == 0) && (k % 5 == 0)));
    }

    public void Fizzbuzz(Action printFizzBuzz) {
        WordSubroutine((k) => printFizzBuzz(),  (k) => ((k % 3 == 0) && (k % 5 == 0)));
    }

    public void Number(Action<int> printNumber) {
        WordSubroutine(printNumber, (k) => (!(k % 3 == 0) && !(k % 5 == 0)));

    }

    void WordSubroutine(Action<int> printFunction, Func<int, bool> condition) {
        while (true) {
            lock (_locker)
            {                            
                while (!(condition(i) || i > n)) {
                    Monitor.Wait(_locker);
                }
                if (i > n) {
                    break;
                }

                printFunction(i);
             
                i = i + 1;
                Monitor.PulseAll(_locker);
            }
        }
    }

    static void Main()
    {
        FizzBuzz foo = new FizzBuzz(15);

        void printFizz()
        {
            Console.WriteLine("Fizz");
        }

        void printBuzz()
        {
            Console.WriteLine("Buzz");

        }
        void printFizzBuzz()
        {
            Console.WriteLine("FizzBuzz");

        }
        void printNumber(int i)
        {
            Console.WriteLine(i);

        }


        Thread threadA = new Thread(() => foo.Fizz(printFizz));
        Thread threadB = new Thread(() => foo.Buzz(printBuzz));
        Thread threadC = new Thread(() => foo.Fizzbuzz(printFizzBuzz));
        Thread threadD = new Thread(() => foo.Number(printNumber));



        threadA.Start();
        threadB.Start();
        threadC.Start();
        threadD.Start();


    }
}