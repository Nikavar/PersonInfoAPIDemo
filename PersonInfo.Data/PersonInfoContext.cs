using Microsoft.EntityFrameworkCore;
using PersonInfo.Data.Configuration;
using PersonInfo.Model.Models;
using PersonInfo.Model.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PersonInfo.Data
{
    public class PersonInfoContext : DbContext
    {
        public PersonInfoContext()
        {

        }

        public PersonInfoContext(DbContextOptions<PersonInfoContext> options) : base(options)
        {

        }

        public DbSet<Person>? Persons { get; set; }
        public DbSet<PersonRelatedPeople>? PersonRelatedPeople { get; set; }
        public DbSet<PhoneNumber>? PhoneNumbers { get; set; }
        public DbSet<PhoneType>? PhoneTypes { get; set; }
        public DbSet<RelationType>? RelationTypes { get; set; }


        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PhoneType>().HasData(
                new PhoneType
                {
                     Id = 1,
                     PhoneNumberType = PhoneNumberType.Office
                },

                new PhoneType
                {
                    Id = 2,
                    PhoneNumberType = PhoneNumberType.Home
                },

                new PhoneType
                {
                    Id=3,
                    PhoneNumberType= PhoneNumberType.Mobile
                });

            modelBuilder.Entity<RelationType>().HasData
               (
               new RelationType
               {
                   Id = 1,
                   Relation = Relation.Relative
               },
                new RelationType
                {
                    Id = 2,
                    Relation = Relation.Colleague
                },
                new RelationType
                {
                    Id = 3,
                    Relation = Relation.Acquaintance
                },
                new RelationType
                {
                    Id = 4,
                    Relation = Relation.Other
                }
           );


            //Map entity to table
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new PersonRelatedPeopleConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneNumberConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RelationTypeConfiguration());

        }
    }
}
