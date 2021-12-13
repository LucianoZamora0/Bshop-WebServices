using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bshop_WebServices.Models;

namespace Bshop_WebServices.Repository.Contracts
{
    /*
     * Interfaz encargada de definir los siguientes metodos para posterior consulta en BD.
     */
    public interface IProductRepository
    {
        /*LLamado para obtener producto por Id*/
        Task<Product> Get(int id);
        /*LLamado para obtener todos los productos por rango*/
        Task<ICollection<Product>> GetAll(int start, int end);
        /*LLamado para obtener el total de productos*/
        Task<int> GetAllCount();
        /*LLamado para obtener todos los productos por categoria*/
        Task<ICollection<Product>> GetByCategory(List<int> categories, int start, int end);
        /*LLamado para obtener el total de productos por categoria*/
        Task<int> GetByCategoryCount(List<int> categories);
        /*LLamado para obtener todos los productos por filtro de categoria o precio*/
        Task<ICollection<Product>> GetByFilters(List<int> categories, int minPrice, int maxPrice, int start, int end);
        /*LLamado para obtener el total de productos por filtro de categoria o precio*/
        Task<int> GetByFiltersCount(List<int> categories, int minPrice, int maxPrice);
        /*LLamado para obtener todos los productos por texto ingresado en la busqueda*/
        Task<ICollection<Product>> GetBySearch(string name, int start, int end);
        /*LLamado para obtener el numero de productos que entrega la busqueda por texto ingresado*/
        Task<int> GetBySearchCount(string name);
    }
}
