namespace UserManager.ViewModels
{
    public class UserListModel
    {
        public IEnumerable<UserViewModel>? Users { get; set; }
        public List<string>? UserIds { get; set; }
    }
}