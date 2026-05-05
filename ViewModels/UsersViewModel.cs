using FURNITRACK.Models;

namespace FURNITRACK.ViewModels
{
    public class UsersViewModel
    {
        public List<User> Users { get; set; } = new List<User>();
        public string? SelectedRole { get; set; }
        public string? SearchTerm { get; set; }
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public Dictionary<string, int> UsersByRole { get; set; } = new Dictionary<string, int>();
    }
}
