using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grabacr07.Utilities.Collections
{
	public class ObservableLinkedList<T> : ICollection<T>, ICollection,
										   INotifyCollectionChanged, INotifyPropertyChanged
	{

		#region private fields

		internal ObservableLinkedListNode<T> head;
		internal int count;
		internal int version;
		private object _syncRoot;

		private const string CountString = "Count";

		#endregion

		#region public properties

		public int Count
		{
			get { return count; }
		}

		public ObservableLinkedListNode<T> First
		{
			get { return head; }
		}

		public ObservableLinkedListNode<T> Last
		{
			get { return head == null ? null : head.prev; }
		}

		#endregion

		#region constructors

		public ObservableLinkedList()
		{
		}

		public ObservableLinkedList(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}

			foreach (T item in collection)
			{
				AddLast(item);
			}
		}

		#endregion

		#region public methods

		public ObservableLinkedListNode<T> AddAfter(ObservableLinkedListNode<T> node, T value)
		{
			ValidateNode(node);
			var result = new ObservableLinkedListNode<T>(node.list, value);
			InternalInsertNodeBefore(node.next, result);
			return result;
		}

		public void AddAfter(ObservableLinkedListNode<T> node, ObservableLinkedListNode<T> newNode)
		{
			ValidateNode(node);
			ValidateNewNode(newNode);
			InternalInsertNodeBefore(node.next, newNode);
			newNode.list = this;
		}

		public ObservableLinkedListNode<T> AddBefore(ObservableLinkedListNode<T> node, T value)
		{
			ValidateNode(node);
			var result = new ObservableLinkedListNode<T>(node.list, value);
			InternalInsertNodeBefore(node, result);
			if (node == head)
			{
				head = result;
			}
			return result;
		}

		public void AddBefore(ObservableLinkedListNode<T> node, ObservableLinkedListNode<T> newNode)
		{
			ValidateNode(node);
			ValidateNewNode(newNode);
			InternalInsertNodeBefore(node, newNode);
			newNode.list = this;
			if (node == head)
			{
				head = newNode;
			}
		}

		public ObservableLinkedListNode<T> AddFirst(T value)
		{
			var result = new ObservableLinkedListNode<T>(this, value);
			if (head == null)
			{
				InternalInsertNodeToEmptyList(result);
			}
			else
			{
				InternalInsertNodeBefore(head, result);
				head = result;
			}

			return result;
		}

		public void AddFirst(ObservableLinkedListNode<T> node)
		{
			ValidateNewNode(node);

			if (head == null)
			{
				InternalInsertNodeToEmptyList(node);
			}
			else
			{
				InternalInsertNodeBefore(head, node);
				head = node;
			}
			node.list = this;
		}

		public ObservableLinkedListNode<T> AddLast(T value)
		{
			var result = new ObservableLinkedListNode<T>(this, value);
			if (head == null)
			{
				InternalInsertNodeToEmptyList(result);
			}
			else
			{
				InternalInsertNodeBefore(head, result);
			}
			return result;
		}

		public void AddLast(ObservableLinkedListNode<T> node)
		{
			ValidateNewNode(node);

			if (head == null)
			{
				InternalInsertNodeToEmptyList(node);
			}
			else
			{
				InternalInsertNodeBefore(head, node);
			}
			node.list = this;
		}

		public void Clear()
		{
			ObservableLinkedListNode<T> current = head;
			while (current != null)
			{
				ObservableLinkedListNode<T> temp = current;
				current = current.Next;   // use Next the instead of "next", otherwise it will loop forever 
				temp.Invalidate();
			}

			head = null;
			count = 0;
			version++;
		}

		public bool Remove(T value)
		{
			ObservableLinkedListNode<T> node = Find(value);
			if (node != null)
			{
				InternalRemoveNode(node);
				return true;
			}
			return false;
		}

		public void Remove(ObservableLinkedListNode<T> node)
		{
			ValidateNode(node);
			InternalRemoveNode(node);
		}

		public void RemoveFirst()
		{
			if (head == null) { throw new InvalidOperationException(); }
			InternalRemoveNode(head);
		}

		public void RemoveLast()
		{
			if (head == null) { throw new InvalidOperationException(); }
			InternalRemoveNode(head.prev);
		}


		#region contains, find, copy methods

		public bool Contains(T value)
		{
			return Find(value) != null;
		}

		public void CopyTo(T[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}

			if (index < 0 || index > array.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}

			if (array.Length - index < Count)
			{
				throw new ArgumentException();
			}

			ObservableLinkedListNode<T> node = head;
			if (node != null)
			{
				do
				{
					array[index++] = node.item;
					node = node.next;
				} while (node != head);
			}
		}

		public ObservableLinkedListNode<T> Find(T value)
		{
			ObservableLinkedListNode<T> node = head;
			EqualityComparer<T> c = EqualityComparer<T>.Default;
			if (node != null)
			{
				if (value != null)
				{
					do
					{
						if (c.Equals(node.item, value))
						{
							return node;
						}
						node = node.next;
					} while (node != head);
				}
				else
				{
					do
					{
						if (node.item == null)
						{
							return node;
						}
						node = node.next;
					} while (node != head);
				}
			}
			return null;
		}

		public ObservableLinkedListNode<T> FindLast(T value)
		{
			if (head == null) return null;

			ObservableLinkedListNode<T> last = head.prev;
			ObservableLinkedListNode<T> node = last;
			EqualityComparer<T> c = EqualityComparer<T>.Default;
			if (node != null)
			{
				if (value != null)
				{
					do
					{
						if (c.Equals(node.item, value))
						{
							return node;
						}

						node = node.prev;
					} while (node != last);
				}
				else
				{
					do
					{
						if (node.item == null)
						{
							return node;
						}
						node = node.prev;
					} while (node != last);
				}
			}
			return null;
		}

		#endregion

		#endregion

		#region private/internal members

		private void InternalInsertNodeBefore(ObservableLinkedListNode<T> node, ObservableLinkedListNode<T> newNode)
		{
			newNode.next = node;
			newNode.prev = node.prev;
			node.prev.next = newNode;
			node.prev = newNode;
			version++;
			count++;
		}

		private void InternalInsertNodeToEmptyList(ObservableLinkedListNode<T> newNode)
		{
			Debug.Assert(head == null && count == 0, "StatusList must be empty when this method is called!");
			newNode.next = newNode;
			newNode.prev = newNode;
			head = newNode;
			version++;
			count++;
		}

		internal void InternalRemoveNode(ObservableLinkedListNode<T> node)
		{
			Debug.Assert(node.list == this, "Deleting the node from another list!");
			Debug.Assert(head != null, "This method shouldn't be called on empty list!");
			if (node.next == node)
			{
				Debug.Assert(count == 1 && head == node, "this should only be true for a list with only one node");
				head = null;
			}
			else
			{
				node.next.prev = node.prev;
				node.prev.next = node.next;
				if (head == node)
				{
					head = node.next;
				}
			}
			node.Invalidate();
			count--;
			version++;
		}

		#endregion

		#region validation methods

		internal void ValidateNewNode(ObservableLinkedListNode<T> node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}

			if (node.list != null)
			{
				throw new InvalidOperationException();
			}
		}


		internal void ValidateNode(ObservableLinkedListNode<T> node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}

			if (node.list != this)
			{
				throw new InvalidOperationException();
			}
		}

		#endregion


		#region ICollection<T> members

		bool ICollection<T>.IsReadOnly
		{
			get { return false; }
		}

		void ICollection<T>.Add(T value)
		{
			AddLast(value);
		}

		#endregion

		#region ICollection members

		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		object ICollection.SyncRoot
		{
			get
			{
				if (_syncRoot == null)
				{
					Interlocked.CompareExchange<Object>(ref _syncRoot, new Object(), null);
				}
				return _syncRoot;
			}
		}

		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}

			if (array.Rank != 1)
			{
				throw new ArgumentException();
			}

			if (array.GetLowerBound(0) != 0)
			{
				throw new ArgumentException();
			}

			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}

			if (array.Length - index < Count)
			{
				throw new ArgumentException();
			}

			T[] tArray = array as T[];
			if (tArray != null)
			{
				CopyTo(tArray, index);
			}
			else
			{
				//
				// Catch the obvious case assignment will fail. 
				// We can found all possible problems by doing the check though.
				// For example, if the element type of the Array is derived from T,
				// we can't figure out if we can successfully copy the element beforehand.
				// 
				Type targetType = array.GetType().GetElementType();
				Type sourceType = typeof(T);
				if (!(targetType.IsAssignableFrom(sourceType) || sourceType.IsAssignableFrom(targetType)))
				{
					throw new ArgumentException();
				}

				object[] objects = array as object[];
				if (objects == null)
				{
					throw new ArgumentException();
				}
				ObservableLinkedListNode<T> node = head;
				try
				{
					if (node != null)
					{
						do
						{
							objects[index++] = node.item;
							node = node.next;
						} while (node != head);
					}
				}
				catch (ArrayTypeMismatchException)
				{
					throw new ArgumentException();
				}
			}
		}

		#endregion

		#region IEnumerable<T> members

		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region IEnumerable members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region Enumerator class

		public struct Enumerator : IEnumerator<T>, IEnumerator
		{
			private ObservableLinkedList<T> list;
			private ObservableLinkedListNode<T> node;
			private int version;
			private T current;
			private int index;

			internal Enumerator(ObservableLinkedList<T> list)
			{
				this.list = list;
				version = list.version;
				node = list.head;
				current = default(T);
				index = 0;
			}

			public T Current
			{
				get { return current; }
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					if (index == 0 || (index == list.Count + 1))
					{
						throw new InvalidOperationException("コレクションが変更されました。列挙操作は実行されません？");
					}

					return current;
				}
			}

			public bool MoveNext()
			{
				if (version != list.version)
				{
					throw new InvalidOperationException();
				}

				if (node == null)
				{
					index = list.Count + 1;
					return false;
				}

				++index;
				current = node.item;
				node = node.next;
				if (node == list.head)
				{
					node = null;
				}
				return true;
			}

			void System.Collections.IEnumerator.Reset()
			{
				if (version != list.version)
				{
					throw new InvalidOperationException();
				}

				current = default(T);
				node = list.head;
				index = 0;
			}

			public void Dispose()
			{
			}
		}

		#endregion


		#region INotifyCollectionChanged, INotifyPropertyChanged

		/// <summary>
		/// コレクションが変更された際に発生するイベントです。
		/// </summary>
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		/// <summary>
		/// プロパティが変更された際に発生するイベントです。
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// CollectionChangedイベントを発生させます。
		/// </summary>
		/// <param name="args">NotifyCollectionChangedEventArgs</param>
		protected void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			var threadSafeHandler = Interlocked.CompareExchange(ref CollectionChanged, null, null);

			if (threadSafeHandler != null)
			{
				threadSafeHandler(this, args);
			}
		}

		/// <summary>
		/// PropertyChangedイベントを発生させます。
		/// </summary>
		/// <param name="propertyName">変更されたプロパティの名前</param>
		protected void OnPropertyChanged(string propertyName)
		{
			var threadSafeHandler = Interlocked.CompareExchange(ref PropertyChanged, null, null);

			if (threadSafeHandler != null)
			{
				threadSafeHandler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		/// <summary>
		/// Helper to raise CollectionChanged event to any listeners 
		/// </summary> 
		protected void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
		{
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
		}

		/// <summary> 
		/// Helper to raise CollectionChanged event to any listeners
		/// </summary> 
		protected void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
		{
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
		}

		/// <summary>
		/// Helper to raise CollectionChanged event to any listeners 
		/// </summary>
		protected void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
		{
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
		}

		/// <summary>
		/// Helper to raise CollectionChanged event with action == Reset to any listeners
		/// </summary> 
		protected void OnCollectionReset()
		{
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		#endregion

	}
}
