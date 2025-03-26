using System.Net.Sockets;
using System.Text.Json;

using TouchSenderInterpreter.Models;

namespace TouchSenderInterpreter.ConsoleAppDemo
{
    internal class Program
    {
        static readonly int port = 50000;
        static TouchSenderResult touchSenderResult = new();
        static async Task Main(string[] args)
        {
            using var cts = new CancellationTokenSource();
            var task = ReceiveTouchSenderPayload(cts.Token);
            Console.WriteLine("Press 'Q' to quit, 'C' to show the latest ZigSim data");
            int startRow = Console.GetCursorPosition().Top;

            var jsonOption = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            while (true)
            {
                // type 'Q' to quit
                var cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.Q)
                {
                    Console.WriteLine("Cancelling...");
                    cts.Cancel();
                    break;
                }
                if (cki.Key == ConsoleKey.C)
                {
                    ClearFromLine(startRow);

                    if (touchSenderResult.IsSuccess)
                    {

                        Console.WriteLine("Deserialized TouchSenderPayload:");
                        Console.WriteLine(JsonSerializer.Serialize(touchSenderResult, jsonOption));
                    }
                    else
                    {
                        Console.WriteLine($"Error: {touchSenderResult.ErrorMessage}");
                    }
                }
            }

            try
            {
                await task;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Task was cancelled");
            }
        }

        static async Task ReceiveTouchSenderPayload(CancellationToken cts)
        {
            using (var udpClient = new UdpClient(port))
            {
                while (true)
                {
                    var receivedResults = await udpClient.ReceiveAsync(cts);
                    touchSenderResult = Interpreter.Read(receivedResults.Buffer);
                }
            }
        }
        static void ClearFromLine(int startRow)
        {
            int currentRow = Console.GetCursorPosition().Top;
            for (int row = startRow; row <= currentRow; row++)
            {
                Console.SetCursorPosition(0, row);
                Console.Write(new string(' ', Console.WindowWidth)); // 空白で上書き
            }
            Console.SetCursorPosition(0, startRow); // カーソルを戻す
        }
    }
}
