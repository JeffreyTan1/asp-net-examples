using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerWebsite.Models;

public class BillPayAndAlertsViewModel
{
    public List<BillPay> BillPays { get; set; }
    public bool HasAlerts { get; set; }

}
