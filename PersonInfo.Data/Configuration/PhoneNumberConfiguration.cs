using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonInfo.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PersonInfo.Data.Configuration
{
    public class PhoneNumberConfiguration : IEntityTypeConfiguration<PhoneNumber>
    {
        public void Configure(EntityTypeBuilder<PhoneNumber> builder)
        {

            builder.ToTable("PhoneNumbers");

            builder.HasKey(pn => pn.Id);

            builder.Property(pn => pn.PhoneNum)
                .HasMaxLength(50)
                .IsUnicode(false);


            // relations

            builder.HasOne(p => p.Person)
                .WithMany(pn => pn.PhoneNumbers);

            builder.HasOne(pt => pt.PhoneTypes)
                .WithMany(pn => pn.PhoneNumbers);
        }
    }
}
