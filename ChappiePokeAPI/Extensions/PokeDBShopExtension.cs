using ChappiePokeAPI.DataAccess;
using EntityModels.EntityModels;
using HelperVariables.Globals;
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
            return products;
        }

        public static async Task<Product> UpdateProduct(this PokeDBContext context, Product product)
        {

            var prod = context.Products.First(x => x.ProductID == product.ProductID);
            prod.Name = product.Name;
            prod.Type = product.Type;
            prod.Description = product.Description;
            prod.Cost = product.Cost;
            prod.Price = product.Price;
            var groups = prod.ProductGroups.Count;
            prod.ProductGroups = product.ProductGroups;

            var imagesToDelete = prod.ImageGroups.Where(x => !product.ImageGroups.Any(y => y.ImagePath == x.ImagePath));
            foreach (var img in imagesToDelete)
            {
                var imgPath = System.IO.Path.Combine(Paths.AssetUploadPath, img.ImagePath);
                if (System.IO.File.Exists(imgPath))
                {
                    System.IO.File.Delete(imgPath);
                }
            }
            prod.ImageGroups = product.ImageGroups;
            await context.SaveChangesAsync();
            return prod;
        }
    }
}
