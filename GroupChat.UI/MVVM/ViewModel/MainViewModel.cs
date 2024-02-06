using GroupChat.UI.MVVM.Core;
using GroupChat.UI.MVVM.Store;

namespace GroupChat.UI.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private NavigationStore? _navigationStore;

        public NavigationStore? NavigationStore
        {
            get { return _navigationStore; }
            set { _navigationStore = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            _navigationStore = new NavigationStore();
            _navigationStore.CurrentViewModel = new ConnectionViewModel(_navigationStore);
        }
    }
}