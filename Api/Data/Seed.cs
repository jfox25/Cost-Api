using System.Threading.Tasks;
using Api.Models;

namespace Api.Data
{
    public class Seed
    {
        public static async Task SeedDb(ApiContext context)
        {
            var lookupTypeCategory = new LookupType() {
                LookupName = "Category"
            };
            context.LookupTypes.Add(lookupTypeCategory);
            var lookupTypeDirective = new LookupType() {
                LookupName = "Directive"
            };
            context.LookupTypes.Add(lookupTypeDirective);
            var lookupTypeBusiness = new LookupType() {
                LookupName = "Business"
            };
            context.LookupTypes.Add(lookupTypeBusiness);

            await context.SaveChangesAsync();
        }
    }
}