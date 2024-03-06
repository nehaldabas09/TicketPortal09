using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace TicketPortal09.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdminService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> GetAgentIdAsync()
        {
            var agentRole = await _userManager.FindByNameAsync("Agent");
            var agents = await _userManager.GetUsersInRoleAsync(agentRole.Id);
            return agents.FirstOrDefault()?.Id;
        }
    }
}
