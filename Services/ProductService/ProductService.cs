﻿using Microsoft.EntityFrameworkCore;
using web_api_cosmetics_shop.Data;
using web_api_cosmetics_shop.Models.DTO;
using web_api_cosmetics_shop.Models.Entities;

namespace web_api_cosmetics_shop.Services.ProductService
{
	public class ProductService : IProductService
	{
		private readonly CosmeticsShopContext _context;
		public ProductService(CosmeticsShopContext context) {
			_context = context;
		}

		//---------- Add ----------
		public async Task<Product> AddProduct(Product product)
		{
			await _context.Products.AddAsync(product);
			var addProductResult = await _context.SaveChangesAsync();
			if(addProductResult == 0)
			{
				return null!;
			}

            return product;
		}

		public async Task<ProductItem> AddProductItem(ProductItem productItem)
		{
			await _context.ProductItems.AddAsync(productItem);
			var addProductItemResult = await _context.SaveChangesAsync();
			if (addProductItemResult == 0)
			{
				return null!;
			}
			return productItem;
		}

		public async Task<ProductConfiguration> AddProductOption(ProductConfiguration productConfiguration)
		{
			await _context.ProductConfigurations.AddAsync(productConfiguration);
			var addProductOptionResult = await _context.SaveChangesAsync();
			if (addProductOptionResult == 0)
			{
				return null!;
			}

			return productConfiguration;
		}

		public async Task<ProductCategory> AddProductCategory(ProductCategory productCategory)
		{
			await _context.ProductCategories.AddAsync(productCategory);
			var result = await _context.SaveChangesAsync();
			if(result == 0)
			{
				return null!;
			}
			return productCategory;
		}


		//---------- Get ----------
		public async Task<List<Product>> GetAllProducts()
		{
			var products = await _context.Products.ToListAsync();
			return products;
		}

		public async Task<Product> GetProductById(int id)
		{
			var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
			return product!;
		}

		public async Task<List<ProductCategory>> GetCategories(Product product)
		{
			var categories = await _context.ProductCategories.Where(c => c.ProductId == product.ProductId).ToListAsync();
			return categories;
		}

		public async Task<List<ProductItem>> GetItems(Product product)
		{
			var items = await _context.ProductItems.Where(pi => pi.ProductId == product.ProductId).ToListAsync();
			return items;
		}

		public async Task<List<ProductConfiguration>> GetConfigurations(ProductItem productItem)
		{
			var configurations = await _context.ProductConfigurations.Where(pc => pc.ProductItemId == productItem.ProductItemId).ToListAsync();
			return configurations;
		}


		//---------- Remove ----------
		public async Task<int> RemoveProduct(Product product)
		{
			_context.Remove(product);
			var result = await _context.SaveChangesAsync();

			return result;
		}

		public async Task<int> RemoveProductItems(Product product)
		{
			var productItems = await _context.ProductItems
											.Where(pi => pi.ProductId == product.ProductId)
											.ToListAsync();
			_context.RemoveRange(productItems);
			var result = await _context.SaveChangesAsync();

			return result;
		}

		public async Task<int> RemoveProductCategories(Product product)
		{
			var categories = await _context.ProductCategories
												.Where(c => c.ProductId == product.ProductId)
												.ToListAsync();

			_context.RemoveRange(categories);
			var result = await _context.SaveChangesAsync();

			return result;
		}

		public async Task<int> RemoveProductOptions(ProductItem productItem)
		{
			var productOptions = await _context.ProductConfigurations
											.Where(po => po.ProductItemId == productItem.ProductItemId)
											.ToListAsync();

			_context.RemoveRange(productOptions);
			var result = await _context.SaveChangesAsync();

			return result;
		}

		// ---------- Update ----------
		// Update Product
		public async Task<Product> UpdateProduct(Product product)
		{
			var existProduct = await GetProductById(product.ProductId);
			if(existProduct == null)
			{
				return null!;
			}

			existProduct.Name = product.Name;
			existProduct.Description = product.Description;
			existProduct.Image = product.Image;

			var result = await _context.SaveChangesAsync();
			if(result == 0)
			{
				return null!;
			}

			return product;
		}
	}
}