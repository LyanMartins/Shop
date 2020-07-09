using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Service;

namespace Shop.Controller
{
    [Route("users")]
    public class UserController : ControllerBase
    {
        
        [HttpGet]
        [Route("")]
        [Authorize(Roles="manager")]
        public  async Task<ActionResult<List<User>>> get(
            [FromServices]DataContext context
        )
        {
            var user = await context.User
                .AsNoTracking()
                .ToListAsync();
            return user;
        }


        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public  async Task<ActionResult<User>> Post(
            [FromBody]User model,
            [FromServices]DataContext context
        )
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try{

                model.Role = "employee";

                context.User.Add(model);
                await context.SaveChangesAsync();
                model.Password = "";

                return model;
            }catch (Exception){
                return BadRequest(new { message = "Não foi possivel cadastrar o usuario"});
            }
            
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public  async Task<ActionResult<dynamic>> Authenticate(
            [FromBody]User model,
            [FromServices]DataContext context
        )
        {
            var user = await context.User
                .AsNoTracking()
                .Where(x => x.Username == model.Username && x.Password == model.Password)
                .FirstOrDefaultAsync();

            if(user == null)
                return NotFound(new {message = "Usuário não encontrado!"});

            var token = TokenService.GenerateToken(user);
            user.Password = "";
            return new { user = user,token = token };
        }
        [HttpPut]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> put(
            int id,
            [FromBody]User model,
            [FromServices] DataContext context
        )
        {
            var user = await context.User.FirstOrDefaultAsync(x => x.Id == id);

            if(user == null)
                return NotFound(new { message = "Usuário não encontrado!"});
            
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<User>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel atualizar o usuário!" });
            }
        }



    }
}