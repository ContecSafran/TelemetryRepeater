using SkUtil;
using SkUtil.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelemetryRepeater
{
    class Repeater
    {
       // FileStream fs1;
      //  FileStream fs2;
        byte[] Header = new byte[8];
        byte[] Data = null;
        byte[] TmBuffer= null;
        byte[] TmRequest = new byte[]{
                (byte) 0x49, (byte) 0x96, (byte) 0x02, (byte) 0xD2, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x40,
                (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
                (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
                (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
                (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
                (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
                (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
                (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0xB6, (byte) 0x69, (byte) 0xFD, (byte) 0x2E
        };
        public SocketClient Client;
        public event PropertyChangedEventHandler PropertyChanged;

        public Repeater()
        {
            CreateServer();
        }
        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #region Property

        private string ClientIPValue = "192.168.219.220";
        public string CrtIP
        {
            get
            {
                return ClientIPValue;
            }
            set
            {
                this.ClientIPValue = value;
                NotifyPropertyChanged();

            }
        }
        private int CrtPortValue = 3070;
        public int CrtPort
        {
            get
            {
                return CrtPortValue;
            }
            set
            {
                this.CrtPortValue = value;
                NotifyPropertyChanged();

            }
        }
        #endregion
        public void Start()
        {
            CreateServer();
            Server.ASyncStart(ServerPort);
        }
        public void Stop()
        {
            ServerStop();
            if (Client.isConnected())
            {
                Client.Disconnect();
            }
        }

        public virtual void ClientStart(bool ClientType = true)
        {
            Client = new SocketClient(ClientType);
            Client.ReceiveData += new SocketClient.ReceiveDataEventHandler(Client_ReceiveData);
            Client.DisconnetServer += new SocketClient.DisConnectEventHandler(Client_DisconnetServer);
            Client.connetServer += new SocketClient.ConnectEventHandler(Client_connetServer);
            Client.connetFalse += new SocketClient.ConnectFalseEventHandler(Client_connetFalse);
            Client.Client_Connect(CrtIP, CrtPort);

        }
        public void AddReceiveDataEventHandler(SocketClient.ReceiveDataEventHandler receiveDataEventHandler)
        {
            Client.ReceiveData += receiveDataEventHandler;
        }
        public virtual void ClientStop()
        {
            if (Client != null)
            {
                if (Client.isConnected())
                {
                    Client.Close();
                    Client = null;
                }
            }
        }

        //비동기 시 이함수 사용
        public void CrtSend(byte[] data)
        {
            Client.OnlySendToServer(data);
        }
        //동기화 하기 위해서는 이 함수 써야함
        public byte[] SendToCrt(byte[] data)
        {
            if (Client == null)
            {
                ClientStart();
            }
            return Client.SendToServer(data);
        }
        public virtual void Client_connetFalse(SocketClient _SocketClient)
        {
             Log.WriteLog("Client_connetFalse connetFalse IP : " + _SocketClient._ServerIP + " Port : " + _SocketClient._ServerPort);
        }
        public virtual void Client_connetServer(SocketClient _SocketClient)
        {
            Log.WriteLog("Client_connetServer connetServer IP : " + _SocketClient._ServerIP + " Port : " + _SocketClient._ServerPort);
        }

        public virtual void Client_DisconnetServer(SocketClient _SocketClient)
        {
            Log.WriteLog("Client_DisconnetServer DisconnetServer IP : " + _SocketClient._ServerIP + " Port : " + _SocketClient._ServerPort);
        }
        public void Client_ReceiveData(NetworkStream ClientStream)
        {
            if (!Client_ReceiveData(ClientStream, 0, Header, Header.Length))
            {
                return;
            }
            if (CT_Converter.ByteToInt32(Header, 0) != 1234567890)
            {
                return;
            }
            int size = CT_Converter.ByteToInt32(Header, 4);
            if(size <= 68)
            {
                return;
            }
            if(Data == null)
            {
                Data = new byte[size];
            }
            else if(Data.Length != size)
            {
                Data = new byte[size];
            }

            Buffer.BlockCopy(Header, 0, Data, 0, Header.Length);
            if (!Client_ReceiveData(ClientStream, Header.Length, Data, Data.Length - Header.Length))
            {
                return;
            }
            int TmBufferSize = CT_Converter.ByteToInt32(Data, 40);
            if (TmBuffer == null)
            {
                TmBuffer = new byte[TmBufferSize];
            }
            else if (TmBuffer.Length != TmBufferSize)
            {
                TmBuffer = new byte[TmBufferSize];
            }
            Buffer.BlockCopy(Data, 64, TmBuffer, 0, TmBuffer.Length);
            //fs1.Write(Data, 0, Data.Length);
           // fs2.Write(TmBuffer, 0, TmBuffer.Length);
            Server.SendToClientAll(TmBuffer);
        }

        public bool Client_ReceiveData(NetworkStream ClientStream,int offset, byte[] buffer, int readSize)
        {
            return ClientStream.Read(buffer, offset, readSize) == readSize;
        }



        public SocketServer Server;
        //내부 인터페이스
        public PacketQueue<byte[]> SocketSendQueue = new PacketQueue<byte[]>();
        public PacketQueue<ReceivePacketData> SocketRecvQueue = new PacketQueue<ReceivePacketData>();

        #region Property

        private int ServerPortValue = 3000;
        public int ServerPort
        {
            get
            {
                return ServerPortValue;
            }
            set
            {
                this.ServerPortValue = value;
                NotifyPropertyChanged();

            }
        }
        public int ClientCount
        {
            get
            {
                return this.Server.NowClientCount();
            }
        }
        #endregion
        public void CreateServer()
        {
            if (Server == null)
            {
                Server = new SocketServer(SocketSendQueue, SocketRecvQueue);
                Server._ConnectLimit = 1;
                Server._ServerPort = ServerPort;
                Server.ConnetClient += new SocketServer.ConnetEventHandler(Server_ConnetClient);
                Server.DisconnectClient += new SocketServer.DisconnectEventHandler(Server_DisconnectClient);
                Server.ReceiveClient += new SocketServer.ReceiveEventHandler(Server_ReceiveData);
            }
        }
        public void ServerStart()
        {
            CreateServer();
            Server.ASyncStart(ServerPort);
        }
        public void AddConnectClientEventHandler(SocketServer.ConnetEventHandler connetEventHandler)
        {
            Server.ConnetClient += connetEventHandler;
        }
        public void AddDisconnectClientEventHandler(SocketServer.DisconnectEventHandler disconnectEventHandler)
        {
            Server.DisconnectClient += disconnectEventHandler;
        }
        public void AddReceiveEventHandler(SocketServer.ReceiveEventHandler receiveEventHandler)
        {
            Server.ReceiveClient += receiveEventHandler;
        }
        public void SendToClient(byte[] data, string ClientID = "")
        {
            if (ClientID != "")
            {
                Server.SendToClient(ClientID, data);
            }
            else
            {
                Server.SendToClientAll(data);
            }
        }

        protected virtual void Server_ReceiveData(string ClientIP, int ClientPort, string ClientID, byte[] data)
        {
        }
        protected virtual void Server_ReceiveControlData(string ClientIP, int ClientPort, string ClientID, byte[] data)
        {
        }
        public void ServerStop()
        {
            if (Server != null)
            {
                Server.DisconnectAllClient();
                Server._listener.Stop();
            }

        }
        protected virtual void Server_DisconnectClient(string ClientIP, int ClientPort, string ClientID)
        {
            ClientStop();
            /*if (fs1 != null)
            {
                fs1.Close();
                fs1 = null;
            }
            if (fs2 != null)
            {
                fs2.Close();
                fs2 = null;
            }*/
            Log.WriteLog("Server_DisconnectClient DisconnectClient IP : " + ClientIP + " Port : " + ClientPort.ToString() + " ID : " + ClientID);
        }

        protected virtual void Server_ConnetClient(string ClientIP, int ClientPort, string ClientID)
        {

            Log.WriteLog("Server_ConnetClient ConnetClient IP : " + ClientIP + " Port : " + ClientPort.ToString() + " ID : " + ClientID);
            /*
            if (fs1 != null) {
                fs1.Close();
                fs1 = null;
            }
            if (fs2 != null)
            {
                fs2.Close();
                fs2 = null;
            }
            
            fs1 = File.OpenWrite(Directory.GetParent(Application.ExecutablePath) + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_receive.dat");
            fs2 = File.OpenWrite(Directory.GetParent(Application.ExecutablePath) + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_send.dat");*/
            ClientStart();
            Thread.Sleep(1000);
            CrtSend(TmRequest);
        }
    }
}
