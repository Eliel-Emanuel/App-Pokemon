using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Store
{
    public class PokemonNegocio
    {
        public List<Pokemon> listar()
        {
			List<Pokemon> lista = new List<Pokemon>();
            AccesoDatos conexion = new AccesoDatos();

			try
			{
				conexion.setQuery("Select P.Id, P.Numero, P.Nombre, P.Descripcion, P.UrlImagen,P.IdTipo, P.IdDebilidad, E.Descripcion Tipo, D.Descripcion Debilidad From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo and D.Id = P.IdDebilidad and Activo = 1");
				conexion.ejecutarLectura();

				while (conexion.Lector.Read())
				{
                    Pokemon aux = new Pokemon();

                    aux.Id = (int)conexion.Lector["Id"];
                    aux.Numero = (int)conexion.Lector["Numero"];
                    aux.Nombre = (string)conexion.Lector["Nombre"];
                    aux.Descripcion = (string)conexion.Lector["Descripcion"];
                    if (!(conexion.Lector["UrlImagen"] is DBNull))
                        aux.Imagen = (string)conexion.Lector["UrlImagen"];
                    aux.Tipo = new Elemento();
                    aux.Tipo.Descripcion = (string)conexion.Lector["Tipo"];
                    aux.Tipo.Id = (int)conexion.Lector["IdTipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Descripcion = (string)conexion.Lector["Debilidad"];
                    aux.Debilidad.Id = (int)conexion.Lector["IdDebilidad"];

                    
                    lista.Add(aux);
                }

				return lista;
                    
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

        public void add(Pokemon nuevo)
        {
            AccesoDatos conexion = new AccesoDatos();
            try
            {
                conexion.setQuery("insert into POKEMONS (Numero, Nombre, Descripcion, UrlImagen , IdTipo, IdDebilidad, Activo) values (" + nuevo.Numero + ", '" + nuevo.Nombre + "', '" + nuevo.Descripcion + "', '" + nuevo.Imagen + "', @IdTipo, @IdDebilidad, 1)");
                conexion.setParametro("@IdTipo", nuevo.Tipo.Id);
                conexion.setParametro("@IdDebilidad", nuevo.Debilidad.Id);
                conexion.ejecutarEscritura();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { conexion.cerrarConexion(); }
        }

        public void modify(Pokemon modificado)
        {
            AccesoDatos conexion = new AccesoDatos();

            try
            {
                conexion.setQuery("Update POKEMONS set Numero = @numero, Nombre = @nombre, Descripcion = @descripcion, UrlImagen = @imagen, IdTipo = @idTipo, IdDebilidad = @idDebilidad where Id = @id");

                conexion.setParametro("@numero", modificado.Numero);
                conexion.setParametro("@nombre", modificado.Nombre);
                conexion.setParametro("@descripcion", modificado.Descripcion);
                conexion.setParametro("@imagen", modificado.Imagen);
                conexion.setParametro("@idTipo", modificado.Tipo.Id);
                conexion.setParametro("@idDebilidad", modificado.Debilidad.Id);
                conexion.setParametro("@id", modificado.Id);

                conexion.ejecutarEscritura();

            }
            catch (Exception ex)
            {
                throw ex;

            }finally { conexion.cerrarConexion(); }
        }

        public void delete(int id)
        {
            AccesoDatos conexion = new AccesoDatos();

            try
            {
                conexion.setQuery("Update POKEMONS set Activo = 0 where Id = @id");
                conexion.setParametro("@id", id);
                conexion.ejecutarEscritura();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Pokemon> Filtrar(string campo, string criterio, string filtro)
        {
            List<Pokemon> list = new List<Pokemon>();
            AccesoDatos conexion = new AccesoDatos();

            try
            {
                string consulta;
                consulta = "Select P.Id, P.Numero, P.Nombre, P.Descripcion, P.UrlImagen,P.IdTipo, P.IdDebilidad, E.Descripcion Tipo, D.Descripcion Debilidad From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo and D.Id = P.IdDebilidad and Activo = 1 and ";

                switch (campo)
                {
                    case "Número":
                        switch (criterio)
                        {
                            case "Mayor a":
                                consulta += "Numero > " + filtro;
                                break;

                            case "Menor a":
                                consulta += "Numero < " + filtro;
                                break;

                            case "Igual a":
                                consulta += "Numero = " + filtro;
                                break;
                        }

                        break;

                    case "Nombre":
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "P.Nombre like '" + filtro + "%'";
                                break;

                            case "Termina con":
                                consulta += "P.Nombre like '%" + filtro + "'";
                                break;

                            case "Contiene":
                                consulta += "P.Nombre like '%" + filtro + "%'";
                                break;
                        }
                        break;

                    case "Descripción":
                        switch (criterio)
                        {

                            case "Comienza con":
                                consulta += "P.Descripcion like '" + filtro + "%'";
                                break;

                            case "Termina con":
                                consulta += "P.Descripcion like '%" + filtro + "'";
                                break;

                            case "Contiene":
                                consulta += "P.Descripcion like '%" + filtro + "%'";
                                break;
                            default:
                                break;
                        }
                        break;
                }
                conexion.setQuery(consulta);
                conexion.ejecutarLectura();

                while (conexion.Lector.Read())
                {
                    Pokemon aux = new Pokemon();

                    aux.Id = (int)conexion.Lector["Id"];
                    aux.Numero = (int)conexion.Lector["Numero"];
                    aux.Nombre = (string)conexion.Lector["Nombre"];
                    aux.Descripcion = (string)conexion.Lector["Descripcion"];
                    if (!(conexion.Lector["UrlImagen"] is DBNull))
                        aux.Imagen = (string)conexion.Lector["UrlImagen"];
                    aux.Tipo = new Elemento();
                    aux.Tipo.Descripcion = (string)conexion.Lector["Tipo"];
                    aux.Tipo.Id = (int)conexion.Lector["IdTipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Descripcion = (string)conexion.Lector["Debilidad"];
                    aux.Debilidad.Id = (int)conexion.Lector["IdDebilidad"];


                    list.Add(aux);
                }

                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
