# TimerTest

## Usage

Usage: 
```sh
TimerTest ["timer"|"thread"] [objectcount] [objectsize]
```
Testing DotNet Timer frequency and accuracy.
In default mode **timer**, a ```System.Timers.Timer(msDelay)``` is used.
In mode **thread**, a busy thread is started which waits until the defined time passed by.
If **objectcount** and **size** are specified, the program is allocating and freeing objects of the given size as fast as possible.

### Examples

```sh
dotnet run 
```
Runs a standard DotNet timer of 10 ms duration with a busy loop in the main thread.

```sh
dotnet run thread 1000 100000
```
Runs a standard timer of 1 ms duration. The main loop allocates 1000 objects of 100000 Bytes size and frees it again as fast as possible to stress the garbage collector.

## Main observation under Windows for ```System.Timers.Timer(msDelay)```

A timer delay of 1ms in **timer** mode will not lead to 1000 calls/s! The windows timers are limited by the timer hardware interrupt which is by default programmed to a frequency of 64Hz or 15.625 ms (see https://randomascii.wordpress.com/2020/10/04/windows-timer-resolution-the-great-rule-change/)

With now memory allocation, the timers are firing very regularly every 15ms. Even if there is a busy loop on one of the CPUs.

Allocating and freeing many small objects (1000 objects with 1kB each) still works reasonably well. As soon as large objects are allocated and freed (1000 objects with 100kB each), the garbage collector blocks the entire program for several seconds from time to time.

Under linux, the timer frequency is typically 100Hz as opposed to 64Hz under windows.

## Other resources

If you need faster timers, have a look at the Multi Media Timers which allow quite precise an fast timers: https://stackoverflow.com/questions/24839105/high-resolution-timer-in-c-sharp

However, these timers will only run on a Windows machine.







