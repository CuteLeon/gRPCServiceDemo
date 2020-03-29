using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcServiceDemo.Protos;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new EmployeeService.EmployeeServiceClient(channel);
            var response = await client.GetByNoAsync(new GetByNoRequest() { No = 102 });
            Console.WriteLine(response.Employee.FirstName);
            Console.ReadLine();
        }
    }
}
