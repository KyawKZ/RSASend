using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace RSASend
{
    internal class Program
    {
        //Get %temp% folder path
        public static string tmp = Path.GetTempPath();
        public static string xml = "<?xml version=\"1.0\" ?>\r\n<data>\r\n<sig TargetName=\"sig\" size_in_bytes=\"256\" verbose=\"1\"/>\r\n</data>\r\n";
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0].ToLower().Contains("com"))//Check argument is only one and contains com
            {
                SerialPortManager spm = new SerialPortManager(args[0].ToUpper());                
                Console.WriteLine("\nEDL Port is : " + args[0] + "\nThe device must be in firehose mode");                
                foreach (var authData in _xiaomiAuthData)
                {
                    spm.Open();
                    byte[] xmlBytes = Encoding.UTF8.GetBytes(xml);
                    spm.WriteBytes(xmlBytes);
                    Thread.Sleep(500);
                    byte[] responseBytes = spm.ReadBytes();
                    string xmlResponse = Encoding.UTF8.GetString(responseBytes);
                    Console.WriteLine("\n" + xmlResponse);
                    if (xmlResponse.Contains("ACK"))
                    {
                        byte[] authBytes = Convert.FromBase64String(authData);
                        spm.WriteBytes(authBytes);
                        Thread.Sleep(500);
                        byte[] authResponseBytes = spm.ReadBytes();
                        string authResponse = Encoding.UTF8.GetString(authResponseBytes);
                        Console.WriteLine("\n" + authResponse);
                        Thread.Sleep(500);
                        spm.Close();
                        if (authResponse.Contains("ACK"))
                        {
                            break;
                        }
                    }                   
                    else
                    {
                        Console.WriteLine("No ACK received,Abort...");
                        break;
                    }           
                }
            }
            else
            {
                Console.WriteLine("No EDL port defined");
            }
        }
        //Auth data
        private static readonly List<string> _xiaomiAuthData = new List<string>
        {
            "vzXWATo51hZr4Dh+a5sA/Q4JYoP4Ee3oFZSGbPZ2tBsaMupn" +
        "+6tPbZDkXJRLUzAqHaMtlPMKaOHrEWZysCkgCJqpOPkUZNaSbEKpPQ6uiOVJpJwA" +
        "/PmxuJ72inzSPevriMAdhQrNUqgyu4ATTEsOKnoUIuJTDBmzCeuh/34SOjTdO4Pc+s3ORfMD0TX+WImeUx4c9xVdSL/xirPl" +
        "/BouhfuwFd4qPPyO5RqkU/fevEoJWGHaFjfI302c9k7EpfRUhq1z+wNpZblOHuj0B3/7VOkK8KtSvwLkmVF" +
        "/t9ECiry6G5iVGEOyqMlktNlIAbr2MMYXn6b4Y3GDCkhPJ5LUkQ==",

        "k246jlc8rQfBZ2RLYSF4Ndha1P3bfYQKK3IlQy/NoTp8GSz6l57RZRfmlwsbB99sUW/sgfaWj89//dvDl6Fiwso" +
        "+XXYSSqF2nxshZLObdpMLTMZ1GffzOYd2d/ToryWChoK8v05ZOlfn4wUyaZJT4LHMXZ0NVUryvUbVbxjW5SkLpKDKwkMfnxnEwaOddmT" +
        "/q0ip4RpVk4aBmDW4TfVnXnDSX9tRI+ewQP4hEI8K5tfZ0mfyycYa0FTGhJPcTTP3TQzy1Krc1DAVLbZ8IqGBrW13YWN" +
        "/cMvaiEzcETNyA4N3kOaEXKWodnkwucJv2nEnJWTKNHY9NS9f5Cq3OPs4pQ=="
        };
    }
}
