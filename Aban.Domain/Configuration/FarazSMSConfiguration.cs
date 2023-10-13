namespace Aban.Domain.Configuration
{
    public class FarazSMSConfiguration
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? FromNumuber { get; set; }
        public string? PatternCode { get; set; }
        public string? APIURL { get; set; }
        public bool IsTest { get; set; }
        public int StaticCode { get; set; }
    }
}
