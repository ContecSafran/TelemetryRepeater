using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SkUtil.Network
{
	public class SocketServer_Client
	{
		public TcpClient _Client;

		public string _ClientID;

		public NetworkStream _ClientStream;

		public PacketQueue<byte[]> _SendQueue;

        private PacketQueue<ReceivePacketData> _ReceiveQueue;
        
        byte[] buffer = new byte[20];
        public SocketServer_Client(PacketQueue<ReceivePacketData> ReceiveQueue, string ClientID)
		{
            this._SendQueue = new PacketQueue<byte[]>();
            this._SendQueue.DATA_INSERT += new PacketQueue<byte[]>.InsertEventHandler(this._SendQueue_DATA_INSERT);
			this._ReceiveQueue = ReceiveQueue;
			this._ClientID = ClientID;
		}

		private void _SendQueue_DATA_INSERT()
		{
			try
			{
				lock (this._SendQueue)
				{
					NetworkStream ClientStream = this._Client.GetStream();
					if (this._SendQueue != null && ClientStream.CanWrite)
					{
                        byte[] SendData = this._SendQueue.Get_Packet();
						ClientStream.Write(SendData, 0, (int)SendData.Length);
						ClientStream.Flush();
					}
				}
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
			}
		}

		public void Disconnect()
		{
			if (this.DisconnetClient != null)
			{
				this.DisconnetClient(this);
			}
		}

		public void DisConnectServer()
		{
			this._Client.Client.Shutdown(SocketShutdown.Both);
			this._Client.Client.Disconnect(false);
			this._Client.Close();
		}

		private void ReadCallback(IAsyncResult result)
		{
			try
			{




                this._ClientStream = this._Client.GetStream();
                if (!(this._ClientStream == null ? true : !this._ClientStream.DataAvailable))
                {
                    //    int Recv = 0;

                    //    byte[] ZodiacHeader = new byte[DefineData.CommonDefine.ZODIACHEADERSIZE];
                    //    Recv = this._ClientStream.Read(ZodiacHeader, 0, ZodiacHeader.Length);
                    //    //��� ������ ��ŭ �޾Ҵ��� ���� ó�� �������
                    //    if (Recv != ZodiacHeader.Length)
                    //    {
                    //        return;
                    //    }

                    //    int _header = CT_Converter.ByteToInt32(ZodiacHeader, 0);
                    //    int _size = CT_Converter.ByteToInt32(ZodiacHeader, 4);
                    //    int _code = CT_Converter.ByteToInt32(ZodiacHeader, 12);
                    //    byte[] _Buffer = new byte[_size];

                    //    Buffer.BlockCopy(ZodiacHeader, 0, _Buffer, 0, Recv);

                    //    while (Recv < _size)
                    //    {
                    //        Recv += this._ClientStream.Read(_Buffer, Recv, _size - Recv);
                    //    }
                    //    ReceivePacketData _PacketData = new ReceivePacketData();
                    //    IPEndPoint remoteEndPoint = (IPEndPoint)this._Client.Client.RemoteEndPoint;
                    //    _PacketData._ClientIP = remoteEndPoint.Address.ToString();
                    //    _PacketData._ClientID = this._ClientID;
                    //    _PacketData._ClientPort = remoteEndPoint.Port;
                    //    _PacketData._ReceiveData = _Buffer;
                    //    this._ReceiveQueue.Add_Packet(_PacketData);
                    //}
                    int d = 0;

                    if (ReceiveClient != null)
                    {
                        ReceiveClient(this);
                    }
                    byte[] BeginReadbuffer = new byte[DefineData.CommonDefine.ZODIACHEADERSIZE];

                    this._ClientStream.BeginRead(BeginReadbuffer, 0, 0, new AsyncCallback(this.ReadCallback), BeginReadbuffer);
                }
                else
                {
                    if (this.DisconnetClient != null)
                    {
                    	this.DisconnetClient(this);
                    }
                }
			}
			catch (Exception exception)
			{
				//Console.WriteLine(string.Concat("GetStream Error : ", exception.Message));
			}
// 			DefineData.KPAPacketHeader _Header = DefineData.KPAPacketHeader.ReadHeaderPacket(this._ClientStream);
// 			if (_Header != null)
// 			{
// 				Console.WriteLine("Recv Packet Header");
// 				DefineData.PacketData _ReadData = null;
// 				if (_Header._Command == DefineData.TranslateCommand.REQ_Bleu)
// 				{
// 					DefineData.KPAPacketRequest TransData = new DefineData.KPAPacketRequest()
// 					{
// 						_Header = _Header
// 					};
// 					if (TransData.ByteToPacket(this._ClientStream))
// 					{
// 						Console.WriteLine("Recv Packet REQ_Bleu");
// 						if (this.GetOption() == -1)
// 						{
// 							_ReadData = TransData;
// 						}
// 						else
// 						{
// 							DefineData.KPAPacketResult kPAPacketResult = new DefineData.KPAPacketResult(DefineData.TranslateCommand.RSP_BleuResult);
// 							byte[] buffer = new byte[DefineData.CommonDefine.TRANSLATE_PACKETSIZE];
// 							this._ClientStream.BeginRead(buffer, 0, 0, new AsyncCallback(this.ReadCallback), buffer);
// 						}
// 					}
// 				}
// 				if (_Header._Command == DefineData.TranslateCommand.REQ_Bleu_ApplicationNum)
// 				{
// 					DefineData.KPAApplicationNumPacketRequest TransData = new DefineData.KPAApplicationNumPacketRequest()
// 					{
// 						_Header = _Header
// 					};
// 					if (TransData.ByteToPacket(this._ClientStream))
// 					{
// 						Console.WriteLine("Recv Packet REQ_Bleu_ApplicationNum");
// 						DefineData.KPAApplicationNumPacketResponse _ResultPacket = new DefineData.KPAApplicationNumPacketResponse(DefineData.TranslateCommand.RSP_Bleu_ApplicationNumResult);
// 						if (this.GetOption() == -1)
// 						{
// 							_ReadData = TransData;
// 						}
// 						else
// 						{
// 							_ResultPacket._ResultFlag = 0;
// 							byte[] buffer = new byte[DefineData.CommonDefine.TRANSLATE_PACKETSIZE];
// 							this._ClientStream.BeginRead(buffer, 0, 0, new AsyncCallback(this.ReadCallback), buffer);
// 						}
// 						this.SendToClient(_ResultPacket);
// 					}
// 				}
// 				if (_Header._Command == DefineData.TranslateCommand.REQ_SentenceFileDBUpdate)
// 				{
// 					DefineData.KPASentenceFileDBUpdatePacketRequest _Packet = new DefineData.KPASentenceFileDBUpdatePacketRequest()
// 					{
// 						_Header = _Header
// 					};
// 					if (_Packet.ByteToPacket(this._ClientStream))
// 					{
// 						if (this.GetOption() == -1)
// 						{
// 							this.SetOption(Convert.ToInt32(_Packet._Option));
// 						}
// 						_ReadData = null;
// 						Console.WriteLine("Recv Packet REQ_SentenceFileDBUpdate");
// 						byte[] buffer = new byte[DefineData.CommonDefine.TRANSLATE_PACKETSIZE];
// 						this._ClientStream.BeginRead(buffer, 0, 0, new AsyncCallback(this.ReadCallback), buffer);
// 					}
// 				}
// 				try
// 				{
// 					if (_ReadData != null)
// 					{
// 						IPEndPoint ClientIP = (IPEndPoint)this._Client.Client.RemoteEndPoint;
// 						_ReadData._ClientIP = ClientIP.Address.ToString();
// 						_ReadData._ClientID = this._ClientID;
// 						_ReadData._ClientPort = ClientIP.Port;
// 						this._ReceiveQueue.Add_Packet(_ReadData);
// 						byte[] buffer = new byte[DefineData.CommonDefine.TRANSLATE_PACKETSIZE];
// 						this._ClientStream.BeginRead(buffer, 0, 0, new AsyncCallback(this.ReadCallback), buffer);
// 					}
// 				}
// 				catch (Exception exception1)
// 				{
// 					Console.WriteLine(exception1.Message);
// 				}
// 			}
// 			else if (this.DisconnetClient != null)
// 			{
// 				this.DisconnetClient(this);
// 			}
		}

		public bool ReceiveStart()
		{
			bool flag;
			try
			{
                this._ClientStream = this._Client.GetStream();
                byte[] BeginReadbuffer = new byte[DefineData.CommonDefine.ZODIACHEADERSIZE];
                this._ClientStream.BeginRead(BeginReadbuffer, 0, 0, new AsyncCallback(this.ReadCallback), BeginReadbuffer);
				flag = true;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
				flag = false;
			}
			return flag;
		}

        public void SendToClient(byte[] Packet)
		{
			this._SendQueue.Add_Packet(Packet);
		}

        public event SocketServer_Client.DisConnetEventHandler DisconnetClient;

        public event SocketServer_Client.ReceiveEventHandler ReceiveClient;

		public event SocketServer_Client.GetOptionHandler GetOption;

		public event SocketServer_Client.SetOptionHandler SetOption;

		public delegate void DisConnetEventHandler(SocketServer_Client TargetClient);



        public delegate void ReceiveEventHandler(SocketServer_Client TargetClient);


		public delegate int GetOptionHandler();

		public delegate void SetOptionHandler(int _Option);
	}
}