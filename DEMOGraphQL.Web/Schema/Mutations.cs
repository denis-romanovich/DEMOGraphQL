using DEMOGraphQL.Data.EF;
using DEMOGraphQL.Data.Entities;
using HotChocolate.Subscriptions;

namespace DEMOGraphQL.SChema
{
    public class Mutations
    {
        public async Task<Card> CreateCard([Service] DataBaseContext dataBaseContext, Card card)
        {
            await dataBaseContext.Cards.AddAsync(card);
            await dataBaseContext.SaveChangesAsync();

            return card;
        }

        public async Task<Card> BlockCard([Service] DataBaseContext dataBaseContext, string cardNumber, [Service] ITopicEventSender sender)
        {
            var card = dataBaseContext.Cards.FirstOrDefault(c => c.Number == cardNumber);

            if (card == null)
            {
                throw new ArgumentException($"Карты с номером {cardNumber} не существует.");
            }

            card.IsBlocked = true;
            dataBaseContext.Cards.Update(card);
            await dataBaseContext.SaveChangesAsync();

            await sender.SendAsync(nameof(Subscriptions.OnCardStatusChange), card);

            return card;
        }
    }
}
