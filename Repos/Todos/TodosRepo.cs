using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ex1_ToDo.Data;
using ex1_ToDo.Models;
using Microsoft.EntityFrameworkCore;
using ToDo_exercise1.Models.Dtos;

namespace ex1_ToDo.Repos.Todos
{
    public class TodosRepo : ITodosRepo
    {
        private readonly TodoDbContext _context;
        public TodosRepo(TodoDbContext context)
        {
            this._context = context;
        }

        public async Task<bool> CreateTodo(TodoCreateDto todo, string email)
        {
            Todo newTodo = new Todo()
            {
                Header = todo.Header,
                Priority = todo.Priority,
                CreatedTime = DateTime.Now,
                AuthorEmail = email,
            };

            await _context.Todos.AddAsync(newTodo);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        // *** Burda tuple isletmiye bilerdim, 
        // amma teze oyrenmisem, isdifade eliyim dedim :D ***
        public async Task<(bool res, int code)> DeleteTodo(int id, string email)
        {
            // code: 0=notfound, 1=smth went wrong, 3=succesful
            var todo = await _context.Todos
                .FirstOrDefaultAsync(t => t.Id == id
                && t.Author.NormalizedEmail == email.ToLower());

            if (todo == null)
                return (false, 0);

            _context.Todos.Remove(todo);

            int result = await _context.SaveChangesAsync();
            if (result > 0)
                return (true, 3);

            return (false, 1);
        }

        public async Task<TodoReturnDto> GetTodo(int id, string email)
        {
            var todoReturn = await (from todo in _context.Todos
                                    where todo.Id == id
                                    && todo.Author.NormalizedEmail == email
                                    select new TodoReturnDto
                                    {
                                        Id = todo.Id,
                                        Header = todo.Header,
                                        Priority = todo.Priority,
                                        CreatedTime = todo.CreatedTime
                                    }).SingleAsync();

            var itemReturn = await (from item in _context.Items
                                    where item.TodoId == todoReturn.Id
                                    orderby item.Order
                                    select new ItemReturnDto
                                    {
                                        Id = item.Id,
                                        Content = item.Content,
                                        isDone = item.isDone,
                                        Order = item.Order,
                                        TodoId = item.TodoId
                                    }).ToListAsync();

            todoReturn.Items = itemReturn;

            return todoReturn;
        }

        public async Task<List<TodoReturnDto>> GetTodos(int rn, int c, string email)
        {
            // Do not include items
            var todos = await (from todo in _context.Todos
                               where todo.Author.NormalizedEmail == email.ToLower()
                               orderby todo.Priority
                               select new TodoReturnDto
                               {
                                   Id = todo.Id,
                                   Header = todo.Header,
                                   CreatedTime = todo.CreatedTime,
                                   Priority = todo.Priority,
                                   Items = null
                               }).Skip((rn - 1) * c).Take(c).ToListAsync();

            return todos;
        }

        public async Task<bool> UpdateTodo(TodoUpdateDto todo, string email)
        {
            var oldTodo = await _context.Todos
                .FirstOrDefaultAsync(t => t.Id == todo.Id
                && t.Author.NormalizedEmail == email.ToLower());

            if (oldTodo != null)
            {
                oldTodo.Header = todo.Header;
                oldTodo.Priority = todo.Priority;
                return true;
            }
            return false;
        }
    }
}