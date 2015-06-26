using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading;
using NUnit.Framework;
using ReactiveUI;

namespace RxTest.Test
{
    [TestFixture]
    public class FlikrImageServiceTest
    {
        [Test]
        public void ConstructFlikrImageService_GivenEmptyString_ShouldBuildFlikrImageService()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var flikrImageService = new FlickrImageService();
            //---------------Test Result -----------------------
            Assert.IsNotNull(flikrImageService);
        }

        [Test]
        public void GetImages_GivenSearchTerm_ShouldReturnGreaterThan0()
        {
            //---------------Set up test pack-------------------
            var flikrImageService = new FlickrImageService();
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            //var searchResultViewModel = await flikrImageService.GetImages("cats");
            var flikrSearchViewModel = new FlickrSearchViewModel(flikrImageService);
            flikrSearchViewModel.Search.Subscribe(_ =>
            {
                Debug.WriteLine(flikrSearchViewModel.Images.Count);
            });
            //---------------Test Result -----------------------
            //Thread.Sleep(5000);
        }
    }
}
