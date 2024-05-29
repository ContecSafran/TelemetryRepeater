using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace SkUtil.Network
{
	public class DefineData
	{
		public DefineData()
		{
		}

		public class CommonDefine
		{
			public readonly static string PASS_KEY;

			public readonly static int ZODIACHEADERSIZE;

			public readonly static int ProcessStackSize;

			public readonly static int TRANSLATE_TIMEOUTVALUE;

			static CommonDefine()
			{
				DefineData.CommonDefine.PASS_KEY = "c^s#l$ie!efs&dfe";
				DefineData.CommonDefine.ZODIACHEADERSIZE = 16;
				DefineData.CommonDefine.ProcessStackSize = 10485760;
				DefineData.CommonDefine.TRANSLATE_TIMEOUTVALUE = 30000;
			}

			public CommonDefine()
			{
			}
		}

		[Serializable]
		public class CoreInfo
		{
			public string _ProcessIP
			{
				get;
				set;
			}

			public int _ProcessPort
			{
				get;
				set;
			}

			public CoreInfo()
			{
			}
		}

		public class DomainDefine
		{
			public readonly static int DOMAIN_TECH;

			public readonly static int DOMAIN_PATENT;

			static DomainDefine()
			{
				DefineData.DomainDefine.DOMAIN_TECH = 0;
				DefineData.DomainDefine.DOMAIN_PATENT = 1;
			}

			public DomainDefine()
			{
			}
		}

		public class DomainLevel
		{
			public readonly static int DOMAIN_REF_SELF;

			public readonly static int CK_DOMAIN_REF_MUTUAL;

			static DomainLevel()
			{
				DefineData.DomainLevel.DOMAIN_REF_SELF = 0;
				DefineData.DomainLevel.CK_DOMAIN_REF_MUTUAL = 1;
			}

			public DomainLevel()
			{
			}
		}


	}


    public class ServerSendPacketData
    {
        public bool AllSendData = false;
        public string _ClientID
        {
            get;
            set;
        }
        public byte[] _SendData;
    }
    public class ReceivePacketData
    {

        public string _ClientID
        {
            get;
            set;
        }

        public string _ClientIP
        {
            get;
            set;
        }

        public int _ClientPort
        {
            get;
            set;
        }
        public byte[] _ReceiveData;
    }
}