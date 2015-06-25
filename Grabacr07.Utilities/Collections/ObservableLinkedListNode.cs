using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Utilities.Collections
{
	public class ObservableLinkedListNode<T>
	{
		internal ObservableLinkedList<T> list;
		internal ObservableLinkedListNode<T> next;
		internal ObservableLinkedListNode<T> prev;
		internal T item;

		public ObservableLinkedListNode(T value)
		{
			this.item = value;
		}

		internal ObservableLinkedListNode(ObservableLinkedList<T> list, T value)
		{
			this.item = value;
			this.list = list;
		}

		public ObservableLinkedList<T> List
		{
			get { return list; }
		}

		public ObservableLinkedListNode<T> Next
		{
			get { return next == null || next == list.head ? null : next; }
		}

		public ObservableLinkedListNode<T> Previous
		{
			get { return prev == null || this == list.head ? null : prev; }
		}

		internal void Invalidate()
		{
			list = null;
			next = null;
			prev = null;
		}
	}
}
