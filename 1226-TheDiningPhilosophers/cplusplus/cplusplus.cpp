#include <iostream>
#include <thread>
#include <mutex>
#include <condition_variable>
#include <functional>

using namespace std;





class DiningPhilosophers {
private:
    std::mutex _lock;
    // std::condition_variable cv;

    int forksHeld [5] = {0, 0, 0, 0, 0 };
    int forksAvailable [5]= {2, 2, 2, 2, 2 };

public:
    void wantsToEat(int philosopher,
                    function<void()> pickLeftFork,
                    function<void()> pickRightFork,
                    function<void()> eat,
                    function<void()> putLeftFork,
                    function<void()> putRightFork) {
        {
            std::unique_lock<std::mutex> lock(_lock);
            cv.wait(lock, [&](){ return forksAvailable[philosopher] == 2;});
            // You will pick up left fork, so the available forks for you will be 1 less
            forksAvailable[philosopher]--;
            // After picking up left fork, this subtracts one available fork from the philosopher on your left
            forksAvailable[(philosopher + 1) % 5]--;
            // You hold one fork
            forksHeld[philosopher] = 1;
        }
        cv.notify_all();
        
		pickLeftFork();

        {
            std::unique_lock<std::mutex> lock(_lock);
            cv.wait(lock, [&](){ return forksAvailable[philosopher] == 1;});
            // You have a second fork, so the available forks for you is now zero
            forksAvailable[philosopher] = 0;
            // You picked up right fork, so this subtracts one available fork from the philosopher on your right
            forksAvailable[(philosopher + 4) % 5]--;
            forksHeld[philosopher] = 2;
        }
        cv.notify_all();

        pickRightFork();

        eat();

        putLeftFork();

        {
            std::unique_lock<std::mutex> lock(_lock);
            // You put down left fork, so you are now holding one
            forksHeld[philosopher] = 1;
            forksAvailable[philosopher]++;
            // You put down left fork, so this adds one available fork from the philosopher on your left
            forksAvailable[(philosopher + 1) % 5]++;
        }
        cv.notify_all();

        putRightFork();

        {
            std::unique_lock<std::mutex> lock(_lock);
            // You put down left fork, so you are now holding one
            forksHeld[philosopher] = 1;
            forksAvailable[philosopher]++;
            // You put down left fork, so this adds one available fork from the philosopher on your left
            forksAvailable[(philosopher + 1) % 5]++;
        }
        cv.notify_all();
    }
};

void pickLeftFork() {
    std::cout << "Picked up Left Fork" << std::endl;
}

void pickRightFork() {
    std::cout << "Picked up Right Fork" << std::endl;
}

void eat() {
    std::cout << "is eating" << std::endl;
}

void putLeftFork() {
    std::cout << "Put down Left Fork" << std::endl;
}

void putRightFork() {
    std::cout << "Put down Right Fork" << std::endl;
}


int main()
{
    DiningPhilosophers foo = DiningPhilosophers();

    std::thread threadZero([&] {
                foo.wantsToEat(0, pickLeftFork, pickRightFork, eat, putLeftFork, putRightFork);
                });
    std::thread threadOne([&] {
                foo.wantsToEat(1, pickLeftFork, pickRightFork, eat, putLeftFork, putRightFork);
                });
    std::thread threadTwo([&] {
                foo.wantsToEat(2, pickLeftFork, pickRightFork, eat, putLeftFork, putRightFork);
                }); 
    std::thread threadThree([&] {
                foo.wantsToEat(3, pickLeftFork, pickRightFork, eat, putLeftFork, putRightFork);
                }); 
    std::thread threadFour([&] {
                foo.wantsToEat(4, pickLeftFork, pickRightFork, eat, putLeftFork, putRightFork);
                }); 
    
    threadZero.join();
    threadOne.join();
    threadTwo.join();
    threadThree.join();
    threadFour.join();

    return(0);
}