namespace Web_Api_Controllers.RequestModels
{
    public class RegistrationRequest
    {
        public String Name { get; set; }

        public String Email { get; set; }

        public String Password { get; set; }

        public String ConfirmPassword { get; set; }
    }
}
