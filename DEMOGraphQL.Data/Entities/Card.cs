using HotChocolate;
using HotChocolate.Authorization;
using System.ComponentModel.DataAnnotations;

namespace DEMOGraphQL.Data.Entities
{
    public class Card
    {
        [GraphQLIgnore]
        public int Id { get; set; }

        [MaxLength(16), MinLength(16)]
        public string Number { get; set; }

        public string Holder { get; set; }

        public string Currency { get; set; }

        public TypeCard TypeCard { get; set; }

        public bool IsBlocked { get; set; }
    }
}
