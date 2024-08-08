using BlazorClient.Provider;
using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages
{
    public class DashboardBase : ComponentBase
    {
        [Inject]
        protected NavigationManager Navigation { get; set; }

        [Inject]
        protected ILogger<DashboardBase> Logger { get; set; }

        [Inject]
        protected CustomAuthStateProvider AuthStateProvider { get; set; }

        protected string UserId { get; set; }
        protected string UserName { get; set; }
        protected string Email { get; set; }
    
    }
}
