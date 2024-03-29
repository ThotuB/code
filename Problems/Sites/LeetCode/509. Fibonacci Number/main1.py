def fibonacci(n):
    fib = [0, 1]

    i = 2
    while i <= n:
        fib.append(fib[i - 1] + fib[i - 2])
        i += 1
    
    return fib[n]