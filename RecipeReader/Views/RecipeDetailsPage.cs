using System;
using Xamarin.Forms;

namespace RecipeReader
{
	public class RecipeDetailsPage : ContentPage
	{
		public RecipeDetailsPage (RecipeViewModel recipe)
		{
			this.BindingContext = recipe;

			Label header = new Label
			{
				Text = recipe.Title,
				Font = Font.BoldSystemFontOfSize(20),
				HorizontalOptions = LayoutOptions.Center
			};

			var webImage = new Image
			{
				Aspect = Aspect.AspectFill,
				Source = ImageSource.FromUri(new Uri(recipe.ImageUrl)),
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			var ingredientList = new Label () {
				Font = Font.SystemFontOfSize(12)
			};

			ingredientList.Text = recipe.Ingredients;
			ingredientList.SetBinding (Label.TextProperty, "Ingredients");

			var publisher = new Label {
				Text = "Published by" + recipe.Publisher,
				Font = Font.SystemFontOfSize(10)
			};

			var publisherUrl = new Label {
				Text = "Publisher Url: "+ recipe.PublisherUrl,
				Font = Font.SystemFontOfSize(10)
			};

			var socialRank = new Label {
				Text = "Social Rank: " + recipe.SocialRank.ToString(),
				Font = Font.SystemFontOfSize(10)
			};

			// Accomodate iPhone status bar.
			this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

			// Build the page.
			this.Content = new ScrollView {
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = new StackLayout {
					Children = {
						header,
						webImage,
						ingredientList,
						publisher,
						publisherUrl,
						socialRank
					}
				}
			};
		}
	}
}

