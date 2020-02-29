using System.Collections.Generic;
using System.Threading.Tasks;
using ex1_ToDo.Models;
using ToDo_exercise1.Models.Dtos;

namespace ToDo_exercise1.Repos.Items
{
    public interface IItemsRepo
    {
        Task<Item> GetItem(int id, string email);
        Task<bool> AddRange(List<ItemCreateDto> items, string email);
        Task<bool> Add(ItemCreateDto item, string email);
        Task<bool> Update(ItemUpdateDto item, string email);
        Task<bool> Delete(int id, string email);
        Task<bool> DeleteRange(int[] ids, string email);
    }
}