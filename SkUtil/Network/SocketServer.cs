using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;


namespace SkUtil.Network
{
	public class SocketServer
	{
		public TcpListener _listener;

		public ConcurrentList<SocketServer_Client> _ClientList = new ConcurrentList<SocketServer_Client>();

        private PacketQueue<byte[]> _SendQueue;

        private PacketQueue<ReceivePacketData> _ReceiveQueue;

        public bool isOnlyConnectLocalClient = false;

		public int _Option = -1;

		public int _ConnectLimit
		{
			get;
			set;
		}

		public int _ServerPort
		{
			get;
			set;
		}

        public SocketServer(PacketQueue<byte[]> SQueue, PacketQueue<ReceivePacketData> RQueue)
		{
			this._ConnectLimit = 1000;
			this._SendQueue = SQueue;
			this._ReceiveQueue = RQueue;
            this._SendQueue.DATA_INSERT += new PacketQueue<byte[]>.InsertEventHandler(this._SendQueue_DATA_INSERT);
		}

		private void _SendQueue_DATA_INSERT()
		{
			try
			{
				this.ClientChecker();
				if (this._SendQueue.Get_PacketCount() != 0 && this._ClientList.Count != 0)
				{
                    byte[] Packet = this._SendQueue.Get_Packet();
					foreach (SocketServer_Client TargetClient in this._ClientList)
					{
						if (!TargetClient._Client.Connected)
						{
							continue;
						}
						TargetClient.SendToClient(Packet);
					}
				}
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
			}
		}

		public void AcceptTcpClientCallback(IAsyncResult ar)
		{
			TcpListener listener = (TcpListener)ar.AsyncState;
			string ClientID = Guid.NewGuid().ToString();
            TcpClient client = listener.EndAcceptTcpClient(ar);
            IPEndPoint ipInfo = (IPEndPoint)client.Client.RemoteEndPoint;
            if (isOnlyConnectLocalClient)
            {
                if (ipInfo.Address.ToString().IndexOf("127.0.0.1") != 0)
                {
                    client.Client.Shutdown(SocketShutdown.Both);
                    client.Client.Disconnect(true);
                    //
                    this._listener.BeginAcceptTcpClient(new AsyncCallback(this.AcceptTcpClientCallback), this._listener);
                    return;
                }
            }
			SocketServer_Client Client = new SocketServer_Client(this._ReceiveQueue, ClientID)
			{
                _Client = client
			};
            Client.ReceiveClient += new SocketServer_Client.ReceiveEventHandler(this.Client_ReceiveClient);
			Client.DisconnetClient += new SocketServer_Client.DisConnetEventHandler(this.Client_DisconnetClient);
			Client.SetOption += new SocketServer_Client.SetOptionHandler(this.Client_SetOption);
			Client.GetOption += new SocketServer_Client.GetOptionHandler(this.Client_GetOption);
			this.ClientChecker();
			if (this._ClientList.Count >= this._ConnectLimit)
			{
                IPEndPoint remoteEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
				Client.Disconnect();
			}
			else
			{
                Client._ClientStream = client.GetStream();

				Client.ReceiveStart();
				this._ClientList.Add(Client);
                this.Connected_Client(ipInfo.Address.ToString(), ipInfo.Port, ClientID);
			}
			this._listener.BeginAcceptTcpClient(new AsyncCallback(this.AcceptTcpClientCallback), this._listener);
		}

        void Client_ReceiveClient(SocketServer_Client TargetClient)
        {
            IPEndPoint ClientIP = (IPEndPoint)TargetClient._Client.Client.RemoteEndPoint;;

            long length = TargetClient._Client.Available;
            byte[] RcvHeaderData = new byte[length];
            TargetClient._ClientStream.Read(RcvHeaderData, 0, (int)length);
            this.Receive_Client(ClientIP.Address.ToString(), ClientIP.Port, TargetClient._ClientID, RcvHeaderData);
        }

		public void ASyncStart(int PortNumber)
		{
			this._ServerPort = PortNumber;
			this._ClientList.Clear();
			if (this._listener != null)
			{
				this._listener.Stop();
				this._listener = null;
			}
			this._listener = new TcpListener(IPAddress.Any, this._ServerPort);
			this._listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			this._listener.Start();
			this._listener.BeginAcceptTcpClient(new AsyncCallback(this.AcceptTcpClientCallback), this._listener);
		}

