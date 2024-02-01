using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Tekton.Service.DTO;
using Tekton.Service.Interfaces;

namespace Tekton.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/Products/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ProductDTO))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetById([FromRoute] long id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null) return NotFound("No se encontró registro del Producto");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Products/")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<ProductDTO>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();

                if (result.Count == 0) return NotFound("No se encontraron registros de Producto");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("api/Products/")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ProductDTO))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public async Task<IActionResult> Insert([FromBody] NewProductDTO data)
        {
            try
            {
                var result = await _service.AddAsync(data);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("api/Products/")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ProductDTO))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public async Task<IActionResult> Update([FromBody] EditProductDTO data)
        {
            try
            {
                var result = await _service.UpdateAsync(data);
                if (result == null) return NotFound("No se encontró registro del producto a actualizar");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("api/Products/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                if (result == null) return NotFound("No se encontró registro del producto a eliminar");
                return Ok("Registro de producto eliminado con éxito");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}