using Microsoft.AspNetCore.Components;

namespace MinhaPagina.Models
{
    public class Sistema
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        [Parameter] public string Category { get; set; } = "Sistema";
        public string Stack { get; set; } = "";
        public string Status { get; set; } = "";
    }
}
