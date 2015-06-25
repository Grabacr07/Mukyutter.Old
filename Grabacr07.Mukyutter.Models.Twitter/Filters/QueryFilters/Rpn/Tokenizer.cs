using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters.Rpn;

namespace Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters.Rpn
{
	internal class Tokenizer
	{
		private readonly IEnumerator<string> iteration;
		private readonly string expression;
		private int index;

		public string Current
		{
			get { return this.iteration.Current; }
		}


		public Tokenizer(string expression)
		{
			this.expression = expression;
			this.index = 0;

			this.iteration = this.GetTokens().GetEnumerator();
		}


		public string Next()
		{
			if (this.iteration.MoveNext())
			{
				return this.iteration.Current;
			}
			return null;
		}

		public void Skip()
		{
			this.Next();
		}

		private IEnumerable<string> GetTokens()
		{
			var token = "";

			for (var c = this.NextChar(); c != (char)0; c = this.NextChar())
			{
				if (c.IsSymbol())
				{
					if (c.IsNot())
					{
						// ! 記号だった場合、次の文字を取ってみて、 != 演算子かどうかをチェックしてみる
						var c2 = this.ViewNextChar();
						if (c2 == '=')
						{
							token += c;
							token += this.NextChar();
							continue;
						}
					}

					if (!string.IsNullOrWhiteSpace(token))
					{
						yield return token.Trim();
					}

					token = "";
					yield return c.ToString();
				}
				else
				{
					if (c == '"')
					{
						do
						{
							if (c == (char)0) throw new InvalidExpressionException("閉じられていないダブルクォーテーションを検出しました。");
							token += c;
							c = this.NextChar();
						} while (c != '"');
					}
					token += c;
				}
			}

			if (!string.IsNullOrWhiteSpace(token))
			{
				yield return token.Trim();
			}
		}

		private char NextChar()
		{
			return (this.index < this.expression.Length)
				? this.expression[this.index++]
				: (char)0;
		}
		private char ViewNextChar()
		{
			return (this.index < this.expression.Length)
				? this.expression[this.index]
				: (char)0;
		}


		public override string ToString()
		{
			return this.Current ?? "";
		}
	}
}
