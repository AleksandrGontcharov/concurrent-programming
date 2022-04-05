using System.Threading;
public class FooBar {
    private int n;

    public FooBar(int n) {
        this.n = n;
    }
    
    int _state = 0;

    public void Foo(Action printFoo) {
        
        for (int i = 0; i < n; i++) {
            
            lock (this)
            {
                while (!(_state == 0))
                {
                    Monitor.Wait(this);
                }
            }
            
        	printFoo();
            
            lock (this)
            {
                _state = 1;
                Monitor.PulseAll(this);
            }
        }
    }

    public void Bar(Action printBar) {
        
        for (int i = 0; i < n; i++) {
            
            lock (this)
            {
                while (!(_state == 1))
                {
                    Monitor.Wait(this);
                }
            }
            
        	printBar();
            
            lock (this)
            {
                _state = 0;
                Monitor.PulseAll(this);
            }
        }
    }
}