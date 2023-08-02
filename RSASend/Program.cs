using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;

namespace RSASend
{
    internal class Program
    {
        //Get %temp% folder path
        public static string tmp=Path.GetTempPath();
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0].ToLower().Contains("com"))//Check argument is only one and contains com
            {
                Console.WriteLine("\nEDL Port is : " + args[0]+"\nThe device must be in firehose mode");      
                //This part is used to check whether Port exists or not
                //If port exists, program continue
                //Goto Exception if not exist
                SerialPort p = new SerialPort(args[0].ToUpper());                
                try
                {
                    p.Open();
                    if (p.IsOpen)
                    {
                        p.Close();

                        Console.WriteLine("\n\nAuthenticating...");
                        IsACK(args[0].ToUpper()); //Send XML Auth and check respond contains "ACK" | if "ACK" not contains Auth Fail                        
                            Console.WriteLine("Sending Payload...");
                            SendAuth(args[0].ToUpper()); //Send Payload and edl authenticated
                            Console.WriteLine("\n--------------------\nEDL Authenticated\n--------------------");                      
                    }                                  
                }
                catch
                {
                    Console.WriteLine("\n--------------------\nPort does not exist\n--------------------");
                }
            }
            else
            {
                Console.WriteLine("No EDL port defined");               
            }
        }

        //This Method is used to send XML auth data and check "ACK"
        private static void IsACK(string port)
        {
            File.WriteAllText(tmp+@"\auth.xml", Properties.Resources.auth);
            SerialPort p = new SerialPort();
            p.PortName = port;
            p.Open(); //Serial port is opened
            byte[] auth = File.ReadAllBytes(tmp+@"\auth.xml"); //Convert XML String to Byte array
            int bc = auth.Length; //Get Length of byte array to write
            p.Write(auth, 0, bc); //Send Byte array to Serial Port
            int btr = p.BytesToRead; //Get Length of bytes to read
            byte[] res = new byte[btr]; //Convert Respond from Serial Port to byte array
            p.Read(res, 0, btr); //Read Respond from Serial Port
            string ack = Encoding.UTF8.GetString(res); //Convert byte array to string            
            p.Close();
            File.Delete(tmp+@"\auth.xml");             
        }
        private static void SendAuth(string port)
        {
            File.WriteAllBytes(tmp+@"\skip.bin", Properties.Resources.skip);
            SerialPort p = new SerialPort();
            p.PortName = port;
            p.Open();
            byte[] auth = File.ReadAllBytes(tmp+@"\skip.bin");//Convert payload file String to Byte array
            //the rest is same as above method
            int bc = auth.Length;
            p.Write(auth, 0, bc);
            int btr = p.BytesToRead;
            byte[] res = new byte[btr];
            p.Read(res, 0, btr);
            p.Close();
            File.Delete(tmp+@"\skip.bin");
        }
    }
}
