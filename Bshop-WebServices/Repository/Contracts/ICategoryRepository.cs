using Bshop_WebServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bshop_WebServices.Repository.Contracts
{
    /*
     * Interfaz encargada de definir los siguientes metodos para posterior consulta en BD.
     */
    public interface ICategoryRepository
    {
        Task<ICollection<Category>> GetAll();
    }
}
