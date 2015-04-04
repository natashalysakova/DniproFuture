namespace DniproFuture.Models.OutputModels
{
    public class PartnersOutputModel
    {
        public string Title { get; set; }
        public string Logo { get; set; }
    }

    public class PartnersModel
    {
        public PartnersOutputModel[] RandomPartners { get; set; }
        public PartnersOutputModel[] AllPartners { get; set; }
    }
}