using Web = AL.Eatables.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using System;
using AL.Eatables.Web.Core.Validators.EatableValidator;
using AL.Eatables.Web.Core.Services.InventoryService;
using System.Web.Http;
using System.Net;
using System.Threading.Tasks;

namespace AL.Eatables.IntegrationTests.Controllers
{
    [TestClass]
    public class InventoryController
    {
        protected readonly Web.Controllers.InventoryController _controller;

        public InventoryController()
        {
            Web.App_Start.MapperConfig.Configure();
            _controller = new Web.Controllers.InventoryController(
                new InventoryService(),
                new EatableValidator());
        }

        [TestMethod]
        public void Integration_ShouldGetListOfEatables()
        {
            var response = _controller.Get();
            var inventoryList = ((OkNegotiatedContentResult<IList<Web.Models.Eatable>>)response).Content.ToList();
 
            Assert.IsNotNull(inventoryList);
            Assert.IsTrue(inventoryList.Count > 0);
        }

        [TestMethod]
        public void Integration_ShouldGetEatableItem()
        {
            var response = _controller.Get(5);
            var inventoryItem = ((OkNegotiatedContentResult<Web.Models.Eatable>)response).Content;

            Assert.IsNotNull(inventoryItem);
        }

        [TestMethod]
        public void Integration_ShouldReturnNullWhenGetEatableItemThatDoesNotExcists()
        {
            var response = _controller.Get(-1);
            var inventoryItem = ((OkNegotiatedContentResult<Web.Models.Eatable>)response).Content;

            Assert.IsNotNull(response);
            Assert.IsNull(inventoryItem);
        }

        [TestMethod]
        public void Integration_ShouldDeleteEatableItem()
        {
            var postResponse = _controller.Post(GenerateUniqEatable());
            var newEatable = ((OkNegotiatedContentResult<Web.Models.Eatable>)postResponse).Content;
            var response = _controller.Get();
            var beforeDeleteInventory = ((OkNegotiatedContentResult<IList<Web.Models.Eatable>>)response).Content.ToList();

            response = _controller.Delete(newEatable.Id);
            var afterDeleteInventory = ((OkNegotiatedContentResult<IList<Web.Models.Eatable>>)response).Content.ToList();

            Assert.IsNotNull(beforeDeleteInventory);
            Assert.IsNotNull(afterDeleteInventory);
            Assert.IsTrue(beforeDeleteInventory.Count == (afterDeleteInventory.Count + 1));
        }

        [TestMethod]
        public void Integration_ShouldInsertNewEatableItemWhenPost()
        {
            var getResponse = _controller.Get();
            var beforePostInventory = ((OkNegotiatedContentResult<IList<Web.Models.Eatable>>)getResponse).Content.ToList();
            var newEatable = new Web.Models.EatableInput { Name = "new-cockie", Price=1.00M };
            var response = _controller.Post(newEatable);
            var result = ((OkNegotiatedContentResult<Web.Models.Eatable>)response).Content;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id != 0);
            Assert.IsTrue(result.Name == newEatable.Name);
            Assert.IsTrue(result.Price == newEatable.Price);
            Assert.IsNull(beforePostInventory.FirstOrDefault(i => i.Id == result.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void Integration_ShouldThrowBadRequestExceptionWhenPostWithInvalidEatableItem()
        {
            try
            {
                _controller.Post(null);
            }
            catch (HttpResponseException ex)
            {
                Assert.AreEqual(ex.Response.StatusCode, HttpStatusCode.BadRequest, "Wrong response type");
                throw;
            }
        }

        [TestMethod]
        public void Integration_ShouldUpdateEatableItem()
        {
            var newEatable = new Web.Models.EatableInput { Name = "new-cockie", Price = 1.00M };
            var updatedEatable = new Web.Models.EatableInput { Name = "new-cockie with seasalt", Price = 1.50M };
            var newResponse = _controller.Post(newEatable);
            var newResult = ((OkNegotiatedContentResult<Web.Models.Eatable>)newResponse).Content;
            var response = _controller.Put(newResult.Id, updatedEatable);
            var inventoryList = ((OkNegotiatedContentResult<IList<Web.Models.Eatable>>)response).Content.ToList();

            Assert.IsNotNull(inventoryList);
            Assert.IsTrue(inventoryList.Count > 0);
            Assert.IsNotNull(inventoryList.Where(i => (i.Name == updatedEatable.Name)).FirstOrDefault());
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void Integration_ShouldThrowBadRequestExceptionWhenPutWithInvalidEatableItem()
        {
            try
            {
                _controller.Put(2, null);
            }
            catch (HttpResponseException ex)
            {
                Assert.AreEqual(ex.Response.StatusCode, HttpStatusCode.BadRequest, "Wrong response type");
                throw;
            }
        }

        [TestClass]
        public class WhenCallingInventoryAsynchronous : InventoryController
        {
            private List<Web.Models.Eatable> _results;
            private List<Web.Models.EatableInput> _inputList;
            private List<IHttpActionResult> _deleteResponseList;

            public WhenCallingInventoryAsynchronous()
            {
                _inputList = new List<Web.Models.EatableInput> { GenerateUniqEatable(), GenerateUniqEatable(), GenerateUniqEatable() };
                _results = new List<Web.Models.Eatable>();
                _deleteResponseList = new List<IHttpActionResult>();
            }

            [TestMethod]
            public void ShouldCallPosthMultipleTimesAsync()
            {
                var postTaskList = GeneratePostTasks();

                Task.WaitAll(postTaskList.ToArray());

                Assert.IsTrue(_results.Count == _inputList.Count);

                var deleteTaskList = GenerateDeleteTasks();

                Assert.IsTrue(deleteTaskList.Count == _results.Count);
            }

            private List<Task> GeneratePostTasks()
            {
                var postTaskList = new List<Task>();

                _inputList.ForEach(i =>
                {
                    var task = new Task(() =>
                    {
                        var response = _controller.Post(i);
                        var newResult = ((OkNegotiatedContentResult<Web.Models.Eatable>)response).Content;

                        _results.Add(newResult);
                    });

                    task.Start();
                    postTaskList.Add(task);
                });

                return postTaskList;
            }

            private List<Task> GenerateDeleteTasks()
            {
                var deleteTaskList = new List<Task>();

                _results.ForEach(r =>
                {
                    var task = new Task(() =>
                    {
                        var response = _controller.Delete(r.Id);

                        _deleteResponseList.Add(response);
                    });

                    task.Start();
                    deleteTaskList.Add(task);
                });
                return deleteTaskList;
            }
        }

        protected Web.Models.EatableInput GenerateUniqEatable()
        {
            var random = new Random();
            var randomNumber = random.Next(10, 100);
            var name = $"new-eatable{ randomNumber }";

            return new Web.Models.EatableInput { Name = name, Price = Convert.ToDecimal(randomNumber) };
        }
    }
}
