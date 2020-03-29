using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcServiceDemo.Protos;
using Microsoft.Extensions.Logging;

namespace GrpcServiceDemo.Services
{
    public class MyEmployeeService : EmployeeService.EmployeeServiceBase
    {
        private readonly ILogger<MyEmployeeService> logger;

        public MyEmployeeService(ILogger<MyEmployeeService> logger)
        {
            this.logger = logger;
        }

        public override Task<EmployeeResponse> GetByNo(GetByNoRequest request, ServerCallContext context)
        {
            return base.GetByNo(request, context);
        }
    }
}
