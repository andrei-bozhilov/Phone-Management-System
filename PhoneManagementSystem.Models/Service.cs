namespace PhoneManagementSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class Service
    {
        public int Id { get; set; }
        //TYPE
        public string Type { get; set; }
        //LABEL
        public string Label { get; set; }
        //SDATE
        public DateTime? StartDate { get; set; }
        //EDATE
        public DateTime? EndDate { get; set; }
        //AMOUNT
        public decimal? Amount { get; set; }
        //USAGE can be int, time, decimal 
        public string Usage { get; set; }
        //MONTH
        public DateTime? Month { get; set; }
        //INCLMIN
        public TimeSpan? IncludedMin { get; set; }
        //USEDMIN
        public TimeSpan? UsedMin { get; set; }

        public int InvoiceDataId { get; set; }

        public virtual InvoiceData InvoiceData { get; set; }


        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} ---- {1}\n{2} ---- {3}\n{4}\n{5}\n{6}\n{7}----{8}",
                this.Type,
                this.Label,
                this.StartDate,
                this.EndDate,
                this.Amount,
                this.Usage,
                this.Month,
                this.IncludedMin,
                this.UsedMin);


            return sb.ToString();
        }
    }
}
