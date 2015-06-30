using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using ReactiveUI;
using Splat;

namespace RxTest.Droid
{
    public class ImageItemView : ReactiveViewHost<SearchResultViewModel>
    {
        public ImageItemView(SearchResultViewModel viewModel, Context ctx, ViewGroup parent)
            : base(ctx, Resource.Layout.ImageItem, parent)
        {
            ViewModel = viewModel;

            this.OneWayBind(ViewModel, vm => vm.Title, v => v.Title.Text);
            
            this.WhenAnyValue(vm => vm.ViewModel.Image)
                //.Subscribe(new ReactiveBitmapObserver(Image));
                .Subscribe(x => Image.SetImageDrawable(((IBitmap)x).ToNative()));
        }


        public ImageView Image { get; private set; }
        public TextView Title { get; private set; }
    }

    public class ReactiveBitmapObserver : IObserver<IBitmap>
    {
        private readonly ImageView _imageView;

        public ReactiveBitmapObserver(ImageView imageView)
        {
            _imageView = imageView;
        }

        public void OnCompleted()
        {
            
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(IBitmap value)
        {
            _imageView.SetImageDrawable(value.ToNative());   
        }
    }
}