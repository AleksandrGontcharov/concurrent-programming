
namespace FizzBuzzMultithreaded;
public class FizzBuzz {
    private int n;

    public FizzBuzz(int n) {
        this.n = n;
    }

    // printFizz() outputs "fizz".
    public void Fizz(Action printFizz) {
        
    }

    // printBuzzz() outputs "buzz".
    public void Buzz(Action printBuzz) {
        
    }

    // printFizzBuzz() outputs "fizzbuzz".
    public void Fizzbuzz(Action printFizzBuzz) {
        
    }

    // printNumber(x) outputs "x", where x is an integer.
    public void Number(Action<int> printNumber) {
        
    }
}


    static void Main()
    {
        FizzBuzz foo = new FizzBuzz();

        void releaseHydrogen()
        {
            Console.WriteLine($"H - {Thread.CurrentThread.Name}");
        }

        void releaseOxygen()
        {
            Console.WriteLine($"O - {Thread.CurrentThread.Name}");

        }


        Thread threadH1 = new Thread(() => foo.Hydrogen(releaseHydrogen));
        Thread threadH2 = new Thread(() => foo.Hydrogen(releaseHydrogen));
        Thread threadH3 = new Thread(() => foo.Hydrogen(releaseHydrogen));
        Thread threadH4 = new Thread(() => foo.Hydrogen(releaseHydrogen));
        Thread threadH5 = new Thread(() => foo.Hydrogen(releaseHydrogen));
        Thread threadH6 = new Thread(() => foo.Hydrogen(releaseHydrogen));
        Thread threadH7 = new Thread(() => foo.Hydrogen(releaseHydrogen));
        Thread threadH8 = new Thread(() => foo.Hydrogen(releaseHydrogen));
        Thread threadO1 = new Thread(() => foo.Oxygen(releaseOxygen));
        Thread threadO2 = new Thread(() => foo.Oxygen(releaseOxygen));
        Thread threadO3 = new Thread(() => foo.Oxygen(releaseOxygen));
        Thread threadO4 = new Thread(() => foo.Oxygen(releaseOxygen));


        threadH1.Name = "threadH1";
        threadH2.Name = "threadH2";
        threadH3.Name = "threadH3";
        threadH4.Name = "threadH4";
        threadH5.Name = "threadH5";
        threadH6.Name = "threadH6";
        threadH7.Name = "threadH7";
        threadH8.Name = "threadH8";
        threadO1.Name = "threadO1";
        threadO2.Name = "threadO2";
        threadO3.Name = "threadO3";
        threadO4.Name = "threadO4";


        threadH1.Start();
        threadH2.Start();
        threadH3.Start();
        threadH4.Start();
        threadH5.Start();
        threadH6.Start();
        //threadH7.Start();
        //threadH8.Start();

        threadO1.Start();
        threadO2.Start();
        threadO3.Start();
        //threadO4.Start();

    }
}