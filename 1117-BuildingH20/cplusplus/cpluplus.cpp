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
    int h = 0;
    int o = 0;
    int element_number = 0;
    std::mutex gLock;
    std::condition_variable gConditionVariable;

public:
    H2O() {
    }

    void hydrogen(function<void()> releaseHydrogen) {
        // std::cout << "starting hydrogen" << h << std::endl;
        {
        std::unique_lock<std::mutex> lock(gLock);
        gConditionVariable.wait(lock, [&](){ return h < 2;});
        h += 1;
        gConditionVariable.notify_all();


                
        gConditionVariable.wait(lock, [&](){ return (h == 2 && o == 1);});

        }

        releaseHydrogen();

        {
            std::unique_lock<std::mutex> lock(gLock);
            element_number += 1;

            if (element_number % 3 == 0) {
                h = 0;
                o = 0;
                gConditionVariable.notify_all();
            }
        }
    }

    void oxygen(function<void()> releaseOxygen) {
        // std::cout << "starting oxygen" << std::endl;
        
        {
        std::unique_lock<std::mutex> lock(gLock);
        gConditionVariable.wait(lock, [&](){ return o < 1;});
        o += 1;
        gConditionVariable.notify_all();

        gConditionVariable.wait(lock, [&](){ return (h == 2 && o == 1);});
        }

        releaseOxygen();

        {
            std::unique_lock<std::mutex> lock(gLock);
            element_number += 1;

            if (element_number % 3 == 0) {
                h = 0;
                o = 0;
                gConditionVariable.notify_all();
            }
        }
    }
};

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