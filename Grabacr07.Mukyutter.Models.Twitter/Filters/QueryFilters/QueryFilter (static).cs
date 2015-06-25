using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters.Rpn;
using Grabacr07.Mukyutter.Models.Twitter.Internal;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Grabacr07.Utilities;

namespace Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters
{
	public partial class QueryFilter
	{
		private static readonly Regex keyValueRegex = new Regex(
			@"(?<key>.*?)(?:\s*?)(?<operator>(=|!=|\?=){1}?)(?:\s*?)""(?<value>.*?)""",
			RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);


		public static QueryFilter Create(string query, TwitterAccount account)
		{
			ReversePolishNotation rpn;
			try
			{
				rpn = ReversePolishNotation.Convert(query);
			}
			catch (InvalidExpressionException ex)
			{
				throw new FilterException(ex.Message, ex);
			}
			catch (Exception ex)
			{
				throw new FilterException("クエリの解析中に不明なエラーが発生しました: " + ex.Message, ex);
			}

			var stack = new Stack<Expression<Func<Status, bool>>>();
			try
			{
				foreach (var token in rpn.Tokens)
				{
					if (token.IsSymbol())
					{
						if (token.IsAnd())
						{
							var left = stack.Pop();
							var right = stack.Pop();
							var visitor = new ParameterVisitor(left.Parameters);
							var exp = Expression.Lambda<Func<Status, bool>>(
								Expression.AndAlso(left.Body, visitor.Visit(right.Body)),
								visitor.Parameters);
							stack.Push(exp);
						}
						else if (token.IsOr())
						{
							var left = stack.Pop();
							var right = stack.Pop();
							var visitor = new ParameterVisitor(left.Parameters);
							var exp = Expression.Lambda<Func<Status, bool>>(
								Expression.OrElse(left.Body, visitor.Visit(right.Body)),
								visitor.Parameters);
							stack.Push(exp);
						}
						else if (token.IsNot())
						{
							var target = stack.Pop();
							var exp = Expression.Lambda<Func<Status, bool>>(
								Expression.Not(target.Body),
								target.Parameters);
							stack.Push(exp);
						}
					}
					else
					{
						stack.Push(GetFilterExpression(token, account));
					}
				}

				var result = stack.Pop();

				return new QueryFilter(query, result.Compile());
			}
			catch (FilterException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new FilterException("フィルターの作成中に不明なエラーが発生しました: " + ex.Message, ex);
			}
		}


		private static Expression<Func<Status, bool>> GetFilterExpression(string query, TwitterAccount account)
		{
			query = query.Trim();

			var matches = keyValueRegex.Matches(query);
			if (matches.Count == 1)
			{
				var key = matches[0].Groups["key"].ToString();
				var value = matches[0].Groups["value"].ToString();
				var op = matches[0].Groups["operator"].ToString();

				var dust = query.Replace(matches[0].Groups[0].ToString(), "");
				if (!string.IsNullOrWhiteSpace(dust))
				{
					throw new FilterException("クエリ '" + key + "' に、認識できない文字 '" + dust + "' が含まれています。");
				}

				if (key == "text")
				{
					var q = new PartialMatch(key, value, op.ToFilterMode());
					return status => q.Match(status.Text, true);
				}
				if (key == "user")
				{
					var q = new ScreenNameQuery(key, new ScreenName(value).Value, op.ToFilterMode());
					return status => q.Match(status.User.ScreenName);
				}
				if (key == "via")
				{
					var q = new FullMatch(key, value, op.ToFilterMode());
					return status => q.Match(status.DisplayStatus.Source.Client, true);
				}
				if (key == "retweeted")
				{
					var q = new BooleanQuery(key, value, op.ToFilterMode());
					return status => q.Match(status.IsRetweetStatus);
				}
				if (key == "mention")
				{
					var q = new MentionQuery(key, value, op.ToFilterMode());
					return status => q.Match(status);
				}

				throw new FilterException("クエリ '" + query + "' は認識できません。");
			}

			if (matches.Count > 1)
			{
				var keys = matches.OfType<Match>()
					.Select(m => m.Groups["key"].ToString())
					.Select(m => "'" + m + "'")
					.ToString(", ");
				throw new FilterException("複数のキー (" + keys + ") を検出しましたが、それらの論理演算子が不明です。");
			}

			throw new FilterException("クエリは key=\"value\" の形式で指定してください。");
		}
	}
}
