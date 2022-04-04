class FooBar {

private:
    int n;

public:
    std::mutex gLock;
    std::condition_variable gConditionVariable;
    FooBar(int n) {
        this->n = n;
    }
    
    int state = 0;

    void foo(function<void()> printFoo) {
        
        for (int i = 0; i < n; i++) {
            
        	{
                std::unique_lock<std::mutex> lock(gLock);
                gConditionVariable.wait(lock, [&](){ return state == 0;});
            }
            
        	printFoo();
            
            {
                std::unique_lock<std::mutex> lock(gLock);
                state = 1;
                gConditionVariable.notify_all();
            }
            
        }
    }

    void bar(function<void()> printBar) {
        
        for (int i = 0; i < n; i++) {
            
        	{
                std::unique_lock<std::mutex> lock(gLock);
                gConditionVariable.wait(lock, [&](){ return state == 1;});
            }
            
        	printBar();
            
            {
                std::unique_lock<std::mutex> lock(gLock);
                state = 0;
                gConditionVariable.notify_all();
            }
            
        }
    }
};