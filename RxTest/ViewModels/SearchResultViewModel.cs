using ReactiveUI;
using Splat;

namespace RxTest
{
    public class SearchResultViewModel : ReactiveObject
    {
        private readonly IBitmap _image;
        private readonly string _title;

        public SearchResultViewModel(IBitmap image, string title)
        {
            _image = image;
            _title = title;
        }

        public IBitmap Image
        {
            get { return _image; }
        }

        public string Title
        {
            get { return _title; }
        }
    }
}