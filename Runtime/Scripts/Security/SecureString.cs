namespace BloodseekerSDK
{
    public struct SecureString
    {
        private string _str;

        public SecureString(string str)
        {
            this._str = str?.Trim('^') ?? string.Empty;
        }

        public static implicit operator string (SecureString secureString)
        {
            return secureString._str;
        }
    }
}
