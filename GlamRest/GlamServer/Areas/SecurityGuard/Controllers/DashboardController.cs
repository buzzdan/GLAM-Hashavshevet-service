using GlamServer.Controllers;
using SecurityGuard.Interfaces;
using SecurityGuard.Services;
using SecurityGuard.ViewModels;
using System.Web.Mvc;
using System.Web.Security;

namespace GlamServer.Areas.SecurityGuard.Controllers
{
    [Authorize(Roles = "Administrator")]
    public partial class DashboardController : BaseController
    {
        #region ctors

        private IMembershipService membershipService;
        private IRoleService roleService;

        public DashboardController()
        {
            this.roleService = new RoleService(Roles.Provider);
            this.membershipService = new MembershipService(Membership.Provider);
        }

        #endregion ctors

        public virtual ActionResult Index()
        {
            DashboardViewModel viewModel = new DashboardViewModel();
            int totalRecords;

            membershipService.GetAllUsers(0, 20, out totalRecords);
            viewModel.TotalUserCount = totalRecords.ToString();
            viewModel.TotalUsersOnlineCount = membershipService.GetNumberOfUsersOnline().ToString();
            viewModel.TotalRolesCount = roleService.GetAllRoles().Length.ToString();

            return View(viewModel);
        }
    }
}