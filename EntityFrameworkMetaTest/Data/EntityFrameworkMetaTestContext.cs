using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkMetaTest.Model;

namespace EntityFrameworkMetaTest.Data
{
    public class EntityFrameworkMetaTestContext : DbContext
    {
        public EntityFrameworkMetaTestContext (DbContextOptions<EntityFrameworkMetaTestContext> options)
            : base(options)
        {
        }

        public DbSet<EntityFrameworkMetaTest.Model.MetaTableModel> MetaTable { get; set; } = default!;
    }
}
