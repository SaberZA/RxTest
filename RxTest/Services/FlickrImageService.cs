using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Xml.Linq;
using Splat;

namespace RxTest
{
    public class FlickrImageService : IImageService
    {
        public FlickrImageService()
        { }

        private readonly string _searchUrlFormat = "https://api.flickr.com/services/rest?method=" + _endpoint + "&api_key=" + _api_key + "&text={0}&safe_search=1&content_type=1&media=photos";
        private readonly static string _endpoint = "flickr.photos.search";
        private readonly static string _api_key = "6df4fb80ed0eed936cebaf0a99067c64";

        private readonly string _photoUrlFormat = "https://farm{0}.staticflickr.com/{1}/{2}_{3}_q.jpg";

        public IObservable<SearchResultViewModel> GetImages(string searchText)
        {
            return Observable.Create<SearchResultViewModel>(async observer =>
            {
                var httpClient = new HttpClient();
                var address = default(Uri);
                var searchResults = default(string);

                try
                {
                    address = new Uri(string.Format(_searchUrlFormat, searchText));
                    searchResults = await httpClient.GetStringAsync(address);
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                    return;
                }

                var pa = XDocument.Parse(searchResults)
                    .Descendants("photos")
                    .Descendants("photo");
                var photos = pa
                    .Select(p => new
                    {
                        Url = string.Format(
                            _photoUrlFormat,
                            p.Attribute("farm").Value,
                            p.Attribute("server").Value,
                            p.Attribute("id").Value,
                            p.Attribute("secret").Value),
                        Title = p.Attribute("title").Value
                    });

                foreach (var photo in photos)
                {
                    try
                    {
                        var imageData = await httpClient.GetByteArrayAsync(photo.Url);
                        var stream = new MemoryStream(imageData);
                        var image = await BitmapLoader.Current.Load(stream, null, null);
                        observer.OnNext(new SearchResultViewModel(image, photo.Title));
                    }
                    catch (HttpRequestException)
                    {
                        // Right now do nothing
                        continue;
                    }
                    catch (Exception ex)
                    {
                        // Any other kind of error, we want to send to subscribers
                        observer.OnError(ex);
                    }
                }

                observer.OnCompleted();
            });
        }
    }
}