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
    public class PersonRelatedPeopleConfiguration : IEntityTypeConfiguration<PersonRelatedPeople>
    {
        public void Configure(EntityTypeBuilder<PersonRelatedPeople> builder)
        {

            builder.ToTable("PersonRelatedPeople");
            builder.HasKey(p => p.Id);

            builder.HasAlternateKey(prp => new { prp.ParentId, prp.RelationTypeId, prp.RelatedPersonId });

            builder.Property(prp => prp.ParentId)
                .HasColumnName("ParentId");

            builder.Property(prp => prp.RelatedPersonId)
                .HasColumnName("RelatedPersonId");

            builder.Property(prp => prp.RelationTypeId)
                .HasColumnName("RelationTypeId");

            // relations

            builder.HasOne(p => p.Person)
                .WithMany(prp => prp.PersonRelatedPeople)
                .HasForeignKey(prp => prp.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.RelatedPerson)
                .WithMany()
                .HasForeignKey(prp => prp.RelatedPersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(rt => rt.RelationType)
                .WithMany(prp => prp.PersonRelatedPeoples)
                .HasForeignKey(prp => prp.RelationTypeId);
        }
    }
}
