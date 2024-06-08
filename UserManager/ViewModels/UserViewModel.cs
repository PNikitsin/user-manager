namespace UserManager.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LoginedAt { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsSelected { get; set; }
    }
}