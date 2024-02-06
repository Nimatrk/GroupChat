using GroupChat.UI.MVVM.Core;

namespace GroupChat.UI.MVVM.Store
{
    public class NavigationStore : ObservableObject
    {
		private ObservableObject _currentViewModel;

		public ObservableObject CurrentViewModel
		{
			get { return _currentViewModel; }
			set { _currentViewModel = value; OnPropertyChanged(); }
		}
	}
}