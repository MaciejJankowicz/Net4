using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace zaj04.Models
{
    public class CompanyContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Team> Teams { get; set; }

        public CompanyContext( ) :base("Kopytko")
         {
            Database.SetInitializer(new CompanyInitializer());
        }

    }

    public class CompanyInitializer : CreateDatabaseIfNotExists<CompanyContext>
    {
        protected override void Seed(CompanyContext context)
        {
            base.Seed(context);
            InitDatabase(context);
        }

        public void InitDatabase(CompanyContext context)
        {
            context.Teams.Add(new Team
            {
                Name = "ATH",
                Members = new List<Member>
                    {
                        new Member {Name="Janek", MemberType = MemberType.Dev },
                        new Member {Name="Zenek", MemberType = MemberType.Tester },
                    }
            });
            context.Teams.Add(new Team
            {
                Name = "EUVIC",
                Members = new List<Member>
                    {
                        new Member {Name="Malgosia", MemberType = MemberType.Dev },
                        new Member {Name="Zosia", MemberType = MemberType.Tester },
                    }
            });
        }

    }
}