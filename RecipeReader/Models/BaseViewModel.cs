using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace RecipeReader
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public BaseViewModel ()
		{
		}

		protected void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) {
				handler (this, new PropertyChangedEventArgs (name));
			}
		}
	}
}

