using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using gRPCServiceDemo.Data;
using gRPCServiceDemo.Protos;
using Microsoft.Extensions.Logging;

namespace gRPCServiceDemo.Services
{
    public class MyEmployeeService : EmployeeService.EmployeeServiceBase
    {
        private readonly ILogger<MyEmployeeService> logger;

        public MyEmployeeService(ILogger<MyEmployeeService> logger)
        {
            this.logger = logger;
        }

        public override async Task<EmployeeResponse> GetByNo(GetByNoRequest request, ServerCallContext context)
        {
            var metaData = context.RequestHeaders;
            foreach (var pair in metaData)
            {
                this.logger.LogInformation($"{pair.Key} : {pair.Value}");
            }

            var employee = InMemoryData.Employees.SingleOrDefault(employee => employee.No == request.No);
            if (employee != null)
            {
                return new EmployeeResponse() { Employee = employee };
            }

            throw new Exception("Employee Not Found.");
        }

        public override async Task GetAll(GetAllRequest request, IServerStreamWriter<EmployeeResponse> responseStream, ServerCallContext context)
        {
            foreach (var employee in InMemoryData.Employees)
            {
                await responseStream.WriteAsync(new EmployeeResponse() { Employee = employee });
            }
        }

        public override async Task<UploadPhotoResponse> UploadPhoto(IAsyncStreamReader<UploadPhotoRequest> requestStream, ServerCallContext context)
        {
            var memoryStream = new MemoryStream();
            while (await requestStream.MoveNext())
            {
                await memoryStream.WriteAsync(requestStream.Current.Data.ToByteArray());
            }

            Console.WriteLine(memoryStream.Length);
            return new UploadPhotoResponse() { Result = true };
        }

        public override async Task<EmployeeResponse> Save(EmployeeRequest request, ServerCallContext context)
        {
            InMemoryData.Employees.Add(request.Employee);
            return new EmployeeResponse() { Employee = request.Employee };
        }

        public override async Task SaveAll(IAsyncStreamReader<EmployeeRequest> requestStream, IServerStreamWriter<EmployeeResponse> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                var response = await Save(requestStream.Current, context);
                await responseStream.WriteAsync(response);
            }
        }
    }
}
