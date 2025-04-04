# TouchSenderInterpreter

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet Version](https://img.shields.io/nuget/v/Voltaney.TouchSenderInterpreter)](https://www.nuget.org/packages/Voltaney.TouchSenderInterpreter/)
[![CI](https://github.com/voltaney/TouchSenderInterpreter/actions/workflows/ci.yml/badge.svg)](https://github.com/voltaney/TouchSenderInterpreter/actions/workflows/ci.yml)
![dotnet Version](https://img.shields.io/badge/.NET-8.0-blueviolet)
![dotnet Version](https://img.shields.io/badge/.NET-9.0-blueviolet)

Parser library for data sent from the mobile application **Touch Sender**.

## Usage

Example of receiving data from **Touch Sender** via UDP and parsing its contents to display touch states.

```csharp
using System.Net;
using System.Net.Sockets;
using TouchSenderInterpreter;

namespace ConsoleApp1;

class Program
{
    static void Main(string[] args)
    {
        int port = 50000;
        IPEndPoint? senderEP = null;

        // Ctrl + C to quit
        using (var udpClient = new UdpClient(port))
        {
            while (true)
            {
                var recievedData = udpClient.Receive(ref senderEP);
                // parse recieved data of Touch Sender
                var result = Interpreter.Read(recievedData);
                if (result.IsSuccess)
                {
                    // print SingleTouchRatio
                    Console.WriteLine(result.Payload?.SingleTouchRatio);
                }
                else
                {
                    Console.WriteLine("Failed to parse Touch Sender data");
                    Console.WriteLine(result.ErrorMessage);
                }
            }
        }
    }
}
```

## Result Structure

If the JSON data is successfully parsed, a result object with the following structure is obtained.

```json
{
  "Payload": {
    "Id": 538,
    "DeviceInfo": {
      "Width": 360,
      "Height": 800
    },
    "SingleTouch": {
      "X": 248.33333333333334,
      "Y": 387
    }
  },
  "IsSuccess": true,
  "ErrorMessage": null
}
```
