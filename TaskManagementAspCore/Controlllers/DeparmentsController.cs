﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using TaskManagementAspCore.Services;
using TaskManagementAspCore.Models;
using TaskManagementAspCore.Data;

namespace TaskManagementAspCore.Controllers
{
    [Route("departments")]
    public class DepartmentController : Controller
    {
        /*
        //GETTERS
        */
        [HttpGet]
        [Route("")]
        //[Authorize(Roles = "manager")]
        public async Task<ActionResult<List<Department>>> GetAction(
           [FromServices] DataContext context,
           [FromBody] Department model)
        {
            var department = await context
            .Departments
            .Include(x => x.CheckoutProcesses)
            .Include(x => x.Users)
            .AsNoTracking()
            .ToListAsync();
            return department;
        }

        /*[HttpGet]
        [Route("anonimo")]
        [AllowAnonymous]
        public string Anonimo() => "Anonimo";

        [HttpGet]
        [Route("autenticado")]
        [Authorize]
        public string Autenticado() => "Autenticado";

        [HttpGet]
        [Route("funcionario")]
        [Authorize(Roles = "employee")]
        public string Funcionario() => "Funcionario";

        [HttpGet]
        [Route("gerente")]
        [Authorize(Roles = "manager")]
        public string Gerente() => "Gerente";*/



        /*
        //POSTERS
        */
        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        //[Authorize(Roles = "manager")]
        public async Task<ActionResult<Department>> Post(
            [FromServices] DataContext context,
            [FromBody] Department model)
        {
            //Verifica se os dados são válidos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                //Força o usuário a ser sempre "funcionário"
                //model.Role = "employee";

                context.Departments.Add(model);
                await context.SaveChangesAsync();

                //Esconde a senha
                //model.Password = "";
                return model;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar o usuário" });
            }
        }

        /*
        //PUTTERS
        */
        [HttpPut]
        [Route("{id:int}")]
        // [Authorize(Roles = "manager")]
        public async Task<ActionResult<Department>> Put(
                    [FromServices] DataContext context,
                    [FromBody] Department model, int id)
        {
            //Verifica se os dados são válidos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar o usuário" });
            }
        }

        /*
        //DELETTERS
        */
        [HttpDelete]
        [Route("{id:int}")]
        // [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> Delete(
        int id,
       [FromServices] DataContext _context)
        {
            var departments = await _context.Departments.FirstOrDefaultAsync(x => x.Id == id);
            if (departments == null)
            {
                return NotFound(new { message = "Usuario não encontrado" });
            }
            try
            {
                _context.Departments.Remove(departments);
                await _context.SaveChangesAsync();
                return Ok(new { message = $"Usuário {departments.Id} removido com sucesso" });
            }
            catch
            {
                return BadRequest(new { message = "Não foi possivel remover a categoria" });
            }

        }
    }
}
