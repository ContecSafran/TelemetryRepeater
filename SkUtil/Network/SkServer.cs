using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SkUtil.Network
{
    public class SkServer
    {
        public SocketServer Server;
        //내부 인터페이스
        public PacketQueue<byte[]> SocketSendQueue = new PacketQueue<byte[]>();
        public PacketQueue<ReceivePacketData> SocketRecvQueue = new PacketQueue<ReceivePacketData>();

        public event PropertyChangedEventHandler PropertyChanged;

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
        private string IPValue = "127.0.0.1";
        public string IP
        {
            get
            {
                return IPValue;
            }
            set
            {
                this.IPValue = value;
                NotifyPropertyChanged();

            }
        }

        private int PortValue = 3000;
        public int Port
        {
            get
            {
                return PortValue;
            }
            set
            {
                this.PortValue = value;
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
        public SkServer()
        {
            CreateServer();
        }
        public void CreateServer()
        {
            if (Server == null)
            {
                Server = new SocketServer(SocketSendQueue, SocketRecvQueue);
                //_Server._ConnectLimit = 50;
                Server._ServerPort = Port;
                Server.ConnetClient += new SocketServer.ConnetEventHandler(Server_ConnetClient);
                Server.DisconnectClient += new SocketServer.DisconnectEventHandler(Server_DisconnectClient);
                Server.ReceiveClient += new SocketServer.ReceiveEventHandler(Server_ReceiveData);
            }
        }
        public void ServerStart()
        {
            CreateServer();
            Server.ASyncStart(Port);
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
        public void Send(byte[] data, string ClientID = "")
        {
            if(ClientID != "")
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

           // LogWriter.WriteLog("[" + this.EuipmentName + "]DisconnectClient IP : " + ClientIP + " Port : " + ClientPort.ToString() + " ID : " + ClientID);
        }

        protected virtual void Server_ConnetClient(string ClientIP, int ClientPort, string ClientID)
        {

           // LogWriter.WriteLog("[" + this.EuipmentName + "]ConnetClient IP : " + ClientIP + " Port : " + ClientPort.ToString() + " ID : " + ClientID);
        }
    }
}
