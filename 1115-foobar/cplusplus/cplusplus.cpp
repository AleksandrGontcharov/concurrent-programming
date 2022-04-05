// https://leetcode.com/problems/print-in-order/

#include <iostream>
#include <thread>
#include <mutex>
#include <condition_variable>

std::mutex gLock;
std::condition_variable gConditionVariable;

class Foo {

    int state = 0;
  
    public: 
        void foo() {

            for (int i = 0; i < 10; i++) {
                {
                    std::unique_lock<std::mutex> lock(gLock);
                    gConditionVariable.wait(lock, [&](){ return state == 0;});
                }

                std::cout << "foo";

                {
                    std::unique_lock<std::mutex> lock(gLock);
                    state = 1;
                }
                gConditionVariable.notify_all();
            }
        }

        void bar() {
             for (int i = 0; i < 10; i++) {
                {
                    std::unique_lock<std::mutex> lock(gLock);
                    gConditionVariable.wait(lock, [&](){ return state == 1;});
                }
                std::cout << "bar" << std::endl;

                {
                    std::unique_lock<std::mutex> lock(gLock);
                    state = 0;
                }
                gConditionVariable.notify_all();
            }
        }
};


int main()
{
    Foo foo = Foo();

    std::thread threadA([&] {
                foo.foo();
                });
    
    std::thread threadB([&] {
        foo.bar();
        });
    

    threadA.join();
    threadB.join();

    return(0);
}