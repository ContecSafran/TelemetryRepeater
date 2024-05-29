using System;
using System.Collections.Concurrent;
using System.Threading;

namespace SkUtil.Network
{
	public class PacketQueue<T>
	{
		private ConcurrentQueue<T> _DataQueue = new ConcurrentQueue<T>();

		public PacketQueue()
		{
		}

		public void Add_Packet(T Packet)
		{
			this._DataQueue.Enqueue(Packet);
			if (this.DATA_INSERT != null)
			{
				this.DATA_INSERT();
			}
		}

		public void Clear()
		{
			T item;
			while (this._DataQueue.TryDequeue(out item))
			{
			}
		}

		public T Get_Packet()
		{
			T Result;
			if (this._DataQueue.TryDequeue(out Result))
			{
				return Result;
			}
			return default(T);
		}

		public int Get_PacketCount()
		{
			return this._DataQueue.Count;
		}

		public event PacketQueue<T>.InsertEventHandler DATA_INSERT;

		public delegate void InsertEventHandler();
	}
}