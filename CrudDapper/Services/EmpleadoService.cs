using CrudDapper.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CrudDapper.Services
{
    public class EmpleadoService
    {

        private readonly IConfiguration _configuration;
        private readonly string cadenaSql;

        public EmpleadoService(IConfiguration configuration)
        {
            _configuration = configuration;
            cadenaSql = _configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<Empleado>> Lista()
        {
            string query = "sp_listaEmpleados";

            using (var con = new SqlConnection(cadenaSql)) {
                
                var lista =  await con.QueryAsync<Empleado>(query,commandType:CommandType.StoredProcedure);
                return lista.ToList();
            }

        }

        public async Task<Empleado> Obtener(int id)
        {
            string query = "sp_obtenerEmpleado";
            var parametros = new DynamicParameters();
            parametros.Add("@idEmpleado", id, dbType: DbType.Int32);

            using (var con = new SqlConnection(cadenaSql))
            {

                var empleado = await con.QueryFirstOrDefaultAsync<Empleado>(query,parametros, commandType: CommandType.StoredProcedure);
                return empleado;
            }

        }


        public async Task<string> Crear(Empleado objeto)
        {
            string query = "sp_crearEmpleado";
            var parametros = new DynamicParameters();
            parametros.Add("@numeroDocumento", objeto.NumeroDocumento, dbType: DbType.String);
            parametros.Add("@nombreCompleto", objeto.NombreCompleto, dbType: DbType.String);
            parametros.Add("@sueldo", objeto.Sueldo, dbType: DbType.Int32);
            parametros.Add("@msgError", dbType: DbType.String,direction:ParameterDirection.Output,size:100);

            using (var con = new SqlConnection(cadenaSql))
            {

                var empleado = await con.ExecuteAsync(query, parametros, commandType: CommandType.StoredProcedure);
                return parametros.Get<string>("@msgError");
            }

        }

        public async Task<string> Editar(Empleado objeto)
        {
            string query = "sp_editarEmpleado";
            var parametros = new DynamicParameters();
            parametros.Add("@idEmpleado", objeto.IdEmpleado, dbType: DbType.Int32);
            parametros.Add("@numeroDocumento", objeto.NumeroDocumento, dbType: DbType.String);
            parametros.Add("@nombreCompleto", objeto.NombreCompleto, dbType: DbType.String);
            parametros.Add("@sueldo", objeto.Sueldo, dbType: DbType.Int32);
            parametros.Add("@msgError", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

            using (var con = new SqlConnection(cadenaSql))
            {
                var empleado = await con.ExecuteAsync(query, parametros, commandType: CommandType.StoredProcedure);
                return parametros.Get<string>("@msgError");
            }

        }


        public async Task Eliminar(int id)
        {
            string query = "sp_eliminarEmpleado";
            var parametros = new DynamicParameters();
            parametros.Add("@idEmpleado", id, dbType: DbType.Int32);

            using (var con = new SqlConnection(cadenaSql))
            {
                await con.ExecuteAsync(query, parametros, commandType: CommandType.StoredProcedure);
            }

        }


    }
}
