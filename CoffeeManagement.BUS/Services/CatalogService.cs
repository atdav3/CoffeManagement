using System;
using System.Collections.Generic;
using System.Data;
using CoffeeManagement.BUS.Exceptions;
using CoffeeManagement.DAL.Repositories;
using CoffeeManagement.DTO.Entities;

namespace CoffeeManagement.BUS.Services
{
    public sealed class CatalogService
    {
        private readonly ProductRepository products = new ProductRepository();
        private readonly CategoryRepository categories = new CategoryRepository();
        private readonly TableRepository tables = new TableRepository();
        private readonly EmployeeRepository employees = new EmployeeRepository();
        private readonly InventoryRepository inventory = new InventoryRepository();
        private readonly PromotionRepository promotions = new PromotionRepository();

        public DataTable GetProducts(string search, int? categoryId) { return products.GetAll(string.IsNullOrWhiteSpace(search) ? null : search.Trim(), categoryId); }
        public DataTable GetCategories() { return categories.GetAll(); }
        public DataTable GetTables() { return tables.GetAll(); }
        public int SaveTable(CafeTableDto table)
        {
            if (table == null || string.IsNullOrWhiteSpace(table.TableName) || table.TableName.Trim().Length > 40)
                throw new BusinessException("Table name is required and must be at most 40 characters.");
            if (string.IsNullOrWhiteSpace(table.AreaName) || table.AreaName.Trim().Length > 60)
                throw new BusinessException("Area name is required and must be at most 60 characters.");
            table.TableName = table.TableName.Trim(); table.AreaName = table.AreaName.Trim();
            if (string.IsNullOrWhiteSpace(table.TableStatus)) table.TableStatus = "Available";
            return tables.Save(table);
        }
        public void DeleteTable(int id) { if (id <= 0) throw new BusinessException("Select a table first."); tables.Delete(id); }
        public void SetTableReservation(int id, bool reserved)
        {
            if (id <= 0) throw new BusinessException("Select a table first.");
            tables.SetStatus(id, reserved ? "Reserved" : "Available");
        }
        public DataTable GetEmployees(string search, string jobTitle = null)
        {
            if (jobTitle != null && jobTitle.Length > 50) throw new BusinessException("Job title filter is too long.");
            return employees.GetAll(string.IsNullOrWhiteSpace(search) ? null : search.Trim(), string.IsNullOrWhiteSpace(jobTitle) ? null : jobTitle.Trim());
        }
        public DataTable GetInventory() { return inventory.GetIngredients(); }
        public DataTable GetInventoryHistory(DateTime start, DateTime end)
        {
            if (start.Date > end.Date) throw new BusinessException("Start date must be on or before end date.");
            return inventory.GetHistory(start.Date, end.Date.AddDays(1));
        }
        public DataTable GetUnits() { return inventory.GetUnits(); }
        public int SaveUnit(UnitDto unit)
        {
            if (unit == null || string.IsNullOrWhiteSpace(unit.UnitName) || unit.UnitName.Trim().Length > 40)
                throw new BusinessException("Unit name is required and must be at most 40 characters.");
            if (string.IsNullOrWhiteSpace(unit.Abbreviation) || unit.Abbreviation.Trim().Length > 12)
                throw new BusinessException("Unit abbreviation is required and must be at most 12 characters.");
            unit.UnitName = unit.UnitName.Trim(); unit.Abbreviation = unit.Abbreviation.Trim();
            return inventory.SaveUnit(unit);
        }
        public void DeleteUnit(int id) { if (id <= 0) throw new BusinessException("Select a unit first."); inventory.DeleteUnit(id); }
        public DataTable GetPromotions(bool activeOnly) { return promotions.GetAll(activeOnly); }
        public int SavePromotion(PromotionDto promotion)
        {
            if (promotion == null || string.IsNullOrWhiteSpace(promotion.PromotionName) || promotion.PromotionName.Trim().Length > 100)
                throw new BusinessException("Promotion name is required and must be at most 100 characters.");
            if (promotion.DiscountPercent <= 0 || promotion.DiscountPercent > 100 || HasMoreThanDecimals(promotion.DiscountPercent, 2)) throw new BusinessException("Discount must be greater than 0 and at most 100 percent with at most 2 decimals.");
            if (promotion.EndsAt <= promotion.StartsAt) throw new BusinessException("Promotion end must be after its start.");
            promotion.PromotionName = promotion.PromotionName.Trim(); return promotions.Save(promotion);
        }
        public void DisablePromotion(int id) { if (id <= 0) throw new BusinessException("Select a promotion first."); promotions.Disable(id); }
        public int SaveIngredient(IngredientDto ingredient)
        {
            if (ingredient == null || string.IsNullOrWhiteSpace(ingredient.IngredientName) || ingredient.IngredientName.Trim().Length > 120)
                throw new BusinessException("Ingredient name is required and must be at most 120 characters.");
            if (ingredient.UnitId <= 0 || ingredient.ReorderLevel < 0 || ingredient.ReorderLevel > 999999999999999.999m || HasMoreThanDecimals(ingredient.ReorderLevel, 3)) throw new BusinessException("Select a unit and enter a reorder level with at most 3 decimals.");
            ingredient.IngredientName = ingredient.IngredientName.Trim(); return inventory.Save(ingredient);
        }
        public void PostInventory(int ingredientId, int userId, string type, decimal quantity, decimal? cost, string note)
        {
            if (ingredientId <= 0 || userId <= 0) throw new BusinessException("Select an ingredient and sign in again.");
            if (type != "Receipt" && type != "Issue") throw new BusinessException("Select receipt or issue.");
            if (quantity <= 0) throw new BusinessException("Quantity must be greater than zero.");
            if (quantity > 999999999999999.999m || HasMoreThanDecimals(quantity, 3)) throw new BusinessException("Quantity must fit the inventory precision of 3 decimal places.");
            if (cost.HasValue && (cost.Value < 0 || cost.Value > 9999999999999999.99m || HasMoreThanDecimals(cost.Value, 2))) throw new BusinessException("Unit cost must be non-negative with at most 2 decimals.");
            if (note != null && note.Length > 300) throw new BusinessException("Inventory note must be at most 300 characters.");
            inventory.Post(ingredientId, userId, type, quantity, cost, note);
        }
        public int SaveEmployee(EmployeeDto employee)
        {
            if (employee == null || string.IsNullOrWhiteSpace(employee.EmployeeCode) || employee.EmployeeCode.Trim().Length > 20)
                throw new BusinessException("Employee code is required and must be at most 20 characters.");
            if (string.IsNullOrWhiteSpace(employee.FullName) || employee.FullName.Trim().Length > 120)
                throw new BusinessException("Employee name is required and must be at most 120 characters.");
            if ((employee.Phone ?? "").Length > 20 || (employee.Email ?? "").Length > 254 || (employee.JobTitle ?? "").Length > 50)
                throw new BusinessException("Phone, email or job title exceeds its maximum length.");
            employee.EmployeeCode = employee.EmployeeCode.Trim(); employee.FullName = employee.FullName.Trim();
            return employees.Save(employee);
        }
        public void DeactivateEmployee(int id) { if (id <= 0) throw new BusinessException("Select an employee first."); employees.Deactivate(id); }

