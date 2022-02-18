using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagementSystem.Service.Helper
{
    public interface IUserService
    {   string GetUserId();
        bool IsAuthenticated();
    }
}
