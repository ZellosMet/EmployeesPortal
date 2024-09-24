using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EmployeePortal.Models;
using Microsoft.Identity.Client;

namespace EmployeePortal.Data;

public class EmployeePortalContext : IdentityDbContext<IdentityUser>
{
    public DbSet<User> Users { get; set; }
    public EmployeePortalContext(DbContextOptions<EmployeePortalContext> options) : base(options) {}
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}