﻿using System;
using System.Collections.Generic;

namespace SASTTest.EF;

public partial class Order
{
    public int OrderID { get; set; }

    public int UserID { get; set; }

    public double? TotalPrice { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DateCompleted { get; set; }

    public string? CreditCardNumber { get; set; }

    public string? CreditCardExpirationMonth { get; set; }

    public int? CreditCardExpirationYear { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual SiteUser User { get; set; } = null!;
}
