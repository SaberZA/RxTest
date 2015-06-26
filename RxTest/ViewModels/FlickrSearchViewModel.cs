using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using ReactiveUI;

namespace RxTest
{
    public class FlickrSearchViewModel : ReactiveObject
    {
        public FlickrSearchViewModel(IImageService imageService)
        {
            Images = new ReactiveList<SearchResultViewModel>();

            var canExecute = this.WhenAnyValue(x => x.SearchText)
                .Select(x => !String.IsNullOrEmpty(x));

            Search = ReactiveCommand.CreateAsyncObservable(canExecute,
                _ =>
                {
                    Images.Clear();
                    ShowError = false;
                    return imageService.GetImages(SearchText);
                });

            Search.Subscribe(images => Images.Add(images));
            Search.ThrownExceptions.Subscribe(_ => ShowError = true);

            _isLoading = Search.IsExecuting.ToProperty(this, vm => vm.IsLoading);

            _canEnterSearchText = this.WhenAnyValue(x => x.IsLoading)
                .Select(x => !x)
                .ToProperty(this, vm => vm.CanEnterSearchText);
        }

        public ReactiveCommand<SearchResultViewModel> Search { get; set; }

        private string _searchText;       
        public string SearchText
        {
            get { return _searchText; }
            set { this.RaiseAndSetIfChanged(ref _searchText, value); }
        }

        private bool _showError;
        public bool ShowError
        {
            get { return _showError; }
            set { this.RaiseAndSetIfChanged(ref _showError, value); }
        }

        private readonly ObservableAsPropertyHelper<bool> _isLoading;
        public bool IsLoading
        {
            get { return _isLoading.Value; }
        }

        private readonly ObservableAsPropertyHelper<bool> _canEnterSearchText;
        public bool CanEnterSearchText
        {
            get { return _canEnterSearchText.Value; }
        }

        private ReactiveList<SearchResultViewModel> _images;
        public ReactiveList<SearchResultViewModel> Images
        {
            get { return _images; }
            set { this.RaiseAndSetIfChanged(ref _images, value); }
        }


    }
}