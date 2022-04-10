using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class GenContext : DbContext
    {
        public GenContext(DbContextOptions<GenContext> options) : base(options) { }

    }
}
