using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Solicitudes.Pages
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> listClients = new List<ClientInfo>();

        public ClientInfo clientInfo = new ClientInfo();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=190.60.82.42;User ID=jdavid;Password=HoEx1901*;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM [svcSolicitudes].[dbo].[Cliente]";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.contrato = reader.GetInt32(0);
                                clientInfo.cedulaTitular = reader.GetInt32(0);
                                
                                listClients.Add(clientInfo);

                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message);
            }
        }

        public void OnPost() {
            clientInfo.cedulaTitular = Convert.ToInt32(Request.Form["numero_cedula_usuario"]);
            clientInfo.contrato = Convert.ToInt32(Request.Form["numero_contrato_usuario"]);

            //guardar el cliente

            try
            {
                String connectionString = "Data Source=190.60.82.42;Initial Catalog=svcSolicitudes;User ID=jdavid;Password=HoEx1901*;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO [svcSolicitudes].[dbo].[Cliente] " +
                                 "(idCliente, idContrato) VALUES " +
                                 "(@idCliente, @idContrato);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@idCliente", clientInfo.cedulaTitular);
                        command.Parameters.AddWithValue("@idContrato", clientInfo.contrato);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)  
            {
                Console.WriteLine(ex);
            }
        }
    }

    public class ClientInfo
    {
        public int cedulaTitular;
        public int contrato;
    }
}