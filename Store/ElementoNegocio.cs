using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dominio;

namespace Store
{
    public class ElementoNegocio
    {
        public List<Elemento> listar()
        {
			List<Elemento> elementos = new List<Elemento>();
			AccesoDatos conexion = new AccesoDatos();
			try
			{
				conexion.setQuery("Select Id, Descripcion from ELEMENTOS");
				conexion.ejecutarLectura();

				while (conexion.Lector.Read())
				{
					Elemento aux = new Elemento();

					aux.Id = (int)conexion.Lector["Id"];
					aux.Descripcion = (string)conexion.Lector["Descripcion"];

					elementos.Add(aux);
				}

				return elementos;
			}
			catch (Exception ex)
			{

				throw ex;
			}
			finally
			{
				conexion.cerrarConexion();
			}
        }
    }
}
