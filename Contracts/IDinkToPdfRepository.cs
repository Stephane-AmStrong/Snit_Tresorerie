using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IDinkToPdfRepository
    {
        public enum Operation
        {
            Display,
            GenerateFile,
            Print
        }

        Task<byte[]> GeneratePdfAsync(string url, Operation operation = Operation.GenerateFile);
    }
}
