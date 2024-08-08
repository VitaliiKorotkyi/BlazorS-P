using Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
 public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterModel model);
        Task<string> LoginAsync(LoginModel model);
    }
}
