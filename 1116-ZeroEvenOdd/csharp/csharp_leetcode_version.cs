// https://leetcode.com/problems/print-zero-even-odd


using System.Threading;

public class ZeroEvenOdd {
    private int n;
    
    static readonly object _locker = new object();
    int _state = 1;
    
    public ZeroEvenOdd(int n) {
        this.n = n;
    }
    
    public void Zero(Action<int> printNumber) {
        
        for (int i = 1; i <= n; i++)
        {

            lock (this)
            {
                while (!(_state == 1))
                {
                    Monitor.Wait(this);
                }
            }

            printNumber(0);


            lock (this)
            {
                _state = 2;
                Monitor.PulseAll(this);
            }
        }
    }
    
     public void Even(Action<int> printNumber) {
        
        for (int i = 1; i <= n; i++)
        {

            lock (this)
            {
                while (!(_state == 2))
                {
                    Monitor.Wait(this);
                }
            }
            if (i % 2 == 0 ) { 
                printNumber(i);
            }
            lock (this)
            {
                _state = 3;
                Monitor.PulseAll(this);
            }

        }
        
    }
    
        public void Odd(Action<int> printNumber)   {

        for (int i = 1; i <= n; i++)
        {

            lock (this)
            {
                while (!(_state == 3))
                {
                    Monitor.Wait(this);
                }
            }

            if (i % 2 == 1)
            {
                  printNumber(i);
            }

            lock (this)
            {
                _state = 1;
                Monitor.PulseAll(this);
            }

        }
    }
}
