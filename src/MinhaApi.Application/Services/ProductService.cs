using AutoMapper;
using MinhaApi.DTOs;
using MinhaApi.Entities;
using MinhaApi.Repositories;

namespace MinhaApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public Task<List<Product>> GetAll() => _repo.GetAllAsync();
    public Task<Product?> GetById(int id) => _repo.GetByIdAsync(id);

    public async Task<Product> Create(ProductDto dto)
    {
        var product = _mapper.Map<Product>(dto);
        return await _repo.AddAsync(product);
    }

    public async Task<Product?> Update(int id, ProductDto dto)
    {
        var product = await _repo.GetByIdAsync(id);
        if (product == null) return null;

        _mapper.Map(dto, product);
        return await _repo.UpdateAsync(product);
    }

    public Task<bool> Delete(int id) => _repo.DeleteAsync(id);
}
