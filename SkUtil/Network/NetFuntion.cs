using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SkUtil.Network
{
	public class NetFuntion
	{
		public NetFuntion()
		{
		}

		public static string GetHostIP()
		{
			IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
			string empty = string.Empty;
			IPAddress[] addressList = host.AddressList;
			for (int i = 0; i < (int)addressList.Length; i++)
			{
				IPAddress TargetAdress = addressList[i];
				if (TargetAdress.AddressFamily == AddressFamily.InterNetwork)
				{
					return TargetAdress.ToString();
				}
			}
			return null;
		}

		public static string GetMACAddress()
		{
			string ThisMacAddress = "";
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			int num = 0;
			while (num < (int)allNetworkInterfaces.Length)
			{
				PhysicalAddress pa = allNetworkInterfaces[num].GetPhysicalAddress();
				if (pa == null || !(pa.ToString() != ""))
				{
					num++;
				}
				else
				{
					ThisMacAddress = pa.ToString();
					break;
				}
			}
			return ThisMacAddress;
		}
	}
}