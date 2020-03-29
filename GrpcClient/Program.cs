using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceDemo.Protos;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var metaDatas = new Metadata
            {
                { "username", "Leon" },
                { "role", "administrator" },
            };
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new EmployeeService.EmployeeServiceClient(channel);
            var response = await client.GetByNoAsync(new GetByNoRequest() { No = 102 }, metaDatas);
            Console.WriteLine(response.Employee.FirstName);
            Console.ReadLine();
        }
    }
}
