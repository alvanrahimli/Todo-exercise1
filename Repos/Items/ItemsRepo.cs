using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ex1_ToDo.Data;
using ex1_ToDo.Models;
using Microsoft.EntityFrameworkCore;
using ToDo_exercise1.Models.Dtos;

namespace ToDo_exercise1.Repos.Items
{
    public class ItemsRepo : IItemsRepo
    {
        private readonly TodoDbContext _context;
        public ItemsRepo(TodoDbContext context)
        {
            this._context = context;
        }

        // GetItems methodu da yazacaqdim, amma sora gordum ki,
        // onsuz da itemlere tekce baxilmiyacaq, list-i get eliyende 
        // itemlerin include elemisem.

        public async Task<Item> GetItem(int id, string email)
        {
            var item = await _context.Items
                .FirstOrDefaultAsync(i => i.Id == id
                && i.Todo.Author.NormalizedEmail == email.ToLower());

            return item;
        }

        public async Task<bool> AddRange(List<ItemCreateDto> items, string email)
        {
            // Bax gor basqa userin todo-suna item elave eliyirmi:
            foreach (var item in items)
            {
                var todo = await _context.Todos
                    .FirstOrDefaultAsync(t => t.Id == item.TodoId
                    && t.Author.NormalizedEmail == email);

                if (todo == null)
                    return false;
            }

            // Herseyi Item listinin icine yigib added olaraq track ele:
            var newItems = new List<Item>();
            for (int i = 0; i < items.Count(); i++)
            {
                newItems.Add(new Item()
                {
                    Content = items[i].Content,
                    isDone = items[i].IsDone,
                    Order = items[i].Order,
                    TodoId = items[i].TodoId
                });
            }
            await _context.Items.AddRangeAsync(newItems);

            var res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<bool> Add(ItemCreateDto item, string email)
        {
            var todo = await _context.Todos
                .FirstOrDefaultAsync(t => t.Id == item.TodoId
                && t.Author.NormalizedEmail == email.ToLower());
            if (todo == null)
                return false;

            var newItem = new Item()
            {
                Content = item.Content,
                isDone = item.IsDone,
                Order = item.Order,
                TodoId = item.TodoId
            };

            await _context.Items.AddAsync(newItem);

            var res = await _context.SaveChangesAsync();

            return res > 0;
        }

        public async Task<bool> Update(ItemUpdateDto item, string email)
        {
            var oldItem = await _context.Items
                .FirstOrDefaultAsync(i => i.Id == item.Id
                && i.Todo.Author.NormalizedEmail == email);

            if (oldItem == null)
                return false;

            oldItem.Content = item.Content;
            oldItem.isDone = item.IsDone;
            oldItem.Order = item.Order;

            var res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<bool> Delete(int id, string email)
        {
            var item = await _context.Items
                .FirstOrDefaultAsync(i => i.Id == id
                && i.Todo.Author.NormalizedEmail == email.ToLower());

            if (item == null)
                return false;

            _context.Items.Remove(item);
            var res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<bool> DeleteRange(int[] ids, string email)
        {
            var itemsToRemove = _context.Items
                .Where(i => ids.Contains(i.Id)
                && i.Todo.Author.NormalizedEmail == email);

            if (itemsToRemove == null)
                return false;

            _context.Items.RemoveRange(itemsToRemove);
            var res = await _context.SaveChangesAsync();
            return res > 0;
        }
    }
}