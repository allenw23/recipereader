using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace RecipeReader
{
	public class RecipeSearchPage : ContentPage
	{
		public RecipeSearchPage ()
		{
			RecipeListViewModel recipeList = new RecipeListViewModel();

			Title = "Food2Fork Search";

			// Accomodate iPhone status bar.
			this.Padding = new Thickness (10, Device.OnPlatform (20, 0, 0), 10, 5);

			var searchbar = new SearchBar () {
				Placeholder = "Search for recipes"
			};

			searchbar.SearchButtonPressed += (sender, e) => {
				recipeList.SearchFor(searchbar.Text);
			};

			var list = new ListView ();

			list.ItemsSource = recipeList.Recipes;
			list.SetBinding (ListView.ItemsSourceProperty, ".");

			var cell = new DataTemplate (typeof(TextCell));
			cell.SetBinding (TextCell.TextProperty, "Title");
			list.ItemTemplate = cell;
			list.ItemTapped += (sender, e) => {
				var recipe = e.Item as RecipeViewModel;
				if (recipe == null)
					return;

				recipeList.GetRecipeIngredients(recipe.Id);

				Navigation.PushAsync(new RecipeDetailsPage (recipe));
				list.SelectedItem = null;
			};

			this.Content = new StackLayout {
				Children = {
					searchbar,
					list
				}
			};
		}

	}
}

