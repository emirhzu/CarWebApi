using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Config
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.CategoryId);
            builder.Property(c => c.CategoryName).IsRequired();
            builder.HasData
            (
                new Category()
                {
                    CategoryId = 1,
                    CategoryName = "Suv"
                },
                new Category()
                {
                    CategoryId = 2,
                    CategoryName = "Sedan"
                },
                new Category()
                {
                    CategoryId = 3,
                    CategoryName = "Hatchback"
                },
                new Category()
                {
                    CategoryId = 4,
                    CategoryName = "Station"
                }
            );
        }
    }
}
