using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrpcServiceDemo.Protos;

namespace GrpcServiceDemo.Data
{
    public class InMemoryData
    {
        public static List<Employee> Employees { get; set; } = new List<Employee>()
        {
            new Employee()
            {
                Id=1,
                No=101,
                FirstName="Leon",
                LastName="Liu",
                Salary=20000,
            },
            new Employee()
            {
                Id=2,
                No=102,
                FirstName="Mathilda",
                LastName="Luo",
                Salary=10000,
            },
        };
    }
}
