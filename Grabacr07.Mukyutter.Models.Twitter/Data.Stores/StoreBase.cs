using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Stores
{
	public abstract class StoreBase
	{
		protected ReaderWriterLockSlim lockslim = new ReaderWriterLockSlim();

		/// <summary>
		/// コレクションを読み取り専用ロック状態に設定し、指定したメソッドを実行します。<paramref name="readAction"/>
		/// 内では、アップグレード可能モードまたは書き込みモードでロックを行うような操作 (e.g. ステータスの追加処理) は実行しないでください。
		/// </summary>
		/// <param name="readAction">コレクションを読み取り専用ロックに設定してから実行するメソッド。</param>
		internal void DoReadLockAction(Action readAction)
		{
			if (this.lockslim.IsReadLockHeld)
			{
				readAction();
			}
			else
			{
				try
				{
					this.lockslim.EnterReadLock();
					readAction();
				}
				finally
				{
					if (this.lockslim.IsReadLockHeld) this.lockslim.ExitReadLock();
				}
			}
		}

		protected internal T DoReadLockAction<T>(Func<T> readAction)
		{
			if (this.lockslim.IsReadLockHeld)
			{
				return readAction();
			}
			else
			{
				try
				{
					this.lockslim.EnterReadLock();
					return readAction();
				}
				finally
				{
					if (this.lockslim.IsReadLockHeld) this.lockslim.ExitReadLock();
				}
			}
		}

		protected internal void DoWriteLockAction(Action writeAction)
		{
			if (this.lockslim.IsWriteLockHeld)
			{
				writeAction();
			}
			else
			{
				try
				{
					this.lockslim.EnterWriteLock();
					writeAction();
				}
				finally
				{
					if (this.lockslim.IsWriteLockHeld) this.lockslim.ExitWriteLock();
				}
			}
		}

		protected internal T DoWriteLockAction<T>(Func<T> writeAction)
		{
			if (this.lockslim.IsWriteLockHeld)
			{
				return writeAction();
			}
			else
			{
				try
				{
					this.lockslim.EnterWriteLock();
					return writeAction();
				}
				finally
				{
					if (this.lockslim.IsWriteLockHeld) this.lockslim.ExitWriteLock();
				}
			}
		}
	}
}
