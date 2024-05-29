using System;
using System.Net.Sockets;
using System.Threading;

namespace SkUtil.Network
{
	public class SocketClient
	{
		public string _ServerIP;

		public int _ServerPort;

		public TcpClient _Client = new TcpClient();

		public NetworkStream _ClientStream;

        public bool IsReceiveType = false;
        public bool _AsyncReceive = false;

        private PacketQueue<byte[]> _SendQueue = new PacketQueue<byte[]>();

        private PacketQueue<byte[]> _ReceiveQueue = new PacketQueue<byte[]>();
        private AutoResetEvent waitHandle;
        public SocketClient(bool _IsReceiveType)
		{
            IsReceiveType = _IsReceiveType;
            this._SendQueue.DATA_INSERT += new PacketQueue<byte[]>.InsertEventHandler(this._SendQueue_DATA_INSERT);
            if (IsReceiveType)
            {
                this._ReceiveQueue.DATA_INSERT += new PacketQueue<byte[]>.InsertEventHandler(this._ReceiveQueue_DATA_INSERT);
            }
		}

        private void _ReceiveQueue_DATA_INSERT()
        {
            try
            {
                if (this._ReceiveQueue != null)
                {
                    byte[] Packet = this._ReceiveQueue.Get_Packet();
                    if (Packet != null && this.ReceiveData != null)
                    {
                        this.ReceiveBuffer(Packet);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void _SendQueue_DATA_INSERT()
        {
            lock (this._SendQueue)
            {
                NetworkStream ClientStream = this._Client.GetStream();
                if (this._SendQueue != null && ClientStream.CanWrite)
                {
                    byte[] SendData = this._SendQueue.Get_Packet();
                    if (SendData.Length > 1460)
                    {
                        for (int i = 0; i < SendData.Length; i += 1460)
                        {
                            ClientStream.Write(SendData, i, 1460);
                        }

                    }
                    else
                    {

                        ClientStream.Write(SendData, 0, (int)SendData.Length);
                    }
                    ClientStream.Flush();
                }
            }
        }

		public bool Client_Connect(string ServerIp, int ServerPortNumber)
		{
			bool flag;
			try
			{
				this._ServerIP = ServerIp;
				this._ServerPort = ServerPortNumber;
				this._Client.BeginConnect(this._ServerIP, this._ServerPort, new AsyncCallback(this.ConnectCallback), this._Client);
				flag = true;
			}
			catch (Exception exception)
			{
				Console.WriteLine(string.Format("Connect Error : {0}", exception.Message));
				flag = false;
			}
			return flag;
		}

		private void ConnectCallback(IAsyncResult result)
        {
            if (this._Client.Connected)
            {
                waitHandle = new AutoResetEvent(true);
                waitHandle.Reset();
                this._ClientStream = this._Client.GetStream();
                this._ClientStream.BeginRead(BeginReadbuffer, 0, 0, new AsyncCallback(this.ReadCallback), BeginReadbuffer);
                if (this.connetServer != null)
                {
                    this.connetServer(this);
                }
            }
            else if (this.connetFalse != null)
            {
                this.connetFalse(this);
            }
		}
        public bool Client_Connect(string ServerIp, int ServerPortNumber, bool IsCallback = false)
		{
			bool flag;
			try
			{
                if (this._Client == null)
                {
                    this._Client = new TcpClient();
                }
				this._ServerIP = ServerIp;
				this._ServerPort = ServerPortNumber;
				this._Client.Connect(this._ServerIP, this._ServerPort);
                
				flag = true;
			}
			catch (Exception exception)
			{
				Console.WriteLine(string.Format("Connect Error : {0}", exception.Message));
				flag = false;
			}
			return flag;
		}

		public void Disconnect()
		{
			//this._Client.Client.Shutdown(SocketShutdown.Both);
			this._Client.Client.Disconnect(false);
            /*
			this._Client.Close();
			if (this.DisconnetServer != null)
			{
				this.DisconnetServer();
			}*/
		}
        public void Close()
        {
            this._Client.Client.Shutdown(SocketShutdown.Both);
            this._Client.Client.Disconnect(false);
            
			this._Client.Close();
			if (this.DisconnetServer != null)
			{
				this.DisconnetServer(this);
			}
            this._Client = null;
        }

		public bool isConnected()
		{
			if (this._Client == null)
			{
				return false;
			}
			return this._Client.Connected;
		}

        byte[] BeginReadbuffer = new byte[DefineData.CommonDefine.ZODIACHEADERSIZE];
		private void ReadCallback(IAsyncResult result)
        {

            try
            {

                bool b = waitHandle.Set();
                b = waitHandle.Reset();
                if (_AsyncReceive == false)
                {
                    NetworkStream ClientStream = _Client.GetStream();
                    this.ReceiveData(ClientStream);

                    ClientStream.Flush();
                }
                else
                {
                    waitHandle.WaitOne();
                }
                this._ClientStream.BeginRead(BeginReadbuffer, 0, 0, new AsyncCallback(this.ReadCallback), BeginReadbuffer);
            }
            catch (Exception ex)
            {
            }
		}

        //����ȭ �ϱ� ���ؼ��� �� �Լ� �����
        public byte[] SendToServer(byte[] _Data)
		{
			try
			{

                _AsyncReceive = true;
                NetworkStream ClientStream = this._Client.GetStream();
                ClientStream.WriteTimeout = 30000;
                ClientStream.ReadTimeout = 30000;
                if (this._SendQueue != null && ClientStream.CanWrite)
                {
                    ClientStream.Write(_Data, 0, (int)_Data.Length);
                    ClientStream.Flush();
                }
                waitHandle.WaitOne();
                ClientStream = _Client.GetStream();
                this._ClientStream = this._Client.GetStream();

                long length = 0;
                
                length = this._Client.Available;
                byte[] Packet = null;

                Packet = new byte[this._Client.Available];
                this._ClientStream.Read(Packet, 0, (int)length);
                _AsyncReceive = false;
                /*
                int cnt = 0;
                while (Packet != null ? Packet.Length == 0 : length == 0)
                {

                    length = this._Client.Available;
                    if (length != 0)
                    {
                        Packet = new byte[this._Client.Available];
                        ClientStream.Read(Packet, 0, (int)length);
                    }
                    cnt++;
                    Thread.Sleep(10);
                    if(cnt >= 10)
                    {
                        break;
                    }
                }
                */
                ClientStream.Flush();
                waitHandle.Reset();
                return Packet;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
                return null;
			}
		}

        //�񵿱� �� ���Լ� ���
        public void OnlySendToServer(byte[] _Data)
        {
            try
            {
                if (this._SendQueue != null)
                {
                    this._SendQueue.Add_Packet(_Data);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
		public event SocketClient.ConnectFalseEventHandler connetFalse;

		public event SocketClient.ConnectEventHandler connetServer;

		public event SocketClient.DisConnectEventHandler DisconnetServer;

        public event SocketClient.ReceiveDataEventHandler ReceiveData;
        public event SocketClient.ReceiveBufferEventHandler ReceiveBuffer;

        public delegate void ConnectEventHandler(SocketClient _SocketClient);

        public delegate void ConnectFalseEventHandler(SocketClient _SocketClient);

        public delegate void DisConnectEventHandler(SocketClient _SocketClient);

        public delegate void ReceiveDataEventHandler(NetworkStream ClientStream);


        public delegate void ReceiveBufferEventHandler(byte [] _Buffer);
	}
}
