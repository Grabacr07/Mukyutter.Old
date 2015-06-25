using System.Diagnostics;

namespace Codeplex.OAuth
{
    /// <summary>represents OAuth Token</summary>
    [DebuggerDisplay("Key = {Key}, Secret = {Secret}")]
    public abstract class Token
    {
        public string Key { get; private set; }
        public string Secret { get; private set; }

        public Token(string key, string secret)
        {
            Guard.ArgumentNull(key, "key");
            Guard.ArgumentNull(secret, "secret");

            this.Key = key;
            this.Secret = secret;
        }
    }

    /// <summary>represents OAuth AccessToken</summary>
    public class AccessToken : Token
    {
        public AccessToken(string key, string secret)
            : base(key, secret)
		{ }

		public static implicit operator AccessToken(AsyncOAuth.AccessToken token)
		{
			return new AccessToken(token.Key, token.Secret);
		}
		public static implicit operator AsyncOAuth.AccessToken(AccessToken token)
		{
			return new AsyncOAuth.AccessToken(token.Key, token.Secret);
		}
    }

    /// <summary>represents OAuth RequestToken</summary>
    public class RequestToken : Token
    {
        public RequestToken(string key, string secret)
            : base(key, secret)
        { }

		public static implicit operator RequestToken(AsyncOAuth.RequestToken token)
		{
			return new RequestToken(token.Key, token.Secret);
		}
		public static implicit operator AsyncOAuth.RequestToken(RequestToken token)
		{
			return new AsyncOAuth.RequestToken(token.Key, token.Secret);
		}
    }
}