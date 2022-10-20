using Microsoft.AspNetCore.Identity;
using MvcFPTBook.Enums;
namespace MvcFPTBook.Areas.Identity.Data;
public static class ContextSeed
{
    public static async Task SeedRolesAsync(UserManager<BookUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Seed Roles
        await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.StoreOwner.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.Customer.ToString()));
    }
    public static async Task SeedSuperAdminAsync(UserManager<BookUser> userManager, RoleManager<IdentityRole> roleManager)
{
    //Seed Default User
    var defaultUser = new BookUser
    {
        Name = "LoiTran",
        UserName = "superadmin", 
        Email = "superadmin@gmail.com",
        
        EmailConfirmed = true, 
        PhoneNumberConfirmed = true 
    };
    if (userManager.Users.All(u => u.Id != defaultUser.Id))
    {
        var user = await userManager.FindByEmailAsync(defaultUser.Email);
        if(user==null)
        {
            await userManager.CreateAsync(defaultUser, "123Pa$$word");
            await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
            await userManager.AddToRoleAsync(defaultUser, Roles.StoreOwner.ToString());
            await userManager.AddToRoleAsync(defaultUser, Roles.Customer.ToString());
        }
               
    }
}
}