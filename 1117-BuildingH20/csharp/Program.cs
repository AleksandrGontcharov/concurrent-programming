


using System.Globalization;
using System.Runtime.ExceptionServices;

namespace FirstSecond;
using System.Threading;
public class H2O {

    public H2O() {
        
    }
    
    static readonly object _locker = new object();

    int _switch = 1;
    
    int _number_of_H_printed = 0;

    int _number_of_O_printed = 0;


    public void Hydrogen(Action releaseHydrogen)
    {
        lock (_locker)
        {
            while (!(_switch == 1 && _number_of_H_printed < 2))
            {
                Monitor.Wait(_locker);
                if (_number_of_O_printed < 1 && _number_of_H_printed == 2)
                {
                    Monitor.Pulse(_locker);
                }
            }
            _switch = 2;
        }

        releaseHydrogen();
        lock (_locker)
        {
            _number_of_H_printed += 1;
            _switch = 1;

            if (_number_of_H_printed == 2 && _number_of_O_printed == 1)
            {
                _number_of_H_printed = 0;
                _number_of_O_printed = 0;
            }

            Monitor.Pulse(_locker);
        }
    }

    public void Oxygen(Action releaseOxygen)
    {
        lock (_locker)
        {

            while (!(_switch == 1 && _number_of_O_printed < 1))
            {
                Monitor.Wait(_locker);
                if (_number_of_H_printed < 2 && _number_of_O_printed ==  1)
                {
                    Monitor.Pulse(_locker);
                }
            }
            _switch = 2;
        }

        releaseOxygen();

        lock (_locker)
        {
            _switch = 1;
            _number_of_O_printed += 1;



            if (_number_of_H_printed == 2 && _number_of_O_printed == 1)
            {
                _number_of_H_printed = 0;
                _number_of_O_printed = 0;
            }

            Monitor.Pulse(_locker);
        }
    }


    static void Main()
    {
        H2O foo = new H2O();

        void releaseHydrogen()
        {
            Console.WriteLine("H");
        }

        void releaseOxygen()
        {
            Console.WriteLine("O");
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
        threadH7.Start();
        threadH8.Start();

        threadO2.Start();
        threadO1.Start();
        threadO3.Start();
        threadO4.Start();

    }
}
