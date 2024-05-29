using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SkUtil
{
    public class CT_Converter
    {
        static public int StringObjectToInt32(object o)
        {
            string s = (string)o;
            return s.Equals("") ? 0 : Int32.Parse(s);
        }
        static public double StringObjectToDouble(object o)
        {
            string s = (string)o;
            return s.Equals("") ? 0 : Double.Parse(s);
        }

        public static UInt16 ReverseBytes(UInt16 value)
        {
            return (UInt16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }
        // reverse byte order (32-bit)
        public static UInt32 ReverseBytes(UInt32 value)
        {
            if (BitConverter.IsLittleEndian)
            {
                return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                       (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
            }
            return value;

        }
        public static int ReverseBytes(int value)
        {
            return ((int)(value & 0x000000FF)) << 24 |
                    ((int)(value & 0x0000FF00)) << 8 |
                    ((int)(value & 0x00FF0000)) >> 8 |
                    ((int)(value & 0xFF000000)) >> 24;
        }
        // reverse byte order (64-bit)
        public static UInt64 ReverseBytes(UInt64 value)
        {
            return (value & 0x00000000000000FFUL) << 56 | (value & 0x000000000000FF00UL) << 40 |
                   (value & 0x0000000000FF0000UL) << 24 | (value & 0x00000000FF000000UL) << 8 |
                   (value & 0x000000FF00000000UL) >> 8 | (value & 0x0000FF0000000000UL) >> 24 |
                   (value & 0x00FF000000000000UL) >> 40 | (value & 0xFF00000000000000UL) >> 56;
        }
        public static double ByteToDouble(byte[] _byteData, int _start = 0)
        {
            byte[] _rawdata = new byte[8];
            Buffer.BlockCopy(_byteData, _start, _rawdata, 0, 8);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(_rawdata);
            }
            return BitConverter.ToDouble(_rawdata, 0);
        }

        public static float ByteToFloat(byte[] _byteData, int _start = 0)
        {
            byte[] _rawdata = new byte[4];
            Buffer.BlockCopy(_byteData, _start, _rawdata, 0, 4);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(_rawdata);
            }
            return BitConverter.ToSingle(_rawdata, 0);
        }

        public static int ByteToInt32(byte[] _byteData, int _start = 0)
        {
            return (int)ByteToUInt32(_byteData, _start);
        }


        public static uint ByteToUInt32(byte[] _byteData, int _start = 0)
        {
            if (_byteData.Length == 1)
            {
                return (uint)_byteData[0];
            }
            uint _value = BitConverter.ToUInt32(_byteData, _start);
            if (BitConverter.IsLittleEndian)
            {
                _value = ReverseBytes(_value);
            }
            return _value;

        }

        public static long ByteToInt64(byte[] _byteData, int _start = 0)
        {
            return (long)ByteToUInt64(_byteData, _start);
        }

        public static ulong ByteToUInt64(byte[] _byteData, int _start = 0)
        {
            if (_byteData.Length == 1)
            {
                return (uint)_byteData[0];
            }
            ulong _value = BitConverter.ToUInt64(_byteData, _start);
            if (BitConverter.IsLittleEndian)
            {
                _value = ReverseBytes(_value);
            }
            return _value;
        }

        public static void DoubleToByte(double _value, ref byte[] _byteData, int _start = 0)
        {
            byte[] convertdata = BitConverter.GetBytes(_value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(convertdata);
            }
            Buffer.BlockCopy(convertdata, 0, _byteData, _start, 8);
        }

        public static void Int32ToByte(object _value, ref byte[] _byteData, int _start = 0)
        {
            byte[] convertdata = BitConverter.GetBytes(Convert.ToInt32(_value));
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(convertdata);
            }
            Buffer.BlockCopy(convertdata, 0, _byteData, _start, 4);
        }

        public static void Int64ToByte(object _value, ref byte[] _byteData, int _start = 0)
        {
            byte[] convertdata = BitConverter.GetBytes(Convert.ToInt64(_value));
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(convertdata);
            }
            Buffer.BlockCopy(convertdata, 0, _byteData, _start, 8);
        }

        public static void Int32ToByte(int _value, ref byte[] _byteData, int _start = 0)
        {
            byte[] convertdata = BitConverter.GetBytes(_value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(convertdata);
            }
            Buffer.BlockCopy(convertdata, 0, _byteData, _start, 4);
        }

        public static void UInt32ToByte(uint _value, ref byte[] _byteData, int _start = 0)
        {
            byte[] convertdata = BitConverter.GetBytes(_value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(convertdata);
            }
            Buffer.BlockCopy(convertdata, 0, _byteData, _start, 4);
        }

        public static byte[] UInt32ToByte(uint _value)
        {
            byte[] convertdata = BitConverter.GetBytes(_value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(convertdata);
            }
            return convertdata;
        }

        public static byte[] Int32ToByte(int _value)
        {
            byte[] convertdata = BitConverter.GetBytes(_value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(convertdata);
            }
            return convertdata;
        }
        public static void FloatToByte(object _value, ref byte[] _byteData, int _start = 0)
        {
            byte[] convertdata = BitConverter.GetBytes(Convert.ToSingle(_value));
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(convertdata);
            }
            Buffer.BlockCopy(convertdata, 0, _byteData, _start, 4);
        }
        public static void FloatToByte(float _value, ref byte[] _byteData, int _start = 0)
        {
            byte[] convertdata = BitConverter.GetBytes(_value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(convertdata);
            }
            Buffer.BlockCopy(convertdata, 0, _byteData, _start, 4);
        }

        public static byte[] FloatToByte(float _value)
        {
            byte[] convertdata = BitConverter.GetBytes(_value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(convertdata);
            }
            return convertdata;
        }
        public static string ByteToString(byte[] strByte, int _size = 0, int _start = 0)
        {
            string str = "";
            if (_size == 0)
            {
                str = Encoding.Default.GetString(strByte);
            }
            else
            {
                str = Encoding.Default.GetString(strByte, _start, _size);
            }

            return str;
        }
        // String을 바이트 배열로 변환
        public static void StringToByte(object _value, ref byte[] _byteData, int _size = 0, int _start = 0)
        {
            StringToByte((string)_value, ref _byteData, _size, _start);
        }

        public static void StringToByte(string _value, ref byte[] _byteData, int _start = 0)
        {
            byte[] StrByte = Encoding.UTF8.GetBytes(Convert.ToString(_value));
            Buffer.BlockCopy(StrByte, 0, _byteData, _start, StrByte.Length);
        }



        public static double Stream_ReadDouble(Stream _stream)
        {
            byte[] BodyData = new byte[8];
            if (_stream.Read(BodyData, 0, 8) == -1)
            {
                return 0;
            }
            return CT_Converter.ByteToDouble(BodyData, 0);
        }

        public static int Stream_ReadInt32(Stream _stream)
        {
            byte[] BodyData = new byte[4];
            if (_stream.Read(BodyData, 0, 4) == -1)
            {
                return 0;
            }
            return CT_Converter.ByteToInt32(BodyData, 0);
        }

        public static string Stream_ReadString(Stream _stream, int readlength)
        {
            int RcveCount = 0;
            byte[] BodyData = new byte[readlength];
            while (RcveCount < readlength)
            {
                int count = _stream.Read(BodyData, RcveCount, readlength - RcveCount);
                if (count == -1)
                {
                    return "";
                }
                RcveCount = RcveCount + count;
            }
            return Encoding.UTF8.GetString(BodyData);
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        public static byte[] HexToByte(string msg)
        {
            msg = msg.Replace(" ", "");
            if (msg.Length == 1)
            {

                byte[] comBuffer = new byte[1];
                comBuffer[0] = (byte)Convert.ToByte(msg, 16);
                return comBuffer;
            }
            else
            {
                byte[] comBuffer = new byte[msg.Length / 2];
                for (int i = 0; i < msg.Length; i += 2)
                {
                    try
                    {
                        comBuffer[i / 2] = (byte)Convert.ToByte(msg.Substring(i, 2), 16);
                    }
                    catch (ArgumentOutOfRangeException argumentoutofrange)
                    {
                    }
                }
                return comBuffer;
            }
        }
        public static byte[] ToBuffer<T>(T _Object)
        {

            int size = Marshal.SizeOf(_Object);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(_Object, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }
        public static void FromBuffer<T>(byte[] arr, ref T str, int _StartOffset = 0)
        {
            int size = Marshal.SizeOf(str);
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(arr, _StartOffset, ptr, size);

            str = (T)Marshal.PtrToStructure(ptr, str.GetType());
            Marshal.FreeHGlobal(ptr);
        }
    }
}
