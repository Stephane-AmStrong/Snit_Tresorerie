
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEmailSenderRepository
    {
        void Send(Message message);
        Task SendAsync(Message message);
    }
}
