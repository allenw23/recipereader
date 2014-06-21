using System;
using Xamarin.Forms;

namespace RecipeReader
{
	public class App
	{
		public static Page GetMainPage ()
		{	
			return new NavigationPage (new RecipeSearchPage ());
		}

		public static string ApiKey {
			get {
		
				return "5a478bd4291381aab38b71cf8a226b15";
			}
		}
	}
}

