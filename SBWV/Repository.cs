namespace SBWV
{
    public class Repository
    {
        private SwapBookDbContext dbContext;

        public Repository(SwapBookDbContext db)
        {
            dbContext = db;
        }

        public void AddUser(User user)
        {
            SwapBookDbContext swapBookDbContext = new SwapBookDbContext();

            swapBookDbContext.Add(user);

            swapBookDbContext.SaveChanges();
        } 
    }
}
