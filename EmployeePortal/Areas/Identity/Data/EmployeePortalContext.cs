using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortal.Data;

public class EmployeePortalContext : IdentityDbContext<IdentityUser>
{
    public EmployeePortalContext(DbContextOptions<EmployeePortalContext> options)
        : base(options)
    {
    }
}
