using DEMOGraphQL.Data.EF;
using DEMOGraphQL.Data.Entities;
using HotChocolate.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DEMOGraphQL.SChema
{
    public class Queries
    {
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Card> GetCards([Service] DataBaseContext dataBaseContext)
        {
            return dataBaseContext.Cards;
        }

        [UseProjection]
        public IQueryable<Card> GetCardsByCurrencies([Service] DataBaseContext dataBaseContext, IEnumerable<string> currencies)
        {
            return dataBaseContext.Cards.Where(c => currencies.Contains(c.Currency));
        }

        [UseProjection]
        public IQueryable<Card> GetCardsByHolders([Service] DataBaseContext dataBaseContext, IEnumerable<string> holders)
        {
            return dataBaseContext.Cards.Where(c => holders.Contains(c.Holder));
        }

        [UseOffsetPaging(IncludeTotalCount = true)]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Card> GetCardsWithPagination([Service] DataBaseContext dataBaseContext)
        {
            return dataBaseContext.Cards;
        }

        [Authorize]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Card> GetCardsByAuthorize([Service] DataBaseContext dataBaseContext)
        {
            return dataBaseContext.Cards;
        }
    }
}
