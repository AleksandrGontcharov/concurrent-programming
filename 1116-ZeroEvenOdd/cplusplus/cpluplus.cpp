#include <iostream>
#include <thread>
#include <mutex>
#include <condition_variable>
#include <functional>

using namespace std;

std::mutex gLock;
std::condition_variable gConditionVariable;

class LockFromScratch
{
private:
    int n;
    int state = 1;

public:
    LockFromScratch(int n)
    {
        this->n = n;
    }

    // printNumber(x) outputs "x", where x is an integer.
    void zero(function<void(int)> printNumber)
    {

        for (int i = 1; i <= n; i++)
        {
            {
                std::unique_lock<std::mutex> lock(gLock);
                gConditionVariable.wait(lock, [&]()
                                        { return state == 1; });
            }

            printNumber(0);

            {
                std::unique_lock<std::mutex> lock(gLock);
                if (i % 2 == 1)
                {
                    state = 3;
                }
                else
                {
                    state = 2;
                }
            }
            gConditionVariable.notify_all();
        }
    }

    void even(function<void(int)> printNumber)
    {
        for (int i = 2; i <= n; i = i + 2)
        {
            {
                std::unique_lock<std::mutex> lock(gLock);
                gConditionVariable.wait(lock, [&]()
                                        { return state == 2; });
            }

            printNumber(i);

            {
                std::unique_lock<std::mutex> lock(gLock);
                state = 1;
            }
            gConditionVariable.notify_all();
        }
    }
    void odd(function<void(int)> printNumber)
    {
        for (int i = 1; i <= n; i = i + 2)
        {
            {
                std::unique_lock<std::mutex> lock(gLock);
                gConditionVariable.wait(lock, [&]()
                                        { return state == 3; });
            }

            printNumber(i);

            {
                std::unique_lock<std::mutex> lock(gLock);
                state = 1;
            }
            gConditionVariable.notify_all();
        }
    }
};

void printNumber(int x)
{
    std::cout << x;
}

int main()
{
    LockFromScratch foo(8);

    std::thread threadA([&]
                        { foo.zero(printNumber); });

    std::thread threadB([&]
                        { foo.even(printNumber); });

    std::thread threadC([&]
                        { foo.odd(printNumber); });

    threadA.join();
    threadB.join();
    threadC.join();

    return (0);
}