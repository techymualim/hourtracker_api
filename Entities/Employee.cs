using System;
using System.Collections.Generic;

namespace Workhourtrack.Entities;

public partial class Employee
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Workhour> Workhours { get; set; } = new List<Workhour>();
}
