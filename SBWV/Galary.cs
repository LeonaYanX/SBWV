using System;
using System.Collections.Generic;

namespace SBWV;

public partial class Galary
{
    public int Id { get; set; }

    public int? IdBook { get; set; }

    public byte[]? Photo { get; set; }

    public virtual Book? IdBookNavigation { get; set; }
}
