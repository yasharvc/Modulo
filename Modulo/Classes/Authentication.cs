namespace Modulo.Classes
{
	internal class Authentication
	{
		public UserType Type { get; private set; }

		public Authentication(UserType userType = UserType.SiteUser)
		{
			Type = userType;
		}

		public string Authenticate(string userName, string password, UserType userType)
		{
			var user = new User { UserName = userName, Password = password };
			return user.Authenticate(userType);
		}
		public bool IsTokenValid(string userToken)
		{
			return new User().IsTokenValid(userToken);
		}
		public bool ExtendTokenTime(string userToken)
		{
			return new User().ExtendTokenTime(userToken, 10);
		}
	}
}