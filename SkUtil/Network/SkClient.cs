using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SkUtil.Network
{
    public  class SkClient
    {
        public SocketClient Client;
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

        private string IPValue = "192.168.219.220";
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
        #endregion

        public virtual void ClientStart(bool ClientType = true)
        {
            Client = new SocketClient(ClientType);
            Client.ReceiveData += new SocketClient.ReceiveDataEventHandler(Client_ReceiveData);
            Client.DisconnetServer += new SocketClient.DisConnectEventHandler(Client_DisconnetServer);
            Client.connetServer += new SocketClient.ConnectEventHandler(Client_connetServer);
            Client.connetFalse += new SocketClient.ConnectFalseEventHandler(Client_connetFalse);
            Client.Client_Connect(IP,Port);

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
        public void Send(byte[] data)
        {
            Client.OnlySendToServer(data);
        }
        //동기화 하기 위해서는 이 함수 써야함
        public byte[] SendToServer(byte[] data)
        {
            if (Client == null)
            {
                ClientStart();
            }
            return Client.SendToServer(data);
        }
        public virtual void Client_connetFalse(SocketClient _SocketClient)
        {
           // LogWriter.WriteLog("connetFalse IP : " + _SocketClient._ServerIP + " Port : " + _SocketClient._ServerPort);
        }
        public virtual void Client_connetServer(SocketClient _SocketClient)
        {
           // LogWriter.WriteLog("connetServer IP : " + _SocketClient._ServerIP + " Port : " + _SocketClient._ServerPort);
        }

        public virtual void Client_DisconnetServer(SocketClient _SocketClient)
        {
           // LogWriter.WriteLog("DisconnetServer IP : " + _SocketClient._ServerIP + " Port : " + _SocketClient._ServerPort);
        }
        public virtual void ControlClient_ReceiveData(NetworkStream ClientStream)
        {
        }
        public virtual void Client_ReceiveData(NetworkStream ClientStream)
        {
        }
    }
}
