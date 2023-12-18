//Метод Parallel.Invoke выполняет три метода
using System;

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
//Отмена задач и параллельных операций.
//Мягкий выход без исключений
/*CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
CancellationToken token=cancelTokenSource.Token;

Task task = new Task(() =>
{
    for (int i = 1; i < 10; i++)
    {
        if (token.IsCancellationRequested)
        {
            Console.WriteLine("Операция прервана");
            return;
        }
        Console.WriteLine($"Квадрат числа {i} равен {i * i}");
        Thread.Sleep(200);
    }
}, token);
task.Start();
Thread.Sleep(1000);
cancelTokenSource.Cancel();
Thread.Sleep(1000);
Console.WriteLine($"Task Status: {task.Status}");
cancelTokenSource.Dispose();*/
//Отмена задачи с помощью генерации исключения
/*CancellationTokenSource cancelTokenSource1 = new CancellationTokenSource();
CancellationToken token1 = cancelTokenSource1.Token;
Task task1 = new Task(() =>
{
    for (int i = 1; i < 10; i++)
    {
        if (token1.IsCancellationRequested)
            token1.ThrowIfCancellationRequested();
        Console.WriteLine($"Квадрат числа {i} равен {i * i}");
        Thread.Sleep(200);
    }
}, token1);
try
{
    task1.Start();
    Thread.Sleep(1000);
    cancelTokenSource1.Cancel();
    Thread.Sleep(10000);
}
catch (AggregateException ae)
{
    foreach (Exception e in ae.InnerExceptions)
    {
        if (e is TaskCanceledException)
            Console.WriteLine("Операция прервана");
        else
            Console.WriteLine(e.Message);
    }
}
finally
{
    cancelTokenSource1.Dispose();
}
Console.WriteLine($"Task Status: {task1.Status}");*/
/*CancellationTokenSource cancelTokenSource2 = new CancellationTokenSource();
CancellationToken token2 = cancelTokenSource2.Token;
Task task2 = new Task(() =>
{
    int i = 1;
    token2.Register(() =>
    {
        Console.WriteLine("Опреция прервана");
        i = 10;
    });
    for (; i < 10; i++)
    {
        Console.WriteLine($"Квадрат числа {i} равен {i * i}");
        Thread.Sleep(200);
    }
}, token2);

task2.Start();
Thread.Sleep(1000);
cancelTokenSource2.Cancel();
Thread.Sleep(1000);
cancelTokenSource2.Dispose();
Console.WriteLine($"Task Status: {task2.Status}");*/
//Передача токена во внешний метод
/*CancellationTokenSource cancelTokenSource1 = new CancellationTokenSource();
CancellationToken token1= cancelTokenSource1.Token;

Task task1 = new Task(() =>PrintSquares(token1),token1);
try
{
    task1.Start();
    Thread.Sleep(1000);
    cancelTokenSource1.Cancel();

    task1.Wait();
}
catch(AggregateException ae)
{
    foreach(Exception e in ae.InnerExceptions)
    {
        if (e is TaskCanceledException)
            Console.WriteLine("Операция прервана");
        else
            Console.WriteLine(e.Message);
    }
}
finally
{
    cancelTokenSource1.Dispose();
}
Console.WriteLine($"Task Status: {task1.Status}");
void PrintSquares(CancellationToken token)
{
    for(int i = 1;i < 10;i++) 
    {
        if (token.IsCancellationRequested)
            token.ThrowIfCancellationRequested();
        Console.WriteLine($"Квадрат числа {i} равен {i * i}");
        Thread.Sleep(200);
    }
}*/
//Отмена параллельных операций Parallel
CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
CancellationToken token = cancelTokenSource.Token;
new Task(() =>
{
    Thread.Sleep(400);
    cancelTokenSource.Cancel();
}).Start();
try
{
    Parallel.ForEach<int>(new List<int>() { 1, 2, 3, 4, 5, 6 }, new ParallelOptions { CancellationToken = token }, Square3);
    //Parallel.For(1,6,new ParallelOptions {CancellationToken = token},Square3);
}
catch(OperationCanceledException)
{
    Console.WriteLine("Операция прервана");
}
finally
{
    cancelTokenSource.Dispose();
}
void Square3(int n)
{
    Thread.Sleep(3000);
    Console.WriteLine($"Квадрат числа {n} равен {n * n}");
}