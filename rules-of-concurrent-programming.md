# Rules of concurrent programming

## Rules

1. Lock shared resources
2. Never invoke methods inside of a lock scope whose code is not directly under your control. The implementation of those methods can be overridden (if it is a virtual method) or changed. General rule of thumb: avoid putting methods in a lock scope.
3. Use notify_all() vs notify_one(): It is harder to prove the correctness of such code in all cases and it doesn't hurt to use notify_all().
4. Do not use anything outside of the scope of what you have learned.

## Other conventions 

1. Do not put notify_one() or notify_all() inside of a lock scope. This is writing apriori suboptimal code.


## Notes

1. The compiler and operating system and hardware can freely change the order of operations and execute reads and writes in a different order than you wrote it in your code. This is under the condition that the observable behavior of your code isn't affected in a single-threaded context. No such guarantees are done for a multi-threaded context. The concept of fencing is needed to explicitly write code that follows a certain order. The lock object is the simplest tool which automatically does all the "fencing". There are other more advanced tools.
