using AutoMapper;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            var user = new Entity()
            {
                Id = Guid.NewGuid(),
                Items = new List<Item>()
                {
                    new Item
                    {
                        Id = Guid.NewGuid(),
                        Name = "Name1",
                        Items = new [] { new Item { Id = Guid.NewGuid(), Name = "Title2" } }

                    }
                }
            };

            var model = user.ConvertTo<Model>();

            Console.WriteLine("Model:");
            Console.WriteLine(model.Id);
            Console.WriteLine("Model.Items:");

            foreach (var item in model.EntityItems)
            {
                Console.WriteLine($"- Id: {item.Id}, Name:{item.UserName}");

                foreach (var item2 in item.Items)
                {
                    Console.WriteLine($"-- Id: {item2.Id}, Name:{item2.UserName}");
                }
            }

            var entity = model.ConvertTo<Entity>();

            Console.WriteLine("Entity:");
            Console.WriteLine(entity.Id);
            Console.WriteLine("Model.Items:");

            foreach (var item in entity.Items)
            {
                Console.WriteLine($"- Id: {item.Id}, Name:{item.Name}");

                foreach (var item2 in item.Items)
                {
                    Console.WriteLine($"-- Id: {item2.Id}, Name:{item2.Name}");
                }
            }
        }
    }

    #region Entities

    public class Entity : IEntity
    {
        [MatchToPropertyTo("Id")]
        public Guid Id { get; set; }

        [MatchToPropertyTo("EntityItems")]
        public IEnumerable<Item> Items { get; set; }

    }

    public class Item
    {
        public Guid Id { get; set; }

        [MatchToPropertyTo("UserName")]
        public string Name { get; set; }


        public IEnumerable<Item> Items { get; set; }
    }

    public class ItemView
    {
        public Guid Id { get; set; }

        [MatchToPropertyTo("Name")]
        public string UserName { get; set; }

        public IEnumerable<ItemView> Items { get; set; }
    }

    public class Model : IModel
    {
        public Guid Id { get; set; }

        [MatchToPropertyTo("Items")]
        public IEnumerable<ItemView> EntityItems { get; set; }
    }

    #endregion
}
