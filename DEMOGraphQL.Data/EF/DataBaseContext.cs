using DEMOGraphQL.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DEMOGraphQL.Data.EF
{
    public class DataBaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DataBaseContext(DbContextOptions<DataBaseContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            GenerateParametersTable(modelBuilder);
            InitialData(modelBuilder);
        }

        private void GenerateParametersTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>().HasKey(c => c.Id);
            modelBuilder.Entity<Card>().HasIndex(c => c.Number).IsUnique();
        }

        private void InitialData(ModelBuilder modelBuilder)
        {
            var initialCardsData = new List<Card>
            {
                new Card { 
                    Id = 1, 
                    Number = "1111222233334444", 
                    Holder = "Denis", 
                    Currency="BYN",
                    TypeCard = TypeCard.UnionPay,
                    IsBlocked = false,
                },
                new Card {
                    Id = 2,
                    Number = "1111222233334445",
                    Holder = "Vlad",
                    Currency="EUR",
                    TypeCard = TypeCard.MasterCard,
                    IsBlocked = false,
                },
                new Card {
                    Id = 3,
                    Number = "1111222233334446",
                    Holder = "Ivan",
                    Currency="USD",
                    TypeCard = TypeCard.Visa,
                    IsBlocked = false,
                },
            };

            modelBuilder.Entity<Card>().HasData(initialCardsData);

            var initialUsersData = new List<User>
            {
                new User {
                    Id = 1,
                    Email = "email@gmail.com",
                    Password = "password",
                },
            };

            modelBuilder.Entity<User>().HasData(initialUsersData);
        }
    }
}
