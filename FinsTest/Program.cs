﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CableRobot;
using CableRobot.Fins;

namespace FinsTest
{
    public static class Program
    {
        public static void Main()
        {
            RunTests().Wait();
        }

        public static async Task RunTests()
        {
            using (var client = new FinsClient(new IPEndPoint(IPAddress.Parse("192.168.250.1"), 9600)))
            {
                Console.WriteLine("Writing -20.0 to %D800");
                client.WriteData(800, AngleSender.SerializeAngle(-20.0));
                Console.Write("Reading from %D800: ");
                Console.WriteLine(AngleSender.DeserializeAngle(client.ReadData(800, 4)));
                Console.WriteLine("Sleeping for 1s");
                Thread.Sleep(1000);

                Console.WriteLine("Writing 0.0 to %D800");
                client.WriteData(800, AngleSender.SerializeAngle(0.0));
                Console.Write("Reading from %D800: ");
                Console.WriteLine(AngleSender.DeserializeAngle(client.ReadData(800, 4)));
                Thread.Sleep(1000);

                Console.WriteLine("Writing -20.0 to %D800 (async)");
                await client.WriteDataAsync(800, AngleSender.SerializeAngle(-20.0));
                Console.Write("Reading from %D800 (async): ");
                Console.WriteLine(AngleSender.DeserializeAngle(await client.ReadDataAsync(800, 4)));
                Console.WriteLine("Sleeping for 1s");
                Thread.Sleep(1000);

                Console.WriteLine("Writing 0.0 to %D800 (async)");
                client.WriteData(800, AngleSender.SerializeAngle(0.0));
                Console.Write("Reading from %D800 (async): ");
                Console.WriteLine(AngleSender.DeserializeAngle(client.ReadData(800, 4)));
                Thread.Sleep(1000);

                Console.WriteLine("Writing -20.0 to %D800 (no response)");
                client.WriteDataNoResponse(800, AngleSender.SerializeAngle(-20.0));
                Thread.Sleep(1000);
                Console.Write("Reading from %D800 (async): ");
                Console.WriteLine(AngleSender.DeserializeAngle(await client.ReadDataAsync(800, 4)));
                Thread.Sleep(1000);

                Console.WriteLine("Writing 0.0 to %D800 (no response)");
                client.WriteDataNoResponse(800, AngleSender.SerializeAngle(0.0));
                Thread.Sleep(1000);
                Console.Write("Reading from %D800 (async): ");
                Console.WriteLine(AngleSender.DeserializeAngle(await client.ReadDataAsync(800, 4)));
                Thread.Sleep(1000);
            }

            Console.WriteLine("Test done~");
        }
    }
}