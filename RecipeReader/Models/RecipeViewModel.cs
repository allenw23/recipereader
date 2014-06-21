using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace RecipeReader
{
	public class RecipeViewModel : BaseViewModel
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private string ingredients;

		public string Id { get; set;  }
		public string Title { get; set; }
		public float SocialRank { get; set;}
		public string PublisherUrl { get; set; }
		public string Url { get; set; }
		public string Ingredients { 
			get { return ingredients; }  
			set { ingredients = value; OnPropertyChanged ("Ingredients"); } 
		}
		public string ImageUrl { get; set; }
		public string Publisher { get; set; }

		public RecipeViewModel ()
		{

		}
	}
}

