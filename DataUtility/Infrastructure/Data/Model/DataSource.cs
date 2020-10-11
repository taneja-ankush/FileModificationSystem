using Microsoft.EntityFrameworkCore;

namespace DataUtility.Infrastructure.Data.Model
{
    public class DataSource
    {
        public Source Source { get; set; }

        public string FilePath { get; set; }

        public DbContext Context { get; set; }
    }
}
