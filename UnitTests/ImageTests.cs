using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            //arrange - create a product with image data
            Product prod = new Product
            {
                ProductID = 2,
                Name = "Test",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            //arrange - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[]
            {
                new Product
                {
                    ProductID = 1,
                    Name = "P1"
                },
                prod,
                new Product
                {
                    ProductID = 3,
                    Name = "P3"
                }
            }.AsQueryable());

            //arrange - create the controller
            ProductController target = new ProductController(mock.Object);

            //act - call the get image method
            ActionResult result = target.GetImage(2);

            //assert 
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(prod.ImageMimeType, (((FileResult)result).ContentType));
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_Id()
        {
            //arrange - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[]
            {
                new Product
                {
                    ProductID = 1,
                    Name = "P1"
                },
                new Product
                {
                    ProductID = 2,
                    Name = "P2"
                }
            }.AsQueryable());

            //arrange - create the controller
            ProductController target = new ProductController(mock.Object);

            //act - call the get image method
            ActionResult result = target.GetImage(100);

            //assert
            Assert.IsNull(result);
        }
    }
}
