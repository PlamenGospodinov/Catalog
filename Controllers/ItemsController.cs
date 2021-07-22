using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers{
    //Get/items
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase{
        private readonly IItemsRepository repository;
        public ItemsController(IItemsRepository repository){
            this.repository = repository;
        }

        //GET/items
        [HttpGet]
        public IEnumerable<ItemDto> GetItems(){
            var items = repository.GetItems().Select(item => item.AsDto());
            return items;
        }

        //GET/items/{id}
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id){
            var item = repository.GetItem(id);
            if(item is null){
                return NotFound();
            }
            return item.AsDto();
        }

        //POST/items
        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto){
            Item item = new(){
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            repository.CreateItem(item);
            return CreatedAtAction(nameof(GetItem),new {id = item.Id},item.AsDto());
        }
    
    }
}