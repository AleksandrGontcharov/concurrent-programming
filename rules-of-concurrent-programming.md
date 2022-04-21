# Rules of concurrent programming

## Rules of the road

To be applied in order.

1. Share nothing
2. Share immutable things
3. Always put the shared mutable resources under a lock - only one owner at a time



## Notes

1. Lock shared mutable resources
  --Note: sharing immutable things is fine
2. Never invoke methods inside of a lock scope whose code is not directly under your control including virtual methods. The implementation of those methods can be overridden (if it is a virtual method) or changed.
3. Use notify_all() vs notify_one(): It is harder to prove the correctness of such code in all cases and it doesn't hurt to use notify_all().
4. Do not use anything outside of the scope of what you have learned.
5. You should have wakeup/Pulse/Notify *every time* you change variables you use in wait conditions. This makes it easier to check for correctness.

## Other conventions 

1. Do not put notify_one() or notify_all() inside of a lock scope in C++ to make code reviewers happy, but do it in such a way that it doesn't compromise correctness. Putting notify in a lock is writing apriori suboptimal code since you cannot unlock that which is currently in a lock, so there will be several trips made to unlock it.


## Notes

1. The compiler and operating system and hardware can freely change the order of operations and execute reads and writes in a different order than you wrote it in your code. This is under the condition that the observable behavior of your code isn't affected in a single-threaded context. No such guarantees are done for a multi-threaded context. The concept of fencing is needed to explicitly write code that follows a certain order. The lock object is the simplest tool which automatically does all the "fencing". There are other more advanced tools.
