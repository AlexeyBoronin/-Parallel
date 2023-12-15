//Метод Parallel.Invoke выполняет три метода
Parallel.Invoke(
    Print,
    () =>
    {
        Console.WriteLine($"Выполнение задач {Task.CurrentId}");
        Thread.Sleep(3000);
    },
    () => Square(7)
    );
void Print()
{
    Console.WriteLine($"Выполняется задача {Task.CurrentId}");
    Thread.Sleep(3000);
}
void Square(int n)
{
    Console.WriteLine($"Выполняется задача {Task.CurrentId}");
    Thread.Sleep(3000);
    Console.WriteLine($"Результат {n * n}");
}
Parallel.For(1, 5, Square);
ParallelLoopResult result = Parallel.ForEach<int>(
    new List<int>() { 1, 3, 5, 7, 9, 11 },
    Square);
ParallelLoopResult result1 = Parallel.For(1, 10, Square1);
if (!result1.IsCompleted)
    Console.WriteLine($"Выполнение цикла завершено на итерации {result1.LowestBreakIteration}");
void Square1(int n, ParallelLoopState pls)
{
    if (n == 6) pls.Break();
    Console.WriteLine($"Квадрат числа {n} равен {n * n}");
    Thread.Sleep(1000);
}