        public int SaveCategory(CategoryDto category)
        {
            if (category == null || string.IsNullOrWhiteSpace(category.CategoryName) || category.CategoryName.Trim().Length > 80)
                throw new BusinessException("Category name is required and must be at most 80 characters.");
            if ((category.Description ?? "").Length > 250) throw new BusinessException("Category description must be at most 250 characters.");
            return categories.Save(category.CategoryId == 0 ? (int?)null : category.CategoryId, category.CategoryName.Trim(), category.Description);
        }

        public void DeleteCategory(int id)
        {
            if (id <= 0) throw new BusinessException("Select a category first.");
            categories.Delete(id);
        }

        public int SaveProduct(ProductDto product)
        {
            if (product == null) throw new BusinessException("Product is required.");
            if (product.CategoryId <= 0) throw new BusinessException("Select a category.");
            if (string.IsNullOrWhiteSpace(product.ProductName) || product.ProductName.Trim().Length > 120)
                throw new BusinessException("Product name is required and must be at most 120 characters.");
            if (product.UnitPrice < 0 || product.UnitPrice > 9999999999999999.99m || HasMoreThanDecimals(product.UnitPrice, 2))
                throw new BusinessException("Price must be non-negative and contain at most 2 decimals.");
            if ((product.Description ?? "").Length > 500 || (product.ImagePath ?? "").Length > 500)
                throw new BusinessException("Product description or image path exceeds its maximum length.");
            product.ProductName = product.ProductName.Trim();
            return products.Save(product);
        }

        public void DeleteProduct(int id)
        {
            if (id <= 0) throw new BusinessException("Select a product first.");
            products.Delete(id);
        }

        private static bool HasMoreThanDecimals(decimal value, int decimalPlaces)
        {
            return decimal.Round(value, decimalPlaces) != value;
        }
    }
}
