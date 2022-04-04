#include <mutex>


class Foo {
    int state = 1;
public:
    std::mutex gLock;
    std::condition_variable gConditionVariable;
    
    Foo() {
        
    }

    void first(function<void()> printFirst) {
        
        {
                std::unique_lock<std::mutex> lock(gLock);
                gConditionVariable.wait(lock, [&] () { return state == 1;});
        }

        printFirst();
        
        {
                std::unique_lock<std::mutex> lock(gLock);
                state = 2;
                gConditionVariable.notify_all();
        }
    }

    void second(function<void()> printSecond) {
        
        {
                std::unique_lock<std::mutex> lock(gLock);
                gConditionVariable.wait(lock, [&] () { return state == 2;});
        }

        printSecond();
        
        {
                std::unique_lock<std::mutex> lock(gLock);
                state = 3;
                gConditionVariable.notify_all();
        }
    }

    void third(function<void()> printThird) {
        
        {
                std::unique_lock<std::mutex> lock(gLock);
                gConditionVariable.wait(lock, [&] () { return state == 3;});
        }

        printThird();
        
        {
                std::unique_lock<std::mutex> lock(gLock);
                state = 1;
                gConditionVariable.notify_all();
        }
    }
};