namespace SBWV;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte[]? Photo { get; set; }

    public string Phone { get; set; } = null!;

    public string City { get; set; } = null!;

    public int Age { get; set; }

    public int? IsAdmin { get; set; }

    public bool IsComfirmed { get; set; }

    public string Token { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
