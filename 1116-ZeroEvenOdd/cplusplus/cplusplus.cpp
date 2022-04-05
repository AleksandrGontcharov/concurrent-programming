// https://leetcode.com/problems/print-in-order/

#include <iostream>
#include <thread>
#include <mutex>
#include <condition_variable>

std::mutex gLock;
std::condition_variable gConditionVariable;

class Foo {
    private:
        int state = 0;
  
    public: 
        void Zero() {

            for (int i = 1; i <= 7; i++) {
                {
                    std::unique_lock<std::mutex> lock(gLock);
                    gConditionVariable.wait(lock, [&](){ return state == 0;});
                }

                std::cout << 0;

                {
                    std::unique_lock<std::mutex> lock(gLock);
                    state = 1;
                }
                gConditionVariable.notify_all();
            }
        }

        void Even() {
             for (int i = 1; i <= 7; i++) {
                {
                    std::unique_lock<std::mutex> lock(gLock);
                    gConditionVariable.wait(lock, [&](){ return state == 1;});
                }
                if (i % 2 == 0) {
                    std::cout << i;
                }
                {
                    std::unique_lock<std::mutex> lock(gLock);
                    state = 2;
                }
                gConditionVariable.notify_all();
            }
        }
        void Odd() {
             for (int i = 1; i <= 7; i++) {
                {
                    std::unique_lock<std::mutex> lock(gLock);
                    gConditionVariable.wait(lock, [&](){ return state == 2;});
                }
                if (i % 2 == 1) {
                    std::cout << i;
                }
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
                foo.Zero();
                });
    
    std::thread threadB([&] {
        foo.Even();
        });
    
    std::thread threadC([&] {
        foo.Odd();
        });
    

    threadA.join();
    threadB.join();
    threadC.join();

    return(0);
}