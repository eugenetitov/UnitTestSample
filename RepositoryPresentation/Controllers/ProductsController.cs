using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Repositories.Models;
using Services.Interfaces;

namespace RepositoryPresentation.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ITransactionManager _transactionManager;
        private readonly IATMService _aTMService;

        public ProductsController(IProductService productService, ITransactionManager transactionManager, IATMService aTMService)
        {
            _aTMService = aTMService;
            _productService = productService;
            _transactionManager = transactionManager;
            _aTMService.Withdraw(10, Currency.USD, "", Country.Germany);
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return _productService.GetAllProducts();
        }

        //[HttpGet]
        //[Route("getExpression")]
        //public IEnumerable<Product> GetExpression()
        //{
        //    return _productService.GetAllProductsExpression();
        //}

        [Route("getTop5")]
        [HttpGet]
        public void GetTop5()
        {
            _productService.GetTop5Products();
        }

        [HttpPost]
        public void Post([FromBody]Product product)
        {
            _productService.AddProduct(product);
        }

        [HttpGet]
        [Route("add")]
        public async Task AddDummyProducts()
        {
            //await _transactionManager.ExecuteInTransaction(
            //    async () =>
            //    {
                    await _productService.AddProduct(new Product { Id = 1, Name = "Coca Cola", Price = 2 });
                    await _productService.AddProduct(new Product { Id = 2, Name = "Fanta", Price = 2 });
                    await _productService.AddProduct(new Product { Id = 3, Name = "Snikers", Price = 3 });
              //  });
        }
    }
}
