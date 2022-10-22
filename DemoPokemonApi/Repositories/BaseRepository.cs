using DemoPokemonApi.Data;
using DemoPokemonApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DemoPokemonApi.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected PokemonWorldContext PokemonWorldContext { get; set; }
    public BaseRepository(PokemonWorldContext pokemonWorldContext)
    {
        PokemonWorldContext = pokemonWorldContext;
    }
    public IQueryable<T> GetAll() => PokemonWorldContext.Set<T>().AsNoTracking();
    public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression) =>
        PokemonWorldContext.Set<T>().Where(expression).AsNoTracking();
    public void Create(T entity) => PokemonWorldContext.Set<T>().Add(entity);
    public void Update(T entity) => PokemonWorldContext.Set<T>().Update(entity);
    public void Delete(T entity) => PokemonWorldContext.Set<T>().Remove(entity);
}
