using System;

namespace RxTest
{
    public interface IImageService
    {
        IObservable<SearchResultViewModel> GetImages(string searchText);
    }
}