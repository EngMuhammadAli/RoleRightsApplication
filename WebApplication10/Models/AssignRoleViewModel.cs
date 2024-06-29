namespace WebApplication10.Models
{
    public class AssignRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();
        public List<string> UserRoles { get; set; } = new List<string>();
        public List<string> SelectedRoles { get; set; } = new List<string>(); // New property for selected roles
    }


}
