using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RecipeReader
{
	public class RecipeListViewModel
	{
		private ObservableCollection<RecipeViewModel> recipes = new ObservableCollection<RecipeViewModel> ();
		private bool IsBusy;

		public ObservableCollection<RecipeViewModel> Recipes { 
			get { return recipes; } 
			private set { recipes = value; }
		}
			
		public RecipeListViewModel () 
		{
		}

		public void SearchFor(string searchFor, string sort = null, int? page = null)
		{
			if (IsBusy)
				return;
			IsBusy = true;

			try {
				string url = "http://food2fork.com/api/search?key=" + App.ApiKey + "&q=" + searchFor;
				var httpReq = (HttpWebRequest)HttpWebRequest.Create (new Uri(url));
				string content = null;
				httpReq.BeginGetResponse (((ar) => {
					var request = (HttpWebRequest)ar.AsyncState;
					using (var response = (HttpWebResponse)request.EndGetResponse (ar)) {
						using (StreamReader reader = new StreamReader(response.GetResponseStream()))
						{
							content = reader.ReadToEnd();

							JObject r0 = JObject.Parse(content);
							JArray recipeArray = (JArray) r0.SelectToken("recipes");

							var recipes = recipeArray.Select (r => new RecipeViewModel {
								Title = (string) r["title"],
								Url = (string) r["f2f_url"],
								Id = (string) r["recipe_id"],
								SocialRank = float.Parse((string) r["social_rank"]),
								PublisherUrl = (string) r["publisher_url"],
								Publisher = (string) r["publisher"],
								ImageUrl = (string) r["image_url"]
							}).ToList ();

							Xamarin.Forms.Device.BeginInvokeOnMainThread(()=>{
								Recipes.Clear();
								foreach (var r in recipes)
								{
									// remove recipes with bad ingredients
									if (r.Publisher == "Tasty Kitchen")
										continue;

									Recipes.Add(r);
								}
							});

							return ;
						}
					}
				}), httpReq);


			} catch (Exception ex) {
				var newPage = new ContentPage();
				newPage.DisplayAlert("Error", "Unable to load recipes " + ex.Message, "OK", null);
			}


			IsBusy = false;

		}

		/// <summary>
		/// Updates the recipe.
		/// </summary>
		/// <param name="id">Identifier.</param>
		public void GetRecipeIngredients(string id)
		{
			//			string jsonData = @"{""publisher"": ""The Pioneer Woman"", ""f2f_url"": ""http://food2fork.com/view/47319"", ""ingredients"": [""12 whole New Potatoes (or Other Small Round Potatoes)"", ""3 Tablespoons Olive Oil"", ""Kosher Salt To Taste"", ""Black Pepper To Taste"", ""Rosemary (or Other Herbs Of Choice) To Taste""], ""source_url"": ""http://thepioneerwoman.com/cooking/2008/06/crash-hot-potatoes/"", ""recipe_id"": ""47319"", ""image_url"": ""http://static.food2fork.com/CrashHotPotatoes5736.jpg"", ""social_rank"": 100.0, ""publisher_url"": ""http://thepioneerwoman.com"", ""title"": ""Crash Hot Potatoes""}";
			string url = "http://food2fork.com/api/get?key=" + App.ApiKey + "&rId=" + id;
			var httpReq = (HttpWebRequest)HttpWebRequest.Create (new Uri(url));
			string content = null;
			httpReq.BeginGetResponse (((ar) => {
				var request = (HttpWebRequest)ar.AsyncState;
				using (var response = (HttpWebResponse)request.EndGetResponse (ar)) {
					using (StreamReader reader = new StreamReader(response.GetResponseStream()))
					{
						content = reader.ReadToEnd();

						JObject r0 = JObject.Parse(content);
						JArray j0 = (JArray) r0.SelectToken("recipe").SelectToken("ingredients");

						IList<string> ingredients = j0.Select(i => (string) i).ToList();

						RecipeViewModel updateRecipe = Recipes.SingleOrDefault(i=>i.Id == id);

						if (updateRecipe == null)
							return;

						string ingredientStr = null;

						foreach (string s in ingredients)
						{
							ingredientStr += s + Environment.NewLine;
						}

						updateRecipe.Ingredients = ingredientStr.Replace(@"&#174;", "");


						return ;
					}
				}
			}), httpReq);


			return;

		}
	}
}

