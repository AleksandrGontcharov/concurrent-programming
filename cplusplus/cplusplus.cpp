// https://leetcode.com/problems/print-in-order/

#include <iostream>
#include <thread>
#include <mutex>
#include <condition_variable>

std::mutex gLock;
std::condition_variable gConditionVariable;

class Foo {

    int state = 1;
  
    public: 
        void first() {
            {
                std::unique_lock<std::mutex> lock(gLock);
                while (state != 1) {
                    gConditionVariable.wait(lock);
                }
            }

            std::cout << "first" << std::endl;

            {
                std::unique_lock<std::mutex> lock(gLock);
                state = 2;
                gConditionVariable.notify_all();
            }
        }

        void second() {
            {
                std::unique_lock<std::mutex> lock(gLock);
                while (state != 2) {
                    gConditionVariable.wait(lock);
                }
            }
            std::cout << "second" << std::endl;

            {
                std::unique_lock<std::mutex> lock(gLock);
                state = 3;
                gConditionVariable.notify_all();
            }
        }

        void third() {
            {
                std::unique_lock<std::mutex> lock(gLock);
                while (state != 3) {
                    gConditionVariable.wait(lock);
                }
            }
            std::cout << "third" << std::endl;

            {
                std::unique_lock<std::mutex> lock(gLock);
                state = 1;
                gConditionVariable.notify_all();
            }
        }
};


int main()
{
    Foo foo = Foo();

    std::thread threadA([&] {
                foo.first();
                });
    
    std::thread threadB([&] {
        foo.second();
        });
    
    std::thread threadC([&] {
        foo.third();
        });

    threadA.join();
    threadB.join();
    threadC.join();

    return(0);
}