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
    [Activity(Label = "RxTest.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : ReactiveActivity<FlickrSearchViewModel>
    {
        public EditText SearchText { get; set; }
        public Button SearchButton { get; set; }
        public ListView ImagesList { get; set; }

        private IMenuItem _loadingItem { get; set; }
        private ImageView _loadingView { get; set; }
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //Setup ViewModel
            var imageService = new FlickrImageService();
            ViewModel = new FlickrSearchViewModel(imageService);

            // Load Views
            SearchText = FindViewById<EditText>(Resource.Id.editTextSearch);
            SearchButton = FindViewById<Button>(Resource.Id.buttonSearch);
            ImagesList = FindViewById<ListView>(Resource.Id.listViewImages);

            // Set up bindings
            this.Bind(ViewModel, vm => vm.SearchText, v => v.SearchText.Text);
            this.OneWayBind(ViewModel, vm => vm.CanEnterSearchText, v => v.SearchText.Enabled);
            this.BindCommand(ViewModel, vm => vm.Search, v => v.SearchButton);
            

            //Configure list adapter
            var adapter = new ReactiveListAdapter<SearchResultViewModel>(
                ViewModel.Images,
                (vm, parent) => new ImageItemView(vm, this, parent));
            ImagesList.Adapter = adapter;

            // Set up animations
            //var loadingAnimation = AnimationUtils.LoadAnimation(this, Resource.Animation.loading_rotate);
            //loadingAnimation.Duration = Animation.Infinite;
            //loadingAnimation.RepeatMode = RepeatMode.Restart;

            this.WhenAnyValue(v => v.ViewModel.IsLoading)
                .Subscribe(isLoading =>
                {
                    if (_loadingItem != null)
                    {
                        if (isLoading)
                        {
                            //_loadingView.StartAnimation(loadingAnimation);
                            Console.WriteLine("Starting animation load...");
                        }
                        else
                        {
                            //_loadingView.ClearAnimation();
                            Console.WriteLine("Ending load animation...");
                        }

                        _loadingItem.SetVisible(isLoading);
                    }
                });

            this.WhenAnyValue(v => v.ViewModel.ShowError)
                .Where(x => x)
                .Subscribe(showError =>
                {
                    Toast.MakeText(this, "Could not load image data", ToastLength.Long)
                        .Show();
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

