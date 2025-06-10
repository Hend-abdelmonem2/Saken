using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Saken_WebApplication.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Implement
{
   public class DummyDataService: IDummyDataService
    {
        private readonly IConfiguration _configuration;

        public DummyDataService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task RunSqlScriptAsync(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Scripts", fileName);
            Console.WriteLine("🔍 Looking for SQL file at path: " + filePath);
            if (!File.Exists(filePath))
                //Console.WriteLine("Full SQL Path: " + filePath);
            throw new FileNotFoundException("SQL script file not found", filePath);
           

            string sql = await File.ReadAllTextAsync(filePath);
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using SqlConnection connection = new(connectionString);
            using SqlCommand command = new(sql, connection);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
