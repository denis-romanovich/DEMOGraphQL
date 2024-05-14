using DEMOGraphQL.Data.EF;
using DEMOGraphQL.Data.Entities;

namespace DEMOGraphQL.SChema
{
    public class Subscriptions
    {
        [Subscribe]
        public Card OnCardStatusChange([EventMessage] Card card)
        {
            return card;
        }
    }
}
