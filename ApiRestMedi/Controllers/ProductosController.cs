using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiRestMedi.Context;
using ApiRestMedi.Service;
using ApiRestMedi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ApiRestMedi.ManejoErrores;

namespace ApiRestMedi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly Servicio _servicio;
        private readonly AppDbContext _context;
        

        public ProductosController(AppDbContext context, Servicio servicio)
        {
            _context = context;
            _servicio = servicio;

        }
        //listar productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Productos>>> GettProductos()
        {
            try
            {
                var productos = await _servicio.ListarProductosAsync();
                return Ok(productos);
            }
            catch (ProductoException ex)
            {
                // Aquí puedes registrar el error si estás usando un sistema de logging
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message); // Error interno del servidor
            }
            catch (Exception ex)
            {
                // Manejo genérico de errores
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }
        //insertar productos
        [HttpPost]
        public async Task<IActionResult> crear([FromBody] Productos productos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            var resultado = await _servicio.InsertarDatosAsync(productos);
            if (resultado)
            {
                return CreatedAtAction(nameof(crear), productos);
            }
            return StatusCode(500, "Error al insertar datos");
        }
        //buscar productos
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<Productos>>> BuscarProductosPorNombre([FromQuery] string nombre)
        {
            try
            {
                var productos = await _servicio.BuscarProductosPorNombreAsync(nombre);
                if (productos == null || !productos.Any())
                {
                    return NotFound("No se encontraron productos con ese nombre.");
                }
                return Ok(productos);
            }
            catch (ProductoException ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }
        //modificar productos
        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarProducto(int id, [FromBody] Productos productos)
        {
            if (id != productos.IdProducto)
            {
                return BadRequest("El ID del producto no coincide.");
            }

            try
            {
                var resultado = await _servicio.ActualizarProductoAsync(productos);
                if (!resultado)
                {
                    return NotFound("No se encontró el producto para actualizar.");
                }
                // Devuelve un código 200 OK con un mensaje de éxito
                return Ok(new { mensaje = "Datos actualizados correctamente." });
            }
            catch (ProductoException ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }


        [HttpPut("{Id}")]
        public async Task<ActionResult> DardeBajaProducto(int id, [FromBody] Productos productos)
        {
            if (id != productos.IdProducto)
            {
                return BadRequest("El ID del producto no coincide.");
            }

            try
            {
                var resultado = await _servicio.DardeBajaProducto(productos);
                if (!resultado)
                {
                    return NotFound("No se encontró el producto para darle de baja.");
                }
                // Devuelve un código 200 OK con un mensaje de éxito
                return Ok(new { mensaje = "Producto dado de baja correctamente." });
            }
            catch (ProductoException ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

    }
}
