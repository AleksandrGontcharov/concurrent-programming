


using System.Globalization;
using System.Runtime.ExceptionServices;

namespace H20_BARRIER;
public class H2O
{

    public H2O()
    {

    }

    static readonly object _locker = new object();

    int H = 0;
    int O = 0;
    bool _switch = true;
    int element_number = 0;

    public void Hydrogen(Action releaseHydrogen)
    {
        lock (_locker)
        {
            _switch = false;
            

            // Trap #1
            while (!(H < 2))
            {
                Console.WriteLine($"    {Thread.CurrentThread.Name} stuck in TRAP #1 => H = {H}, O = {O}");
                // This traps the 3rd/4th/5th.. hydrogens
                Monitor.Wait(_locker);
            }
            // This lets through the first two Hydrogens
            H += 1;

            Console.WriteLine($"    {Thread.CurrentThread.Name} I will get passed TRAP #2 and I am third element of H20  = {(H == 2 && O == 1)} - I must inform the others in trap #2 to release ourselves");

            if ((H == 2 && O == 1))
            {
                Monitor.PulseAll(_locker);
            }

            //// Trap #2
            while (!(H == 2 && O == 1))
            {
                Console.WriteLine($"    {Thread.CurrentThread.Name} stuck in TRAP #2 Because I am either first or second element of H20 => H = {H}, O = {O}");
                // Now we "Trap" the oxygen until we have a full H20 molecule
                Monitor.Wait(_locker);
            }
        }
        releaseHydrogen();

        lock (_locker)
        {
            element_number += 1;
            _switch = (element_number % 3 == 0);
            Console.WriteLine($"    {Thread.CurrentThread.Name} Ready for the next element - I am element number {element_number} I can pulse all {_switch}");

            // The switch ensures that all three elements of the previosu H20 molecule were printed before we can pulse the next set
            if (_switch) {
                H = 0;
                O = 0;
                Console.WriteLine($"    {Thread.CurrentThread.Name} Finally pulsing -  H = {H}, O = {O}");
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
                Console.WriteLine($"    {Thread.CurrentThread.Name} stuck in TRAP #1 => H = {H}, O = {O}");

                // This traps the 2rd/3th/4th.. oxygen
                Monitor.Wait(_locker);
            }
            // This lets through the first Oxygen
            O += 1;

            Console.WriteLine($"    {Thread.CurrentThread.Name} I will get passed TRAP #2 and I am third element of H20  = {(H == 2 && O == 1)} - I must inform the others in trap #2 to release ourselves");

            if ((H == 2 && O == 1))
            {
                Monitor.PulseAll(_locker);
            }

            //// Trap #2
            while (!(H == 2 && O == 1))
            {
                Console.WriteLine($"    {Thread.CurrentThread.Name} stuck in TRAP #2 Because I am either first or second element of H20 => H = {H}, O = {O}");
                // Now we "Trap" the oxygen until we have a full H20 molecule
                Monitor.Wait(_locker);
            }
        }

        releaseOxygen();


        lock (_locker)
        {
            //O = 0;
            element_number += 1;
            _switch = (element_number % 3 == 0);
            Console.WriteLine($"    {Thread.CurrentThread.Name} Ready for the next element - I am element number {element_number} I can pulse all {_switch}");
            if (_switch)
            {
                H = 0;
                O = 0;
                Console.WriteLine($"    {Thread.CurrentThread.Name} Finally pulsing -  H = {H}, O = {O}");

                Monitor.PulseAll(_locker);
            }
        }
    }


    static void Main12()
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