using System;
using System.IO;
using System.Threading.Tasks;
using Google.Protobuf;
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

            using var getAllCall = client.GetAll(new GetAllRequest());
            var responseStream = getAllCall.ResponseStream;
            while (await responseStream.MoveNext())
            {
                Console.WriteLine(responseStream.Current.Employee.FirstName);
            }

            FileStream fileStream = File.OpenRead("Dream_x280.jpg");
            using var uploadPhotoCall = client.UploadPhoto();
            while (fileStream.Position < fileStream.Length)
            {
                var buffer = new byte[1024];
                int length = await fileStream.ReadAsync(buffer, 0, buffer.Length);

                if (length < buffer.Length)
                {
                    Array.Resize(ref buffer, length);
                }

                await uploadPhotoCall.RequestStream.WriteAsync(
                    new UploadPhotoRequest() { Data = ByteString.CopyFrom(buffer) });
            }
            await uploadPhotoCall.RequestStream.CompleteAsync();


            using var saveAllCall = client.SaveAll();
            for (int index = 0; index < 5; index++)
            {
                await saveAllCall.RequestStream.WriteAsync(new EmployeeRequest()
                {
                    Employee = new Employee()
                    {
                        Id = 10 + index,
                        FirstName = "Employee",
                        LastName = index.ToString(),
                        No = 110 + index,
                        Salary = 10000 + index * 1000,
                    }
                });
            }
            await saveAllCall.RequestStream.CompleteAsync();

            while (await saveAllCall.ResponseStream.MoveNext())
            {
                Console.WriteLine(saveAllCall.ResponseStream.Current.Employee.Id);
            }

            Console.ReadLine();
        }
    }
}
