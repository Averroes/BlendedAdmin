using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlendedAdmin.DomainModel.Items;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlendedAdmin.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    public class ItemsModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder builder)
        {
        }
    }
}
