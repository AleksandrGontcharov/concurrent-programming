#include <iostream>
#include <thread>
#include <mutex>
#include <condition_variable>
#include <functional>
#include <atomic>

std::atomic<bool> taken(false);

class LockFromScratch
{
public:
    void lock()
    {
        bool old_value = false;

        while (!taken.compare_exchange_strong({old_value = false}, true))
        {
            continue;
        }
    }

    void unlock()
    {
        bool old_value = true;
        taken.compare_exchange_strong({old_value = true}, false);
    }

    void threadOne()
    {
        lock();
        std::cout << "Thread one \n";
        unlock();
    }
    void threadTwo()
    {
        lock();
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