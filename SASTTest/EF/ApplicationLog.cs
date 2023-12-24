using System;
using System.Collections.Generic;

namespace SASTTest.EF;

public partial class ApplicationLog
{
    public int ApplicationLogID { get; set; }

    public string LogLevel { get; set; } = null!;

    public string LogText { get; set; } = null!;

    public DateTime DateLogged { get; set; }
}
