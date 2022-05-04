#include <iostream>
#include <thread>
#include <mutex>
#include <condition_variable>
#include <functional>

using namespace std;

// Note: these should be private members of the class (not global variables) - it is not compiling for me when I put them there.
std::mutex _lock;
std::condition_variable cvFizz;
std::condition_variable cvBuzz;
std::condition_variable cvFizzBuzz;

class FizzBuzz {
private:
    int n;
    int h = 0;
    int o = 0;
    
    // int element_number = 0;
    bool divisibleBy3 = false;
    bool divisibleBy5 = false;

    bool fizzTurn = false;
    bool buzzTurn = false;
    bool fizzBuzzTurn = false;
    bool numberTurn = false;

public:
    FizzBuzz(int n) {
        this->n = n;
    }

    // printFizz() outputs "fizz".
    void fizz(function<void()> printFizz) {
         for (int i = 3; i <= n; i += 3) {
            divisibleBy3 = (i % 3 == 0);
            divisibleBy5 = (i % 5 == 0);

            if (divisibleBy3 && !divisibleBy5)
            {
                {
                    std::unique_lock<std::mutex> lock(_lock);
                    // std::cout << "fizz blocking itself " << i << std::endl;
                    cvFizz.wait(lock, [&](){ return fizzTurn;});
                }

                printFizz();                
                {
                    std::unique_lock<std::mutex> lock(_lock);
                    fizzTurn = false;
                    cvFizz.notify_all();
                }
            }
         }

    }

    // printBuzz() outputs "buzz".
    void buzz(function<void()> printBuzz) {
            for (int i = 5; i <= n; i += 5) {
            divisibleBy3 = (i % 3 == 0);
            divisibleBy5 = (i % 5 == 0);

            if (!divisibleBy3 && divisibleBy5) {
                {
                    std::unique_lock<std::mutex> lock(_lock);
                    // std::cout << "buzz blocking itself " << i << std::endl;
                    cvBuzz.wait(lock, [&](){ return buzzTurn;});
                }
                printBuzz();
                {
                    std::unique_lock<std::mutex> lock(_lock);
                    buzzTurn = false;
                    cvBuzz.notify_all();
                }
            }
         }
    }

    // printFizzBuzz() outputs "fizzbuzz".
	void fizzbuzz(function<void()> printFizzBuzz) {

        for (int i = 15; i <= n; i += 15) {
            divisibleBy3 = (i % 3 == 0);
            divisibleBy5 = (i % 5 == 0);

            if (divisibleBy3 && divisibleBy5) {
                {
                    std::unique_lock<std::mutex> lock(_lock);
                    // std::cout << "buzz blocking itself " << i << std::endl;
                    cvFizzBuzz.wait(lock, [&](){ return fizzBuzzTurn;});
                }
                printFizzBuzz();
                {
                    std::unique_lock<std::mutex> lock(_lock);
                    fizzBuzzTurn = false;
                    cvFizzBuzz.notify_all();
                }
            }
         }
    }

    // printNumber(x) outputs "x", where x is an integer.
    void number(function<void(int)> printNumber) {
        for (int i = 1; i <= n; i++) {
            divisibleBy3 = (i % 3 == 0);
            divisibleBy5 = (i % 5 == 0);
            
            fizzTurn = (divisibleBy3 && !divisibleBy5);
            buzzTurn = (!divisibleBy3 && divisibleBy5);
            fizzBuzzTurn = (divisibleBy3 && divisibleBy5);
            numberTurn = (!divisibleBy3 && !divisibleBy5);

            if (numberTurn) {
                printNumber(i);
            }
            else if (fizzTurn) {
                std::unique_lock<std::mutex> lock(_lock);
                // std::cout << "fizz turn so blocking myself" << std::endl;
                cvFizz.notify_all();
                cvFizz.wait(lock, [&](){ return !fizzTurn;});

            }
            else if (buzzTurn) {
                std::unique_lock<std::mutex> lock(_lock);
                // std::cout << "buzz turn so blocking myself" << std::endl;
                cvBuzz.notify_all();
                cvBuzz.wait(lock, [&](){ return !buzzTurn;});
            }
            else if (fizzBuzzTurn) {
                std::unique_lock<std::mutex> lock(_lock);
                // std::cout << "FizzBuzz turn so blocking myself" << std::endl;
                cvFizzBuzz.notify_all();
                cvFizzBuzz.wait(lock, [&](){ return !fizzBuzzTurn;});
            }

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
    FizzBuzz foo = FizzBuzz(31);

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