using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SAILI.Models
{
    public class IdentityHelper
    {
        internal static void SeedIdentities(ApplicationDbContext DefaultConnection)
        {
            string databaseConnection = ConfigurationManager.ConnectionStrings["SailiDbContext"].ConnectionString;
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DefaultConnection));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(DefaultConnection));
            SailiRepository repository = new SailiRepository();
            Vector<Company> Companies = new Vector<Company>();
            

            
            string userName = "bootstrapAdmin";
            string Email = "bootstrapAdmin@hotmail.com";
            string password ="bootstrapAdmin0@";

            if (!roleManager.RoleExists(RoleNames.ROLE_ADMINISTRATOR))
            {
                var roleresult = roleManager.Create(new IdentityRole(RoleNames.ROLE_ADMINISTRATOR));
            }

            if (!roleManager.RoleExists(RoleNames.ROLE_Trader))
            {
                var roleresult = roleManager.Create(new IdentityRole(RoleNames.ROLE_Trader));
            }

            ApplicationUser user = userManager.FindByName(userName);

            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = userName,
                    Email = Email,
                    EmailConfirmed = true
                };

                IdentityResult userResult = userManager.Create(user, password);

                if (userResult.Succeeded)
                {
                    var result = userManager.AddToRole(user.Id, RoleNames.ROLE_ADMINISTRATOR);

                    DefaultConnection.SaveChanges();
                }

                DefaultConnection.Sectors.Add(new Sector
                {
                    IsDeleted = false,
                    SectorName = "Information Technology",
                    Description = "Information technology (IT) is the application of computers to store, study, retrieve, transmit, and manipulate data, or information, often in the context of a business or other enterprise. IT is considered a subset of information and communications technology (ICT).",
                    ImageUrl = "../Content/Images/MarketSectors/IT.png"
                });

                DefaultConnection.SaveChanges();

                DefaultConnection.Sectors.Add(new Sector
                {
                    IsDeleted = false,
                    SectorName = "Financial",
                    Description = "The financial sector is a category of stocks containing firms that provide financial services to commercial and retail customers; this sector includes banks, investment funds, insurance companies and real estate.",
                    ImageUrl = "../Content/Images/MarketSectors/financials.png"
                });

                DefaultConnection.SaveChanges();

                DefaultConnection.Sectors.Add(new Sector
                {
                    IsDeleted = false,
                    SectorName = "Healthcare",
                    Description = "The healthcare sector is the category of stocks relating to medical and healthcare goods or services. The healthcare sector includes hospital management firms, health maintenance organizations (HMOs), biotechnology and a variety of medical products.",
                    ImageUrl = "../Content/Images/MarketSectors/healthcare.png"
                });

                DefaultConnection.SaveChanges();

                DefaultConnection.Sectors.Add(new Sector
                {
                    IsDeleted = false,
                    SectorName = "Energy",
                    Description = "The energy sector is a category of stocks that relate to producing or supplying energy. This sector includes companies involved in the exploration and development of oil or gas reserves, oil and gas drilling, or integrated power firms.",
                    ImageUrl = "../Content/Images/MarketSectors/energy.png"
                });

                DefaultConnection.SaveChanges();

                DefaultConnection.Sectors.Add(new Sector
                {
                    IsDeleted = false,
                    SectorName = "Industrial",
                    Description = "Goods-producing segment of an economy, including agriculture, construction, fisheries, forestry, and manufacturing.",
                    ImageUrl = "../Content/Images/MarketSectors/industrial.png"
                });

                DefaultConnection.SaveChanges();

                DefaultConnection.Sectors.Add(new Sector
                {
                    IsDeleted = false,
                    SectorName = "Materials",
                    Description = "Materials sector is a category of stocks that accounts for companies involved with the discovery, development and processing of raw materials. The sector includes the mining and refining of metals, chemical producers and forestry products.",
                    ImageUrl = "../Content/Images/MarketSectors/materials.jpg"
                });

                DefaultConnection.SaveChanges();

                DefaultConnection.Sectors.Add(new Sector
                {
                    IsDeleted = false,
                    SectorName = "Consumer Discretionary",
                    Description = "Consumer discretionary is the term given to goods and services that are considered non-essential by consumers, but desirable if their available income is sufficient to purchase them. Consumer discretionary goods include durable goods, apparel, entertainment and leisure, and automobiles.",
                    ImageUrl = "../Content/Images/MarketSectors/ConsumerDiscretionary.png"
                });

                DefaultConnection.SaveChanges();

                DefaultConnection.Sectors.Add(new Sector
                {
                    IsDeleted = false,
                    SectorName = "Consumer Staples",
                    Description = "The consumer staples sector is characterized by its global industry classification sector (GICS). The sector is composed of companies whose primary lines of business are food, beverages, tobacco and other household items.",
                    ImageUrl = "../Content/Images/MarketSectors/Consumerstaples.png"
                });

                DefaultConnection.SaveChanges();

                DefaultConnection.Sectors.Add(new Sector
                {
                    IsDeleted = false,
                    SectorName = "Telecommunication",
                    Description = "The Telecommunication Services comprises companies that make communication possible on a global scale whether through the phone or Internet. These companies created the infrastructure that allows data to be sent anywhere in the world. The largest companies in the sector are wireless operators, satellite companies, cable companies and Internet service providers.",
                    ImageUrl = "../Content/Images/MarketSectors/telecommunication.png"
                });

                DefaultConnection.SaveChanges();

                DefaultConnection.Sectors.Add(new Sector
                {
                    IsDeleted = false,
                    SectorName = "Utilities",
                    Description = "The utilities sector is a category of stocks for utilities such as gas and power. The sector contains companies such as electric, gas and water firms, and integrated providers.",
                    ImageUrl = "../Content/Images/MarketSectors/utilities.png"
                });

                DefaultConnection.SaveChanges();

                DefaultConnection.Sectors.Add(new Sector
                {
                    IsDeleted = false,
                    SectorName = "Real Estate",
                    Description = "The main segments of the real estate sector are residential real estate, commercial real estate and industrial real estate. The residential sector focuses on the buying and selling of properties used for nonbusiness purposes.",
                    ImageUrl = "../Content/Images/MarketSectors/realEstate.png"
                });

                DefaultConnection.SaveChanges();

                DefaultConnection.Homes.Add(new Home
                {
                    About = "Have you ever wondered what it may mean if a machine can answer questions like, Which company has the lowest debt but outperformed a sector that outperformed their own?  Or which company has a low P/E rating but grow more than higher P/E rating companies whose sector outperformed theirs? If questions like these returned some findings, wouldn't that be interesting? Interesting still is how it may be answered by economics. After all, we have to try to find an explanation - the data is free from individual interpretation",
                    HelpName = "Kiet Lam",
                    HelpEmail = "s3386614@student.rmit.edu.au",
                    HelpPhoneNumber = "0412 509 235"
                });

                DefaultConnection.SaveChanges();
            }
        }
    }
}