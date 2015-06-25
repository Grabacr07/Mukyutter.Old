using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Utilities;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	public class ApiException : TwitterException
	{
		public Error[] Errors { get; private set; }

		internal ApiException(
			Error[] errors,
			[CallerFilePath]string path = "",
			[CallerMemberName]string member = "",
			[CallerLineNumber]int line = 0)
			: this(errors, null, path, member, line) { }

		internal ApiException(
			Error[] errors,
			Exception innerException,
			[CallerFilePath]string path = "",
			[CallerMemberName]string member = "",
			[CallerLineNumber]int line = 0)
			: base(errors.ToMessage(), innerException, path, member, line)
		{
			this.Errors = errors;
		}
	}

	internal static class ApiExceptionHelper
	{
		public static string ToMessage(this Error[] errors)
		{
			if (errors.Length == 0) return "";
			if (errors.Length == 1) return errors[0].ToMessage() ?? "Twitter API がエラーを返しました: " + errors[0];

			return EnumerableEx.Return("Twitter API が複数のエラーを返しました: ")
				.Concat(errors.Select((e, i) => i + ". " + (e.ToMessage() ?? e.ToString())))
				.ToString(Environment.NewLine);
		}

		public static string ToMessage(this Error error)
		{
			switch (error.Code)
			{
				case 187:
					return "直近の 10 ツイートと同じツイートは投稿できません。";
				default:
					return null;		
			}
		}
	}
}
