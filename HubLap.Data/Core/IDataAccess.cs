using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubLap.Data.Core
{
     public interface IDataAccess
    { //metodo para leer datos 
        Task<IEnumerable<T>> LoadData<T>(string storedProcedure, object parameters = null);

        // metodo para guardar y editar datos 
        Task SaveData<T>(string storedProcedure, T parameters, string connectionStringName = "DefaultConnection");
    }
}
