using System.Diagnostics;

string exeName = "JustProgram.exe";

var semaphore = new Semaphore(1, 1, "MyLock");
string projectDirPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\", exeName));



while (true)
{
    await Task.Run(PrintValue);
}


async Task PrintValue()
{
    try
    {
        semaphore.WaitOne();

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n{Thread.CurrentThread.ManagedThreadId} catch lock");
        var result = await LaunchProgram(projectDirPath, string.Empty);
        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} {result}");

        Console.ForegroundColor = ConsoleColor.DarkGreen;
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    finally
    {
        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} release lock");
        semaphore.Release();
    }
}


Task<string> LaunchProgram(string path, string args)
{

    // Аргументы командной строки (если они нужны)

    // Создание объекта ProcessStartInfo для настройки параметров процесса
    ProcessStartInfo startInfo = new ProcessStartInfo
    {
        FileName = path,
        Arguments = args,
        RedirectStandardOutput = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    // Создание объекта Process с использованием настроек
    using (Process process = new Process { StartInfo = startInfo })
    {
        // Запуск процесса
        process.Start();

        // Ждем завершения процесса и получаем результат выполнения
        process.WaitForExit();
        return process.StandardOutput.ReadToEndAsync();
    }
}