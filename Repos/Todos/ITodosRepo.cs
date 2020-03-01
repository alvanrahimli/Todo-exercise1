using System.Collections.Generic;
using System.Threading.Tasks;
using ex1_ToDo.Models;
using ToDo_exercise1.Models.Dtos;

namespace ex1_ToDo.Repos.Todos
{
    public interface ITodosRepo
    {
        Task<List<TodoReturnDto>> GetTodos(int rn, int c, string email);
        Task<TodoReturnDto> GetTodo(int id, string email);
        Task<bool> CreateTodo(TodoCreateDto todo, string email);
        Task<bool> UpdateTodo(TodoUpdateDto todo, string email);
        Task<(bool res, int code)> DeleteTodo(int id, string email);
    }
}