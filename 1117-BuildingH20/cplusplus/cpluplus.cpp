#include <iostream>
#include <thread>
#include <mutex>
#include <condition_variable>
#include <functional>

using namespace std;

std::mutex gLock;
std::condition_variable gConditionVariable;

class H2O {
private:
    int h = 2;
    int o = 1;
    std::mutex gLock;
    std::condition_variable gConditionVariable;

public:
    H2O() {
    }

    void hydrogen(function<void()> releaseHydrogen) {
        
        std::unique_lock<std::mutex> lock(gLock);
        gConditionVariable.wait(lock, [&](){ return h > 0;});

        releaseHydrogen();

        h = h - 1;
        
        if (h + o == 0) {
            h = 2;
            o = 1;
        }
        gConditionVariable.notify_all();
    }

    void oxygen(function<void()> releaseOxygen) {

        std::unique_lock<std::mutex> lock(gLock);
        gConditionVariable.wait(lock, [&](){ return o > 0;});
        
        releaseOxygen();

        o = o - 1;
        
        if (h + o == 0) {
            h = 2;
            o = 1;
        }
        gConditionVariable.notify_all();
    }
};

void printNumber(int x) {
    std::cout << x;
}

void releaseHydrogen() {
    std::cout << "H";
}

void releaseOxygen() {
    std::cout << "O";
}


int main()
{
    H2O foo;

    std::thread threadH_1([&] {
                foo.hydrogen(releaseHydrogen);
                });
    std::thread threadH_2([&] {
                foo.hydrogen(releaseHydrogen);
                });
    std::thread threadH_3([&] {
                foo.hydrogen(releaseHydrogen);
                });
    std::thread threadH_4([&] {
                foo.hydrogen(releaseHydrogen);
                });
    std::thread threadH_5([&] {
                foo.hydrogen(releaseHydrogen);
                });
    std::thread threadH_6([&] {
                foo.hydrogen(releaseHydrogen);
                });
    
    std::thread threadO_1([&] {
        foo.oxygen(releaseOxygen);
        });
    std::thread threadO_2([&] {
        foo.oxygen(releaseOxygen);
        });
    std::thread threadO_3([&] {
        foo.oxygen(releaseOxygen);
        });
    

    threadH_1.join();
    threadH_2.join();
    threadH_3.join();
    threadH_4.join();
    threadH_5.join();
    threadH_6.join();
    threadO_1.join();
    threadO_2.join();
    threadO_3.join();

    return(0);
}