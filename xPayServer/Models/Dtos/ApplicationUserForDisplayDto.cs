namespace xPayServer.Models.Dtos;
public class ApplicationUserForDisplayDto
{
        public bool LockoutEnabled{get; set;}
        public DateTimeOffset LockoutEnd {get; set;}
        public string Username{get;set;}
        public bool Flag {get; set;}
        public decimal Savings {get; set;}
        public decimal WithdrawalLimit {get;set;}

}