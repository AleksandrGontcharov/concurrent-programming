using System.Globalization;
using System.Runtime.ExceptionServices;

namespace H20_BARRIER_4;
public class H2O
{

    public H2O()
    {

    }

    static readonly object _locker = new object();

    int H = 0; // Counts the number of Hs in the molecule (0, 1, or 2) - will reset before the creation of the next molecule
    int O = 0; // Counts the number of Os in the molecule (0 or 1) - will reset before the creation of the next molecule
    int element_number = 0; // Ensures that all three elements are created before the next molecule is created - using Modulo 3 to determine when to pulse the next set of elements

    public void Hydrogen(Action releaseHydrogen)
    {
        lock (_locker)
        {
            // Trap #1
            while (!(H < 2))
            {
                // This traps the 3rd/4th/5th.. hydrogens
                Monitor.Wait(_locker);
            }
            // This lets through the first two Hydrogens
            H += 1;

            // If this is the final element of the H20 molecule, then we will skip trap #2 and that means that 2 elements are already in Trap #2 
            // we will PulseAll to let our peers know that we are ready to go. Note that anyone in trap #1 will still be stuck there until all three are released
            if ((H == 2 && O == 1))
            {
                Monitor.PulseAll(_locker);
            }

            /// Trap #2
            while (!(H == 2 && O == 1))
            {
                // Now we trap the first two elements of H20 molecule here
                Monitor.Wait(_locker);
            }
        }
        releaseHydrogen();

        lock (_locker)
        {
            element_number += 1;

            // The switch ensures that all three elements of the previous H20 molecule were printed before we can pulse the next set
            if (element_number % 3 == 0) {
                H = 0;
                O = 0;
                Monitor.PulseAll(_locker);
            }
        }
    }

    public void Oxygen(Action releaseOxygen)
    {
        lock (_locker)
        {
            
            // Trap #1
            while (!(O < 1))
            {

                // This traps the 2nd/3rd/4th.. oxygen
                Monitor.Wait(_locker);
            }
            // This lets through the first Oxygen
            O += 1;

            // If this is the final element of the H20 molecule, then we will skip trap #2 and that means that 2 elements are already in Trap #2 
            // we will PulseAll to let our peers know that we are ready to go. Note that anyone in trap #1 will still be stuck there until all three are released
            if ((H == 2 && O == 1))
            {
                Monitor.PulseAll(_locker);
            }

            /// Trap #2
            while (!(H == 2 && O == 1))
            {
                // Now we trap the first two elements of H20 molecule here
                Monitor.Wait(_locker);
            }
        }

        releaseOxygen();


        lock (_locker)
        {
            element_number += 1;

            // The switch ensures that all three elements of the previous H20 molecule were printed before we can pulse the next set
            if (element_number % 3 == 0) {
                H = 0;
                O = 0;
                Monitor.PulseAll(_locker);
            }
        }
    }


    static void Main23()
    {
        H2O foo = new H2O();

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