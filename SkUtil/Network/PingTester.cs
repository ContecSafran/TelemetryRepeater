using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace Contec_MCS.CT_Network
{
	public class PingTester
	{
		public PingTester()
		{
		}

		public void PingTest(string IP, int TimeOut, string TestMessage)
		{
			Ping pingSender = new Ping();
			PingOptions options = new PingOptions()
			{
				DontFragment = true
			};
			string data = string.Concat("Test Message :", TestMessage);
			byte[] buffer = Encoding.ASCII.GetBytes(data);
			PingReply reply = pingSender.Send(IP, TimeOut, buffer, options);
			if (reply.Status != IPStatus.Success)
			{
				this.PingTestResult(false, IP);
				return;
			}
			Console.WriteLine("Address: {0}", reply.Address.ToString());
			Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
			Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
			Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
			Console.WriteLine("Buffer size: {0}", (int)reply.Buffer.Length);
			this.PingTestResult(true, IP);
		}

		public bool PingTestBool(string IP, int TimeOut, string TestMessage)
		{
			Ping pingSender = new Ping();
			PingOptions options = new PingOptions()
			{
				DontFragment = true
			};
			string data = string.Concat("Test Message :", TestMessage);
			byte[] buffer = Encoding.ASCII.GetBytes(data);
			PingReply reply = pingSender.Send(IP, TimeOut, buffer, options);
			if (reply.Status != IPStatus.Success)
			{
				return false;
			}
			Console.WriteLine("Address: {0}", reply.Address.ToString());
			Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
			Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
			Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
			Console.WriteLine("Buffer size: {0}", (int)reply.Buffer.Length);
			return true;
		}

		public event PingTester.PingTestEventHandler PingTestResult;

		public delegate void PingTestEventHandler(bool TestResult, string TestIP);
	}
}