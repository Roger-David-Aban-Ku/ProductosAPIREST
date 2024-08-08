using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductosAPIREST.Context;
using ProductosAPIREST.Models;

namespace ProductosAPIREST.Controllers
{
    [ApiController]
    [Route("api/productos")]
    public class ProductosController : Controller
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<IEnumerable<Producto>>> ListarProductos()
        {
            var productos = await _context.Productos.ToListAsync();

            if (productos == null)
            {
                return NotFound();
            }

            return Ok(productos);
        }

        [HttpPost]
        [Route("agregar")]
        public IActionResult CalcularCompra(Producto producto)
        {
            if (producto == null)
            {
                return BadRequest("No se ha proporciono ningún producto.");
            }

            var Subtotal = producto.precio * producto.cantidad;
            var IVA = Subtotal * 0.16m;
            var Total = Subtotal + IVA;

            return Ok(new
            {
                producto.nombre,
                producto.precio,
                producto.cantidad,
                Subtotal,
                IVA,
                Total
            });

        }

        [HttpPost]
        [Route("comprar")]
        public async Task<IActionResult> AgregarProducto(Producto producto)
        {
            if (producto == null)
            {
                return BadRequest("No se ha proporciono ningún producto.");
            }

            await _context.Productos.AddAsync(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(AgregarProducto), new { id = producto.id }, producto);
        }

        [HttpGet]
        [Route("visualizar/{id}")]
        public async Task<IActionResult> CompraTotal(int id)
        {
            Producto producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound("No existe un producto con ese identificador.");
            }
            var Subtotal = producto.cantidad * producto.precio;
            var IVA = Subtotal * 0.16m;
            var Total = Subtotal + IVA;

            return Ok(new
            {
                producto.nombre,
                producto.precio,
                producto.cantidad,
                Subtotal,
                IVA,
                Total
            });
        }

        [HttpPut]
        [Route("editar/{id}")]
        public async Task<IActionResult> editarCompra(int id, Producto producto)
        {
            var productoExistente = await _context.Productos.FindAsync(id);
            if (productoExistente == null)
            {
                return NotFound("No existe un producto con ese identificador.");
            }
            productoExistente.nombre = producto.nombre;
            productoExistente.descripcion = producto.descripcion;
            productoExistente.precio = producto.precio;
            productoExistente.cantidad = producto.cantidad;
            await _context.SaveChangesAsync();

            return Ok(productoExistente);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public async Task<IActionResult> eliminarCompra(int id)
        {
            var productoeliminado = await _context.Productos.FindAsync(id);
            if(productoeliminado == null)
            {
                return NotFound("No existe un producto con ese identificador.");
            }
            _context.Productos.Remove(productoeliminado);
            await _context.SaveChangesAsync();
            return Ok($"Se elminó el produccto con id: {id}");
        }
    }
}