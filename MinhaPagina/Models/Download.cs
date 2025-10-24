using Microsoft.AspNetCore.Components;

namespace MinhaPagina.Models
{
    public class Download
    {
        public string Name { get; set; } = "";
        public string Summary { get; set; } = "";
        public string Year { get; set; } = "";
        public string Link { get; set; } = "";
        public string Role { get; set; } = "";
        public string RoleLink { get; set; } = "";
        public string Password { get; set; } = "";

        public virtual string? PasswordString
        {
            get
            {
                try
                {
                    return string.IsNullOrWhiteSpace(Password) ? null : Password.DescriptografarAvancado();
                }
                catch
                {
                    return string.IsNullOrWhiteSpace(Password) ? null : "SenhaDificil";
                }
            }
            set
            {
                Password = string.IsNullOrWhiteSpace(value) ? string.Empty : value.CriptografarAvancado();
            }
        }
    }
}
