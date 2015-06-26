using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading;
using NUnit.Framework;
using ReactiveUI;

namespace RxTest.Test
{
    [TestFixture]
    public class FlickrImageServiceTest
    {
        [Test]
        public void ConstructFlikrImageService_GivenEmptyString_ShouldBuildFlikrImageService()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var flickrImageService = new FlickrImageService();
            //---------------Test Result -----------------------
            Assert.IsNotNull(flickrImageService);
        }

        [Test]
        public async void GetImages_GivenSearchTerm_ShouldReturnGreaterThan0()
        {
            //---------------Set up test pack-------------------
            var flickrImageService = new FlickrImageService();
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            //var searchResultViewModel = await flickrImageService.GetImages("cats");
            //Assert.IsNotNull(searchResultViewModel);
            var flickrSearchViewModel = new FlickrSearchViewModel(flickrImageService);
            flickrSearchViewModel.Search.Subscribe(x => Debug.WriteLine("*****\n"+x.Title),
                () => Debug.WriteLine("Sequence Completed."));

            await flickrSearchViewModel.Search.ExecuteAsync();
            //---------------Test Result -----------------------
            Assert.IsTrue(flickrSearchViewModel.Images.Count > 0);
        }
    }
}
