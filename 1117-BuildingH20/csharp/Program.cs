// https://leetcode.com/problems/print-in-order/

namespace program;
public class H20 {
    
    
    public H20() {
    }

    List<string> _state = new List<string>() { "H", "H", "O" };
    
    private readonly object _locker = new object();

    public void Hydrogen(Action releaseHydrogen) {    
        lock (_locker)
        {
            while (!(_state.Contains("H")))
                {
                Monitor.Wait(_locker);
            }
         

            releaseHydrogen();

       
            _state.Remove("H");
            if (_state.Count == 0) {
                _state.Add("H");
                _state.Add("H");
                _state.Add("O");
            }
            Monitor.PulseAll(_locker); 
        }
    }
    
     public void Oxygen(Action releaseOxygen) {

        lock (_locker)
        {
            while (!(_state.Contains("O")))
                {
                Monitor.Wait(_locker);
            }
        
        
            releaseOxygen();

        
            _state.Remove("O");
            if (_state.Count == 0) {
                _state.Add("H");
                _state.Add("H");
                _state.Add("O");
            }
            Monitor.PulseAll(_locker); 
        }
    }


    static void Main()
    {
        H20 foo = new H20();

        // Create threads and assign names

        void releaseHydrogen() {
            Console.Write("H");
        }
        void releaseOxygen() {
            Console.Write("O");
        }

        Thread threadH_1 = new Thread(() => foo.Hydrogen(releaseHydrogen));
        Thread threadH_2 = new Thread(() => foo.Hydrogen(releaseHydrogen));
        Thread threadH_3 = new Thread(() => foo.Hydrogen(releaseHydrogen));
        Thread threadH_4 = new Thread(() => foo.Hydrogen(releaseHydrogen));
        Thread threadO_1 = new Thread(() => foo.Oxygen(releaseOxygen));
        Thread threadO_2 = new Thread(() => foo.Oxygen(releaseOxygen));

        threadH_1.Start();
        threadH_2.Start();
        threadH_3.Start();
        threadH_4.Start();
        threadO_1.Start();
        threadO_2.Start();
    }
};
