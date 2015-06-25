//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reactive.Linq;
//using System.Reactive.Subjects;
//using System.Text;
//using System.Threading.Tasks;
//using Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters;
//using Livet;

//namespace Grabacr07.Mukyutter.Models.Twitter.Accounts
//{
//	public class FilterWizard : NotificationObject
//	{
//		private Subject<string> queryReader = new Subject<string>();

//		#region Query 変更通知 (するかもしれない) プロパティ

//		private string currentQuery;

//		public string Query
//		{
//			get { return this.currentQuery; }
//			set
//			{
//				if (this.currentQuery != value)
//				{
//					this.currentQuery = value;
//					this.queryReader.OnNext(value);
//				}
//			}
//		}

//		#endregion

//		#region Message 変更通知プロパティ

//		private string _Message;

//		public string Message
//		{
//			get { return this._Message; }
//			private set
//			{
//				if (this._Message != value)
//				{
//					this._Message = value;
//					this.RaisePropertyChanged();
//				}
//			}
//		}

//		#endregion

//		#region CanCreateFilter 変更通知プロパティ

//		private bool _CanCreate;

//		public bool CanCreate
//		{
//			get { return this._CanCreate; }
//			private set
//			{
//				if (this._CanCreate != value)
//				{
//					this._CanCreate = value;
//					this.RaisePropertyChanged();
//				}
//			}
//		}

//		#endregion


//		public FilterWizard()
//		{
//			this.queryReader
//				.Do(_ => this.Message = "")
//				.Do(q => this.CanCreate = string.IsNullOrWhiteSpace(q))
//				.Throttle(TimeSpan.FromMilliseconds(1000))
//				.Where(q => !string.IsNullOrWhiteSpace(q))
//				.Select(q => this.CreateCore(q, ex => this.Message = ex.Message))
//				.Where(f => f != null)
//				.Do(_ => this.Message = "フィルターが作成されました。")
//				.Do(_ => this.CanCreate = true)
//				.Subscribe(f => { });
//		}


//		public void Initialize(string query)
//		{
//			this.currentQuery = query;
//			this.RaisePropertyChanged("Query");
//			this.CanCreate = false;
//			this.Message = "";
//		}

//		public QueryFilter Create()
//		{
//			return this.Create(this.Query);
//		}
//		public QueryFilter Create(string query)
//		{
//			var result = this.CreateCore(query, ex => this.Message = ex.Message);
//			if (result != null)
//			{
//				this.Initialize(result.Query);
//				return result;
//			}
//			return null;
//		}

//		private QueryFilter CreateCore(string query, Action<Exception> failureAction)
//		{
//			try
//			{
//				return QueryFilterCreator.Create(query);
//			}
//			catch (Exception ex)
//			{
//				failureAction(ex);
//				return null;
//			}
//		}
//	}
//}
