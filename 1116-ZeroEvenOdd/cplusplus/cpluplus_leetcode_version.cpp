std::mutex gLock;
std::condition_variable gConditionVariable;

class ZeroEvenOdd {
private:
    int n;
    int state = 0;

public:
    ZeroEvenOdd(int n) {
        this->n = n;
    }

    // printNumber(x) outputs "x", where x is an integer.
    void zero(function<void(int)> printNumber) {

            for (int i = 1; i <= n; i++) {
                {
                    std::unique_lock<std::mutex> lock(gLock);
                    gConditionVariable.wait(lock, [&](){ return state == 0;});
                }

                printNumber(0);

                {
                    std::unique_lock<std::mutex> lock(gLock);
                    state = 1;
                }
                gConditionVariable.notify_all();
            }
        }

        void even(function<void(int)> printNumber) {
             for (int i = 1; i <= n; i++) {
                {
                    std::unique_lock<std::mutex> lock(gLock);
                    gConditionVariable.wait(lock, [&](){ return state == 1;});
                }
                if (i % 2 == 0) {
                    printNumber(i);
                }
                {
                    std::unique_lock<std::mutex> lock(gLock);
                    state = 2;
                }
                gConditionVariable.notify_all();
            }
        }
        void odd(function<void(int)> printNumber) {
             for (int i = 1; i <= n; i++) {
                {
                    std::unique_lock<std::mutex> lock(gLock);
                    gConditionVariable.wait(lock, [&](){ return state == 2;});
                }
                if (i % 2 == 1) {
                    printNumber(i);
                }
                {
                    std::unique_lock<std::mutex> lock(gLock);
                    state = 0;
                }
                gConditionVariable.notify_all();
            }
        }
};