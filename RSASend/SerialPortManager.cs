using System;
using System.IO.Ports;

namespace RSASend
{
    public class SerialPortManager
    {
        private SerialPort _port;

        public bool IsOpen => _port?.IsOpen ?? false;

        public SerialPortManager(string portName, int baudRate = 115200, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            _port = new SerialPort(portName, baudRate, parity, dataBits, stopBits)
            {
                ReadTimeout = 500,
                WriteTimeout = 500
            };
        }

        public void Open()
        {
            if (!_port.IsOpen)
                _port.Open();
        }

        public void Close()
        {
            if (_port.IsOpen)
                _port.Close();
        }

        public void WriteBytes(byte[] data)
        {
            Open();
            _port.BaseStream.Write(data, 0, data.Length);
            _port.BaseStream.Flush();
        }

        public byte[] ReadBytes()
        {
            if (!_port.IsOpen)
                throw new InvalidOperationException("Serial port is not open.");
            int totalRead = _port.BytesToRead;
            byte[] buffer = new byte[totalRead];

            if (totalRead > 0)
            {
                int bytesRead = _port.BaseStream.Read(buffer, 0, totalRead);
                if (bytesRead == 0)
                    throw new TimeoutException("Timeout while reading from serial port.");
            }
            return buffer;
        }
    }

}