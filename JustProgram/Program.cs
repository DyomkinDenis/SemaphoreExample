// See https://aka.ms/new-console-template for more information


await Task.Delay(1000);
if (args.FirstOrDefault() != null)
{
    Console.Write(args.FirstOrDefault() + " ");
}
Console.Write($"Program results:D");