		public void BlockClient(string ClientIP, int ClientPort)
		{
			foreach (SocketServer_Client TargetClient in this._ClientList)
			{
				IPEndPoint ClientAdress = (IPEndPoint)TargetClient._Client.Client.RemoteEndPoint;
				if (!(ClientIP == ClientAdress.Address.ToString()) || ClientPort != ClientAdress.Port)
				{
					continue;
				}
				TargetClient.Disconnect();
				return;
			}
		}

		private void Client_DisconnetClient(SocketServer_Client TargetClient)
		{
			IPEndPoint ClientIP = (IPEndPoint)TargetClient._Client.Client.RemoteEndPoint;
			if (TargetClient._ClientStream != null)
			{
				TargetClient._ClientStream = null;
			}
			TargetClient.DisConnectServer();
			this._ClientList.Remove(TargetClient);
			this.ClientChecker();
			this.Disconnect_Client(ClientIP.Address.ToString(), ClientIP.Port, TargetClient._ClientID);
		}

		private int Client_GetOption()
		{
			return this._Option;
		}

		private void Client_SetOption(int _op)
		{
			this._Option = _op;
		}

		private void ClientChecker()
		{
			while (true)
			{
				SocketServer_Client DeleteTarget = null;
				foreach (SocketServer_Client TargetClient in this._ClientList)
				{
					if (TargetClient._Client.Connected)
					{
						continue;
					}
					DeleteTarget = TargetClient;
					break;
				}
				if (DeleteTarget == null)
				{
					break;
				}
				this._ClientList.Remove(DeleteTarget);
			}
		}

		private void Connected_Client(string ClientIP, int ClientPort, string ClientID)
		{
			if (this.ConnetClient != null)
			{
				this.ConnetClient(ClientIP, ClientPort, ClientID);
			}
		}

		private void Disconnect_Client(string ClientIP, int ClientPort, string ClientID)
		{
			if (this.DisconnectClient != null)
			{
				this.DisconnectClient(ClientIP, ClientPort, ClientID);
			}
		}

        private void Receive_Client(string ClientIP, int ClientPort, string ClientID, byte [] data)
        {
            if (this.ReceiveClient != null)
            {
                this.ReceiveClient(ClientIP, ClientPort, ClientID, data);
            }
        }
		public void DisconnectAllClient()
		{
			foreach (SocketServer_Client TargetClient in this._ClientList)
			{
				TargetClient.Disconnect();
			}
		}

		public bool IsConnectNow(int ClientPort)
		{
			bool flag;
			using (IEnumerator<SocketServer_Client> enumerator = this._ClientList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SocketServer_Client TargetClient = enumerator.Current;
					if (ClientPort != ((IPEndPoint)TargetClient._Client.Client.RemoteEndPoint).Port || !TargetClient._Client.Connected)
					{
						continue;
					}
					flag = true;
					return flag;
				}
				return false;
			}
			//return flag;
		}

		public bool IsConnectNow(string ClientID)
		{
			bool flag;
			using (IEnumerator<SocketServer_Client> enumerator = this._ClientList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SocketServer_Client TargetClient = enumerator.Current;
					if (!(ClientID == TargetClient._ClientID) || !TargetClient._Client.Connected)
					{
						continue;
					}
					flag = true;
					return flag;
				}
				return false;
			}
			//return flag;
		}

		public int NowClientCount()
		{
			this.ClientChecker();
			return this._ClientList.Count;

		}

        public void SendToClient(string ClientID, byte[] _Data)
		{
			try
			{
				foreach (SocketServer_Client TargetClient in this._ClientList)
				{
					if (TargetClient._ClientID != ClientID)
					{
						continue;
					}
                    TargetClient.SendToClient(_Data);
					return;
				}
				Console.WriteLine("일치하는 클라이언트 정보를 찾지 못했습니다.");
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
			}
		}

        public void SendToClientAll(byte[] _Data)
		{
			this.ClientChecker();
			if (this._SendQueue != null && this._ClientList.Count != 0)
			{
                this._SendQueue.Add_Packet(_Data);
			}
		}

		public event SocketServer.ConnetEventHandler ConnetClient;

        public event SocketServer.ReceiveEventHandler ReceiveClient;

		public event SocketServer.DisconnectEventHandler DisconnectClient;

        public delegate void ReceiveEventHandler(string ClientIP, int ClientPort, string ClientID, byte [] data);

		public delegate void ConnetEventHandler(string ClientIP, int ClientPort, string ClientID);

		public delegate void DisconnectEventHandler(string ClientIP, int ClientPort, string ClientID);
	}
}
