
namespace DiningPhilosophers;
public class DiningPhilosophers {

    // philosophers: p0, p1, p2, p3, p4
    // forks: f0, f1, f2, f3, f4
    // the order around the table is f0, p0, f1, p1, f2, p2, f3, p3, f4, p4 ->(loop around to f0) 

    // Coniditions
     private int[] forksHeld = new int[] {0, 0, 0, 0, 0 };
    private int[] forksAvailable = new int[] {2, 2, 2, 2, 2 };

    static readonly object _locker = new object();

    public void wantsToEat(int philosopher,
                    Action pickLeftFork,
                    Action pickRightFork,
                    Action eat,
                    Action putLeftFork,
                    Action putRightFork) {

        
        lock (_locker)
        {
            while (!(forksAvailable[philosopher] == 2)) {
                   Console.WriteLine("Philosopher " + philosopher + " is waiting for both forks to be available");
                   Monitor.Wait(_locker);
                }
            // You will pick up left fork, so the available forks for you will be 1 less
            forksAvailable[philosopher]--;
            // After picking up left fork, this subtracts one available fork from the philosopher on your left
            forksAvailable[(philosopher + 1) % 5]--;
            // You hold one fork
            forksHeld[philosopher] = 1;
            // Only pick up the first fork if **both** forks are available
            Monitor.PulseAll(_locker);
        }

        pickLeftFork();

        lock (_locker)
        {
            // Wait for second fork to become available before taking it
            while (!(forksAvailable[philosopher] == 1)) {
                   Monitor.Wait(_locker);
                }
            // You have a second fork, so the available forks for you is now zero
            forksAvailable[philosopher] = 0;

            // You picked up right fork, so this subtracts one available fork from the philosopher on your right
            forksAvailable[(philosopher + 4) % 5]--;
            forksHeld[philosopher] = 2;
            Monitor.PulseAll(_locker);
        }

        pickRightFork();

        eat();

        putLeftFork();

        lock (_locker)
        {
            // You put down left fork, so you are now holding one
            forksHeld[philosopher] = 1;
            forksAvailable[philosopher]++;
            // You put down left fork, so this adds one available fork from the philosopher on your left
            forksAvailable[(philosopher + 1) % 5]++;
            Monitor.PulseAll(_locker);
        }

        putRightFork();

        lock (_locker)
        {
            // You put down right fork, so you are now holding none
            forksHeld[philosopher] = 0;
            forksAvailable[philosopher]++;
            // You put down right fork, so this adds one available fork from the philosopher on your right
            forksAvailable[(philosopher + 4) % 5]++;
            Monitor.PulseAll(_locker);
        }
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
        threadZero.Name = "Philosopher 0";
        threadOne.Name = "Philosopher 1";
        threadTwo.Name = "Philosopher 2";
        threadThree.Name = "Philosopher 3";
        threadFour.Name = "Philosopher 4";

        threadZero.Start();
        threadOne.Start();
        threadTwo.Start();
        threadThree.Start();
        threadFour.Start();
    }
}