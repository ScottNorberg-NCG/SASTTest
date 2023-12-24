using System;
using System.Collections.Generic;

namespace SASTTest.EF;

public partial class UserFile
{
    public int FileID { get; set; }

    public int UserID { get; set; }

    public string FileName { get; set; } = null!;

    public string FileExtension { get; set; } = null!;

    public byte[] FileBytes { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public virtual SiteUser User { get; set; } = null!;
}
