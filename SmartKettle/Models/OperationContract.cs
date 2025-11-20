namespace SmartKettle.Models
{
    public class OperationContract
    {
        public string Name { get; set; }
        public string PreConditions { get; set; }
        public string PostConditions { get; set; }
        public string Effects { get; set; }
        public string ExampleValid { get; set; }
        public string ExampleInvalid { get; set; }
    }
}