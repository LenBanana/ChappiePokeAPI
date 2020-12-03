using ChappiePokeAPI.DataAccess;
using EntityModels.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChappiePokeAPI.Extensions
{
    public static class PokeDBShopExtension
    {
        public static List<Product> GetProducts(this PokeDBContext context)
        {
            List<Product> products = context.Products.ToList();
            //products.ForEach(x => x.Cards = context.Cards.Where(y => y.ProductID == x.ProductID).ToList());
            //products.ForEach(x => x.ProductGroups = context.ProductGroups.Where(y => y.ProductID == x.ProductID).ToList());
            //products.ForEach(x => x.ProductGroups.ForEach(y => y.Group = context.Groups.First(z => z.GroupID == y.GroupID)));
            //products.ForEach(x => x.ImageGroups = context.ImageGroups.Where(y => y.ProductID == x.ProductID).ToList());
            return products;
        }
    }
}
