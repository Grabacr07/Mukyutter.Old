using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters.Rpn;
using Grabacr07.Utilities;

namespace Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters.Rpn
{
	internal class ReversePolishNotation
	{
		private List<string> tokens;

		public IEnumerable<string> Tokens
		{
			get { return this.tokens; }
		}

		private ReversePolishNotation(IEnumerable<string> tokens)
		{
			this.tokens = new List<string>(tokens);
		}

		public override string ToString()
		{
			return this.Tokens.ToString(" ");
		}


		public static ReversePolishNotation Convert(string expression)
		{
			var tokenizer = new Tokenizer(expression);
			var result = new List<string>();

			tokenizer.Next();
			ParseExpression(tokenizer, result);

			return new ReversePolishNotation(result);
		}

		private static void ParseExpression(Tokenizer tokenizer, List<string> result)
		{
			ParseFactor(tokenizer, result);

			for (var token = tokenizer.Current; token.IsOperator(); token = tokenizer.Current)
			{
				var next = tokenizer.Next();
				if (next == null)
				{
					throw new InvalidExpressionException("式が終了していません。");
				}
				if (next.IsOperator())
				{
					throw new InvalidExpressionException("演算子が重複しています。");
				}
				if (next.IsClosingParenthesis())
				{
					throw new InvalidExpressionException("演算子と右かっこが連続しています。");
				}

				ParseFactor(tokenizer, result);
				result.Add(token);
			}
		}

		private static void ParseFactor(Tokenizer tokenizer, List<string> result)
		{
			var not = "";
			var token = tokenizer.Current;
			if (token.IsNot())
			{
				not = token;
				token = tokenizer.Next();
			}

			if (token.IsBegginingParenthesis())
			{
				tokenizer.Skip();	// '(' をスキップ
				var next = tokenizer.Current;

				if (next.IsOperator())
				{
					throw new InvalidExpressionException("左かっこと演算子が連続しています。");
				}

				ParseExpression(tokenizer, result);

				if (!tokenizer.Current.IsClosingParenthesis())
				{
					throw new InvalidExpressionException("かっこが閉じられていません。");
				}
				tokenizer.Skip();	// ')' をスキップ
			}
			else
			{
				ParseElement(tokenizer, result);
			}

			if (!string.IsNullOrEmpty(not))
			{
				result.Add(not);
			}
		}

		private static void ParseElement(Tokenizer tokenizer, List<string> result)
		{
			var token = tokenizer.Current;

			result.Add(token);
			tokenizer.Next();
		}
	}
}
