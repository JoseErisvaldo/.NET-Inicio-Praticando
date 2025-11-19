using MinhaApi.DTOs;
using MinhaApi.Entities;

namespace MinhaApi.Services;

public interface IProductService
{
    Task<List<Product>> GetAll();
    Task<Product?> GetById(int id);
    Task<Product> Create(ProductDto dto);
    Task<Product?> Update(int id, ProductDto dto);
    Task<bool> Delete(int id);
}
