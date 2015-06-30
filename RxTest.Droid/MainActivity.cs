using System;
using System.Reactive.Linq;
using Android.App;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Android.OS;
using ReactiveUI;

namespace RxTest.Droid
{
    [Activity(Label = "RxTest.Droid", MainLauncher = true, Icon = "@drawable/icon", WindowSoftInputMode = SoftInput.StateAlwaysHidden)]
    public class MainActivity : ReactiveActivity<FlickrSearchViewModel>
    {
        public EditText SearchText { get; private set; }
        public Button SearchButton { get; private set; }
        public ListView ImagesList { get; private set; }

        private IMenuItem _loadingItem;
        private ImageView _loadingView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            LoadViewModel();

            LoadViews();

            LoadBindings();

            LoadPrimaryAdapter();

            LoadAnimation();

            LoadErrorDisplay();
        }

        private void LoadViewModel()
        {
            var service = new FlickrImageService();
            ViewModel = new FlickrSearchViewModel(service);
        }

        private void LoadBindings()
        {
            // Set up bindings
            this.Bind(ViewModel, vm => vm.SearchText, v => v.SearchText.Text);
            this.OneWayBind(ViewModel, vm => vm.CanEnterSearchText, v => v.SearchText.Enabled);
            this.BindCommand(ViewModel, vm => vm.Search, v => v.SearchButton);
            this.BindCommand(ViewModel, vm => vm.Cancel, v => v.CancelButton);
        }

        private void LoadViews()
        {
            //load views
            SearchText = FindViewById<EditText>(Resource.Id.searchText);
            SearchButton = FindViewById<Button>(Resource.Id.searchButton);
            ImagesList = FindViewById<ListView>(Resource.Id.imagesList);
            CancelButton = FindViewById<Button>(Resource.Id.cancelButton);
        }

        public Button CancelButton { get; set; }

        private void LoadPrimaryAdapter()
        {
            var adapter = new ReactiveListAdapter<SearchResultViewModel>(
                ViewModel.Images,
                (viewModel, parent) => new ImageItemView(viewModel, this, parent));
            ImagesList.Adapter = adapter;
        }

        private void LoadErrorDisplay()
        {
            this.WhenAnyValue(v => v.ViewModel.ShowError)
                //.Where(x => x)
                .Subscribe(showError =>
                {
                    Toast.MakeText(this, "Could not load image data", ToastLength.Long)
                        .Show();
                });
        }

        private void LoadAnimation()
        {
            var loadingAnimation = AnimationUtils.LoadAnimation(this, Resource.Animation.loading_rotate);
            loadingAnimation.RepeatCount = Animation.Infinite;
            loadingAnimation.RepeatMode = RepeatMode.Restart;

            this.WhenAnyValue(v => v.ViewModel.IsLoading)
                .Subscribe(isLoading =>
                {
                    if (_loadingItem != null)
                    {
                        if (isLoading)
                        {
                            _loadingView.StartAnimation(loadingAnimation);
                        }
                        else
                        {
                            _loadingView.ClearAnimation();
                        }

                        _loadingItem.SetVisible(isLoading);
                    }
                });
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.menu_reactiveflickr, menu);
            return true;
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            _loadingItem = menu.FindItem(Resource.Id.loading);
            _loadingView = (ImageView)_loadingItem.ActionView;
            return true;
        }
    }
}

