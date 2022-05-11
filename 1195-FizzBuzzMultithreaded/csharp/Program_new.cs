
namespace FizzBuzzMultithreaded_two;
public class FizzBuzz {
    private int n;

    private int i = 1;

    static readonly object _locker = new object();

    public FizzBuzz(int n) {
        this.n = n;
    }

    public void Fizz(Action printFizz) {
        WordSubroutine(printFizz, null, "Fizz");
    }

    public void Buzz(Action printBuzz) {
        WordSubroutine(printBuzz, null, "Buzz");
    }

    public void Fizzbuzz(Action printFizzBuzz) {
        WordSubroutine(printFizzBuzz, null, "FizzBuzz");
    }

    public void Number(Action<int> printNumber) {
        WordSubroutine(null, printNumber, "Number");

    }

    void WordSubroutine(Action? printWord, Action<int>? printNumber, string Word) {
        while (true) {
            lock (_locker)
            {                            
                while (!(whoseTurnCalculation(i) == Word || i > n)) {
                    Monitor.Wait(_locker);
                }
                if (i > n) {
                    break;
                }
            }

            if (printWord != null) {
                printWord();
            }
            if (printNumber != null) {
                lock (_locker) {
                    printNumber(i);
                }
            }

            lock (_locker)
            { 
                i = i + 1;
                Monitor.PulseAll(_locker);
            }
        }
    }

    string whoseTurnCalculation(int i) {
        bool divisibleBy3 = (i % 3 == 0);
        bool divisibleBy5 = (i % 5 == 0);
            
        bool fizzTurn = (divisibleBy3 && !divisibleBy5);
        bool buzzTurn = (!divisibleBy3 && divisibleBy5);
        bool fizzBuzzTurn = (divisibleBy3 && divisibleBy5);
        bool numberTurn = (!divisibleBy3 && !divisibleBy5);
            
        if (numberTurn) {
            return "Number";
             }
        else if (fizzTurn) {
            return "Fizz";
        }
        else if (buzzTurn) {
            return "Buzz";
        }
        else  {
            return "FizzBuzz";
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