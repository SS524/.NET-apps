using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserMicroservice.Model;

namespace UserMicroservice.Context
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.Migrate();
        }
        public DbSet<User> UserTbl { get; set; }
    }
}
