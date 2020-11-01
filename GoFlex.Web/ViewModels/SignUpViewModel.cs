namespace GoFlex.Web.ViewModels
{
    public class SignUpViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsOrganizer { get; set; }
    }
}
