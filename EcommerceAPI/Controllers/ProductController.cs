using AutoMapper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.DTO;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork<Products> unitOfWork;
        private readonly IMapper mapper;
        public ApiResponse response;

        public ProductController(IUnitOfWork<Products> unitOfWork,IMapper mapper) {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            response =new ApiResponse();
        }
        [HttpGet]
        public  async Task<ActionResult<ApiResponse>> GetAll()
        {
            var model = await unitOfWork.productsRepositories.GetAll();
            var check=model.Any();
            if (check)
            {
                response.statusCode =System.Net.HttpStatusCode.OK;
                response.IsSuccess= check;
                var mappedProducts=mapper.Map<IEnumerable<Products>,IEnumerable<ProductDTO>>(model);
                response.Result = mappedProducts    ;
                return response;

            }
            else {
                response.Erorrmessages = "not products found";
                response.IsSuccess= false;
                response.statusCode=System.Net.HttpStatusCode.OK;
                return response;
            }
            

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetById(int id)
        {
            //var model = dbContext.Products.Find(id);
            var model= await unitOfWork.productsRepositories.GetById(id);
            return Ok(model);

        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateProduct( ProductCreateDTO productCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                response.IsSuccess = false;
                response.statusCode = System.Net.HttpStatusCode.BadRequest;
                response.Erorrmessages = "Invalid data.";
                return BadRequest(response);
            }

            var product = mapper.Map<Products>(productCreateDTO);
            await unitOfWork.productsRepositories.Create(product);
            await unitOfWork.save();

            response.IsSuccess = true;
            response.statusCode = System.Net.HttpStatusCode.Created;
            response.Result = product;
            return Ok(response);

        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse>> UpdateProduct(Products model)
        {
            unitOfWork.productsRepositories.Update(model);
           await unitOfWork.save();
            return Ok(model);


        }

        [HttpDelete]
        public async Task<ActionResult<ApiResponse>> DeleteProduct(int id)
        {
            unitOfWork.productsRepositories.Delete(id);
            await unitOfWork.save();
            return Ok();


        }
        [HttpGet("Product/{cat_id}")]
        public async Task <ActionResult<ApiResponse>> GetAllProductsByCategoryId(int cat_id)
        {
            var Products=await unitOfWork.productsRepositories.GetAllProductsByCategoryId(cat_id);
            var mappedProducts = mapper.Map<IEnumerable<Products>, IEnumerable<ProductDTO>>(Products);
            return Ok(mappedProducts);
        }
    }
}

