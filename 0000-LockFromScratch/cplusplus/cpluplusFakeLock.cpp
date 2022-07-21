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
    int taken = false;

public:
    void fakeLock()
    {
        if (taken)
        {
            while (taken)
            {
                std::cout << "waiting on lock: taken = true \n";
                continue;
            }
            fakeLock();
        }
        else
        {
            std::cout << "taking lock: taken = true \n";
            taken = true;
        }
    }

    void unlock()
    {
        std::cout << "releasing lock: taken = false \n";
        taken = false;
    }

    void threadOne()
    {
        fakeLock();
        std::cout << "Thread one \n";
        unlock();
    }
    void threadTwo()
    {
        fakeLock();
        std::cout << "Thread two \n";
        unlock();
    }
};

int main()
{
    LockFromScratch lockFromScratch;

    std::thread threadA([&]
                        { lockFromScratch.threadOne(); });

    std::thread threadB([&]
                        { lockFromScratch.threadTwo(); });

    threadA.join();
    threadB.join();

    return (0);
}