
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GestionBanque
{
    public class Bank

    {
    public Bank(string name)
    {
        Name = name;
    }

    public int Id { get; set; }
    public string? Name { get; set; }

    }
}
