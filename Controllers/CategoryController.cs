using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;


// Endpoint => URL
// https://localhost:5001

[Route("v1/categories")]
public class CategoryController : ControllerBase {

    [HttpGet]
    [Route("")]
    [ResponseCache(VaryByHeader = "User-Agent",Location = ResponseCacheLocation.Any,Duration = 30)]
    public async Task<ActionResult<List<Category>>> Get([FromServices]DataContext context)
    {
        var category = await context.category.AsNoTracking().ToListAsync();
        return Ok(category);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id,[FromServices]DataContext context )
    {
        var category = await context.category.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return category;
    }
    
    [HttpPost]
    [Route("")]
    [Authorize(Roles = "employee")]
    public  async Task<ActionResult<Category>> Post(
        [FromBody]Category model,
        [FromServices]DataContext context
    )
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        try
        {
            context.category.Add(model);
            await context.SaveChangesAsync();

            return Ok(model);
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possivel criar a categoria"});
        }
        
    }
    
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "employee")]

    public  async Task<ActionResult<Category>> Put(int id, 
        [FromBody]Category model,
        [FromServices]DataContext context
    )
    {
        
        if(model.Id != id)
            return NotFound(new {message = "Categoria não encontrada"});

        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            context.Entry<Category>(model).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch (DbUpdateConcurrencyException){
            return BadRequest(new { message = "Este registro já foi atualizado!"});
        }
        catch (Exception){
            return BadRequest(new { message = "Não foi possivel atualizar a categoria"});
        }
    
    }
    
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "employee")]

    public  async Task<ActionResult<Category>> Delete(int id, [FromServices]DataContext context)
    {
        var category = await context.category.FirstOrDefaultAsync(x => x.Id == id);
        if(category == null)
            return NotFound(new {message = "Categoria não encontrada!"});

        try
        {
            context.category.Remove(category);
            await context.SaveChangesAsync();
            return Ok(new {message = "Categoria removida com sucesso!"});
        }
        catch (Exception)
        {
            return BadRequest(new {message = "Não foi possivel remover a categoria!"});
        }
    }

}