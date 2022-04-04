using System.Threading;

public class Foo {

    public Foo() {
        
    }
    
    static readonly object _locker = new object();

    int _state = 1;


    public void First(Action printFirst) {

        printFirst();

        lock (_locker) 
        {
            _state = 2; 
            Monitor.PulseAll(_locker); 
        }

    }

    public void Second(Action printSecond) {
        lock (_locker)
        {
            while (!(_state == 2))
            {
                Monitor.Wait(_locker);
            }
        }
        printSecond();

        lock (_locker)
        {
            _state = 3;
            Monitor.PulseAll(_locker);
        }
    }

    public void Third(Action printThird) {
        lock (_locker)
        {
            while (!(_state == 3))
                {
                Monitor.Wait(_locker);
            }
        }
        printThird();
    }
}