
namespace DiningPhilosophers;
public class DiningPhilosophers {
    public void wantsToEat(int philosopher,
                    Action pickLeftFork,
                    Action pickRightFork,
                    Action eat,
                    Action putLeftFork,
                    Action putRightFork) {
        pickLeftFork();
        pickRightFork();
        eat();
        putLeftFork();
        putRightFork();
    }

    static void Main()
    {
        DiningPhilosophers foo = new DiningPhilosophers();

        void pickLeftFork()
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} - Picked Up Left Fork...");
        }

        void pickRightFork()
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} - Picked Up Right Fork...");
        }

        void putLeftFork()
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} - Put Down Left Fork...");
        }

        void putRightFork()
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} - Put Down Right Fork...");
        }
        void eat()
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} - is eating...");
        }

        Thread threadZero = new Thread(() => foo.wantsToEat(0, pickLeftFork, pickRightFork, eat, putLeftFork, putRightFork));
        Thread threadOne = new Thread(()  => foo.wantsToEat(1, pickLeftFork, pickRightFork, eat, putLeftFork, putRightFork));
        Thread threadTwo = new Thread(()  => foo.wantsToEat(2, pickLeftFork, pickRightFork, eat, putLeftFork, putRightFork));
        Thread threadThree = new Thread(() => foo.wantsToEat(3, pickLeftFork, pickRightFork, eat, putLeftFork, putRightFork));
        Thread threadFour = new Thread(() => foo.wantsToEat(4, pickLeftFork, pickRightFork, eat, putLeftFork, putRightFork));
        threadZero.Name = "Philosopher Zero";
        threadOne.Name = "Philosopher One";
        threadTwo.Name = "Philosopher Two";
        threadThree.Name = "Philosopher Three";
        threadFour.Name = "Philosopher Four";

        threadZero.Start();
        threadOne.Start();
        threadTwo.Start();
        threadThree.Start();
        threadFour.Start();
    }
}