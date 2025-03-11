using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.Interfaces;
using ProductManagement.Domain.Entities;

namespace ProductManagement.API.Controllers
{
    [Route("api/products/history")]
    [ApiController]
    public class ProductsHistoryController : ControllerBase
    {
        private readonly IProductHistoryService _productHistoryService;
        public ProductsHistoryController(IProductHistoryService productHistoryService)
        {
            _productHistoryService = productHistoryService;        
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<IEnumerable<ProductHistory>>> GetProductHistory(int productId)
        {
            var history = await _productHistoryService.GetProductHistoryAsync(productId);
            return Ok(history);
        }
    }
}
