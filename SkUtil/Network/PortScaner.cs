using System;
using System.Net;
using System.Net.NetworkInformation;

namespace SkUtil.Network
{
	public class PortScaner
	{
		public PortScaner()
		{
		}

		public static bool IsPortUse(int PortNumber)
		{
			IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
			ipProperties.GetActiveTcpListeners();
			TcpConnectionInformation[] activeTcpConnections = ipProperties.GetActiveTcpConnections();
			for (int i = 0; i < (int)activeTcpConnections.Length; i++)
			{
				if (activeTcpConnections[i].LocalEndPoint.Port == PortNumber)
				{
					return true;
				}
			}
			return false;
		}
	}
}