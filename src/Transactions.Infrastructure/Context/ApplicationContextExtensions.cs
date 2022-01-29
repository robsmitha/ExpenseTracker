using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Domain.Entities;

namespace Transactions.Infrastructure.Context
{
    public static class ApplicationContextExtensions
    {
        public static async Task SeedDataAsync(this ApplicationContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (!await context.Categories.AnyAsync())
            {
                context.Categories.AddRange(new[] { 
                    new Category
                    {
                        Name = "Rent"
                    },
                    new Category
                    {
                        Name = "Renters Insurance"
                    },
                    new Category
                    {
                        Name = "Medical Insurance"
                    },
                    new Category
                    {
                        Name = "Utilities"
                    },
                    new Category
                    {
                        Name = "Car"
                    },
                    new Category
                    {
                        Name = "Student Loan"
                    },
                    new Category
                    {
                        Name = "Phone"
                    },
                    new Category
                    {
                        Name = "Hulu"
                    },
                    new Category
                    {
                        Name = "Spotify"
                    },
                    new Category
                    {
                        Name = "HBO Max"
                    },
                    new Category
                    {
                        Name = "Gas"
                    },
                    new Category
                    {
                        Name = "Grocery"
                    },
                    new Category
                    {
                        Name = "Doctor"
                    },
                    new Category
                    {
                        Name = "MMJ"
                    },
                    new Category
                    {
                        Name = "Pets"
                    },
                    new Category
                    {
                        Name = "Savings"
                    },
                    new Category
                    {
                        Name = "Shopping"
                    },
                    new Category
                    {
                        Name = "Hobbies"
                    },
                    new Category
                    {
                        Name = "Flex"
                    }
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
