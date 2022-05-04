#include <iostream>
#include <thread>
#include <mutex>
#include <condition_variable>
#include <functional>

using namespace std;


class FizzBuzz {
private:
    int n;

    string whichThread = "Number";

    std::mutex _lock;
    std::condition_variable cv;


public:
    FizzBuzz(int n) {
        this->n = n;
    }

    void fizz(function<void()> printFizz) {
        int total = 0;
        {
            std::unique_lock<std::mutex> lock(_lock);
            total = numOfFizz(n);
        }

        for (int i = 1; i <= total; i++) {

            {
                std::unique_lock<std::mutex> lock(_lock);
                cv.wait(lock, [&](){ return (whichThread == "Fizz");});

            }

        printFizz();                
        
            {
                std::unique_lock<std::mutex> lock(_lock);
                whichThread = "Number";
            }
            cv.notify_all();
         }

    }

    void buzz(function<void()> printBuzz) {
        int total = 0;
        {
            std::unique_lock<std::mutex> lock(_lock);
            total = numOfBuzz(n);
        }

        for (int i = 1; i <= total; i++) {

            {
                std::unique_lock<std::mutex> lock(_lock);
                cv.wait(lock, [&](){ return (whichThread == "Buzz");});
            }

        printBuzz();                
        
            {
                std::unique_lock<std::mutex> lock(_lock);
                whichThread = "Number";
            }
            cv.notify_all();
         }
    }

	void fizzbuzz(function<void()> printFizzBuzz) {

        int total = 0;
        {
            std::unique_lock<std::mutex> lock(_lock);
            total = numOfFizzBuzz(n);
        }

        for (int i = 1; i <= total; i++) {

            {
                std::unique_lock<std::mutex> lock(_lock);
                cv.wait(lock, [&](){ return (whichThread == "FizzBuzz");});
            }

        printFizzBuzz();                
        
            {
                std::unique_lock<std::mutex> lock(_lock);
                whichThread = "Number";
            }
            cv.notify_all();
         }
    }

    void number(function<void(int)> printNumber) {

        int total = 0;
        {
            std::unique_lock<std::mutex> lock(_lock);
            total = n;
        }


        for (int i = 1; i <= total; i++) {
            {
                std::unique_lock<std::mutex> lock(_lock);
                
                whichThread = whoseTurnCalculation(i);

                if (whichThread != "Number") {
                    cv.notify_all();
                    cv.wait(lock, [&](){ return (whichThread == "Number");});
                    continue;
                    }
            }            

            printNumber(i);
        }
    }

    int numOfFizz(int n) {
        int count = 0;
        for (int i = 1; i <= n; i++) {
            bool divisibleBy3 = (i % 3 == 0);
            bool divisibleBy5 = (i % 5 == 0);
            if (divisibleBy3 && !divisibleBy5) {
                count++;
            }
        }
        return count;
    }
    
    int numOfBuzz(int n) {
        int count = 0;
        for (int i = 1; i <= n; i++) {
            bool divisibleBy3 = (i % 3 == 0);
            bool divisibleBy5 = (i % 5 == 0);
            if (!divisibleBy3 && divisibleBy5) {
                count++;
            }
        }
        return count;
    }

    int numOfFizzBuzz(int n) {
        int count = 0;
        for (int i = 1; i <= n; i++) {
            bool divisibleBy3 = (i % 3 == 0);
            bool divisibleBy5 = (i % 5 == 0);
            if (divisibleBy3 && divisibleBy5) {
                count++;
            }
        }
        return count;
    }

    string whoseTurnCalculation(int i) {
        bool divisibleBy3 = (i % 3 == 0);
        bool divisibleBy5 = (i % 5 == 0);
            
        bool fizzTurn = (divisibleBy3 && !divisibleBy5);
        bool buzzTurn = (!divisibleBy3 && divisibleBy5);
        bool fizzBuzzTurn = (divisibleBy3 && divisibleBy5);
        bool numberTurn = (!divisibleBy3 && !divisibleBy5);
            
        if (numberTurn) {
            return "Number";
             }
        else if (fizzTurn) {
            return "Fizz";
        }
        else if (buzzTurn) {
            return "Buzz";
        }
        else  {
            return "FizzBuzz";
        }
    }
};

void printFizz() {
    std::cout << "Fizz" << std::endl;
}

void printBuzz() {
    std::cout << "Buzz" << std::endl;
}

void printFizzBuzz() {
    std::cout << "FizzBuzz" << std::endl;
}

void printNumber(int n) {
    std::cout << n << std::endl;
}

int main()
{
    FizzBuzz foo(31);

    std::thread threadA([&] {
                foo.fizz(printFizz);
                });
    std::thread threadB([&] {
                foo.buzz(printBuzz);
                });
    std::thread threadC([&] {
                foo.fizzbuzz(printFizzBuzz);
                });
    std::thread threadD([&] {
                foo.number(printNumber);
                });
    
    threadA.join();
    threadB.join();
    threadC.join();
    threadD.join();

    return(0);
}