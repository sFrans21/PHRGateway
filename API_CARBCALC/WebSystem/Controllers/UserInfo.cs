//using Microsoft.AspNetCore.Authentication;
using Microsoft.Owin.Security;
using System;
using System.DirectoryServices.AccountManagement;
using System.Security.Claims;
using System.Configuration;
using System.Reflection.PortableExecutable;
using WebSystem.Models;
using Microsoft.IdentityModel.Protocols;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using System.Collections.Generic;
using System.Data;
using StackExchange.Redis;
using Microsoft.AspNetCore.Authentication;
using AuthenticationProperties = Microsoft.Owin.Security.AuthenticationProperties;

namespace WebSystem.Controllers
{
    public class UserInfo
    {
        //    UserInfo db_GCG = new UserInfo();
        //    public class AuthenticationResult
        //    {
        //        public AuthenticationResult(string errorMessage = null)
        //        {
        //            ErrorMessage = errorMessage;
        //        }

        //        public String ErrorMessage { get; private set; }
        //        public Boolean IsSuccess => String.IsNullOrEmpty(ErrorMessage);
        //    }

        //    private readonly IAuthenticationManager authenticationManager;
        //    private readonly IAuthenticationService MyAuthentication;

        //    public void ADAuthenticationService(IAuthenticationManager authenticationManager)
        //    {
        //        this.authenticationManager = authenticationManager;
        //        //this.authenticationManager = authenticationManager;
        //    }
        //    public AuthenticationResult SignIn(String username, String password)
        //    {
        //        //#if DEBUG
        //        // authenticates against your local machine - for development time
        //        //ContextType authenticationType = ContextType.Machine;
        //        //#else
        //        // authenticates against your Domain AD
        //        ContextType authenticationType = ContextType.Domain;
        //        //#endif
        //        PrincipalContext principalContext = new PrincipalContext(authenticationType);
        //        bool isAuthenticated = false;
        //        UserPrincipal userPrincipal = null;
        //        try
        //        {
        //            isAuthenticated = principalContext.ValidateCredentials(username, password, ContextOptions.Negotiate);
        //            if (isAuthenticated)
        //            {
        //                userPrincipal = UserPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, username);
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            isAuthenticated = false;
        //            userPrincipal = null;
        //        }

        //        if (!isAuthenticated || userPrincipal == null)
        //        {
        //            return new AuthenticationResult("Username or Password is not correct");
        //        }

        //        if (userPrincipal.IsAccountLockedOut())
        //        {
        //            // here can be a security related discussion weather it is worth 
        //            // revealing this information
        //            return new AuthenticationResult("Your account is locked.");
        //        }

        //        if (userPrincipal.Enabled.HasValue && userPrincipal.Enabled.Value == false)
        //        {
        //            // here can be a security related discussion weather it is worth 
        //            // revealing this information
        //            return new AuthenticationResult("Your account is disabled");
        //        }

        //        var identity = CreateIdentity(userPrincipal);

        //        //authenticationManager.SignOut(MyAuthentication.ApplicationCookie);
        //        authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);


        //        return new AuthenticationResult();
        //    }

        //    public static List<GroupPrincipal> GetGroupRoleUser(string username, string domain_name, string container_role)
        //    {

        //        List<GroupPrincipal> listGroupRole = new List<GroupPrincipal>();

        //        //DOMAIN domain = new EntitiesODR_DB().COMPANY.FirstOrDefault(c => c.COMPANY_ID == company_id).DOMAIN;


        //        PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain_name);
        //        UserPrincipal up = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, username);

        //        PrincipalContext roleOU = new PrincipalContext(ContextType.Domain, domain_name, container_role);
        //        GroupPrincipal findAllGroups = new GroupPrincipal(roleOU, "gcg" + "*");
        //        var ps = new PrincipalSearcher(findAllGroups).FindAll();
        //        foreach (GroupPrincipal group in ps)
        //        {
        //            if (up.IsMemberOf(group))
        //            {
        //                listGroupRole.Add(group);
        //            }
        //        }
        //        return listGroupRole;
        //    }

        //    private ClaimsIdentity CreateIdentity(UserPrincipal userPrincipal)
        //    {
        //        var identity = new ClaimsIdentity(MyAuthentication.ApplicationCookie, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        //        identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "Active Directory"));
        //        identity.AddClaim(new Claim(ClaimTypes.Name, userPrincipal.SamAccountName));
        //        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userPrincipal.SamAccountName));
        //        if (!String.IsNullOrEmpty(userPrincipal.EmailAddress))
        //        {
        //            identity.AddClaim(new Claim(ClaimTypes.Email, userPrincipal.EmailAddress));
        //        }


        //        // add your own claims if you need to add more information stored on the cookie

        //        var domain = ConfigurationManager.AppSettings["Domain"];
        //        var container_role = ConfigurationManager.AppSettings["ContainerRole"];

        //        var AllRoleUser = GetGroupRoleUser(userPrincipal.SamAccountName, domain, container_role);
        //        int count = AllRoleUser.Count;

        //        List<Role> listRoles = new List<Role>();

        //        Dictionary<String, String[]> role_permission = new Dictionary<String, String[]>();

        //        if (AllRoleUser.Count > 0)
        //        {
        //            //foreach (var groupRole in AllRoleUser)
        //            //{
        //            //    if (groupRole.Name != null)
        //            //    {
        //            //        var roles = db_GCG.ROLEs.Where(r => r.ROLE_ID == groupRole.Name)
        //            //                  .Select(r => new {

        //            //                      ROLE_ID = r.ROLE_ID,
        //            //                      PERMISSION = r.PERMISSION
        //            //                  }).ToList();

        //            //        if (roles.Count > 0)
        //            //        {
        //            //            foreach (var role in roles)
        //            //            {
        //            //                if (role.PERMISSION != null)
        //            //                {
        //            //                    if (role.ROLE_ID == "gcg-admin")
        //            //                    {
        //            //                        identity.AddClaim(new Claim(ClaimTypes.Role, role.ROLE_ID, groupRole.Name, "GCG_Admin"));
        //            //                    }

        //            //                    string[] lpermission = Array.ConvertAll(role.PERMISSION.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), p => p.Trim());
        //            //                    role_permission[role.ROLE_ID] = lpermission;

        //            //                    foreach (var permission in lpermission)
        //            //                    {
        //            //                        identity.AddClaim(new Claim(ClaimTypes.Role, permission, groupRole.Name, "GCG"));
        //            //                    }
        //            //                }


        //            //            }
        //            //        }
        //            //    }
        //            //}
        //        }

        //        return identity;
        //    }

        //    public void GetUserInfo()
        //    {
        //        ////var username = User.Identity.Name.Split('\\')[1];
        //        //using (var context = new PrincipalContext(ContextType.Domain, "pertamina"))
        //        //{
        //        //    var user = UserPrincipal.FindByIdentity(context, username);

        //        //    DirectoryEntry directoryEntry = user.GetUnderlyingObject() as DirectoryEntry;
        //        //    if (user != null)
        //        //    {
        //        //        ViewBag.UserName = username;
        //        //        ViewBag.Email = user.EmailAddress;
        //        //        ViewBag.DisplayName = user.DisplayName;
        //        //    }
        //        //}
        //        //return View();
        //    }
        //}
    }
}
