using DemoPokemonApi.Data;
using DemoPokemonApi.Models;
using DemoPokemonApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace DemoPokemonApi.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseDto
{
    protected PokemonWorldContext PokemonWorldContext { get; set; }
    public BaseRepository(PokemonWorldContext pokemonWorldContext)
    {
        PokemonWorldContext = pokemonWorldContext;
    }
    public IQueryable<T> GetAll() => PokemonWorldContext.Set<T>().AsNoTracking();
    public async Task<IEnumerable<T>> GetAllAsync() => await PokemonWorldContext.Set<T>().AsNoTracking().ToListAsync();

    public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression) =>
        PokemonWorldContext.Set<T>().Where(expression).AsNoTracking();
    public async Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> expression) =>
        await PokemonWorldContext.Set<T>().Where(expression).AsNoTracking().ToListAsync();

    public async Task<bool> Exist(int id) => await PokemonWorldContext.Set<T>().AnyAsync(x => x.Id == id);

    public T Create(T entity) => PokemonWorldContext.Set<T>().Add(entity).Entity;
    public void Update(T entity) => PokemonWorldContext.Set<T>().Update(entity);
    public void Delete(T entity) => PokemonWorldContext.Set<T>().Remove(entity);
}
