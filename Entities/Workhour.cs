using System;
using System.Collections.Generic;

namespace Workhourtrack.Entities;

public partial class Workhour
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public int? ProjectId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string? WorkDescription { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Project? Project { get; set; }
}
