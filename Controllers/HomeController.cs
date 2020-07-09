using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;

namespace Shop.Controller
{
    [Route("home")]
    public class HomeController : ControllerBase
    {
        public async Task<ActionResult<dynamic>> GetTask([FromServices] DataContext context)
        {
            var employee = new User { Id = 1 , Username = "lyan", Password = "lyan",Role = "employee"};
            var manager = new User { Id = 2, Username = "lyanmartins",Password = "lyanmartins", Role = "manager"};

            var category = new Category { Id = 1, Title = "Informatica"};
            var product = new Product {Id = 1, Title = "Notebook",Price = 2800, CategoryId = 1, Category = category};

            context.User.Add(employee);
            context.User.Add(manager);
            context.category.Add(category);
            context.products.Add(product);

            await context.SaveChangesAsync();

            return Ok(new {
                message = "Dados configurados!"
            });

        }
    }
}