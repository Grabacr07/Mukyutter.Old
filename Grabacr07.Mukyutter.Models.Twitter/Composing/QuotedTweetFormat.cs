using System;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Internal;

namespace Grabacr07.Mukyutter.Models.Twitter.Composing
{
	/// <summary>
	/// 引用ツイートの書式を表します。
	/// </summary>
	[Serializable]
	public class QuotedTweetFormat
	{
		/// <summary>
		///     <see cref="QuotedTweetFormat" />
		/// クラスの新しいインスタンスを初期化します。
		/// </summary>
		public QuotedTweetFormat() : this(DefaultFormat) { }

		/// <summary>
		/// 引用ツイートの書式を指定して、<see cref="QuotedTweetFormat" />
		/// クラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="format">書式。</param>
		public QuotedTweetFormat(string format)
		{
			this.Format = format;
		}


		/// <summary>
		/// 引用ツイートの書式を取得します。
		/// </summary>
		public string Format { get; set; }


		/// <summary>
		///     <see cref="QuotedTweetFormat.Format" />
		/// で定義された書式を使用し、指定した Twitter ステータスの引用ツイートの文字列形式を返します。
		/// </summary>
		/// <param name="status">引用元ツイート。</param>
		/// <returns>引用ツイートの文字列形式。</returns>
		public string ToString(Status status)
		{
			return this.CreateQuoteString(status).Replace(InputTextLiteral, "");
		}

		/// <summary>
		/// 引用ツイートの入力部分のインデックス番号を取得します。
		/// </summary>
		/// <param name="status">引用元ツイート。</param>
		/// <returns>入力部分のインデックス番号。通常、このインデックス位置にカーソルを合わせます。</returns>
		public int GetInputIndex(Status status)
		{
			return this.CreateQuoteString(status).IndexOf(InputTextLiteral, StringComparison.Ordinal);
		}

		private string CreateQuoteString(Status status)
		{
			return this.Format
				.Replace(TargetUserLiteral, status.User.ScreenName.ValueWithAtmark)
				.Replace(QuoteTextLiteral, status.Text);
		}


		/// <summary>
		/// 引用元ツイートのユーザー名に置き換えられるリテラル。このフィールドは読み取り専用です。
		/// </summary>
		public static readonly string TargetUserLiteral = "$user$";

		/// <summary>
		/// 引用元ツイートの本文に置き換えられるリテラル。このフィールドは読み取り専用です。
		/// </summary>
		public static readonly string QuoteTextLiteral = "$target$";

		/// <summary>
		/// 引用ツイートの入力部分に置き換えられるリテラル。このフィールドは読み取り専用です。
		/// </summary>
		public static readonly string InputTextLiteral = "$input$";

		/// <summary>
		/// 引用ツイートの書式の規定値。このフィールドは読み取り専用です。
		/// </summary>
		public static readonly string DefaultFormat = string.Format(
			"{0} RT {1}: {2}",
			InputTextLiteral,
			TargetUserLiteral,
			QuoteTextLiteral);
	}
}
