using System.Collections.Generic;

namespace Books.Models
{
	public class BasicUser : User
	{
		public override string Role
		{
			get
			{
				return "User";
			}
		}
	}
}
