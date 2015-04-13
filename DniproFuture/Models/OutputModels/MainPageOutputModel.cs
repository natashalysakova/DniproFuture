using DniproFuture.Models.InputModels;

namespace DniproFuture.Models.OutputModels
{
    public class MainPageModel
    {
        public NeedHelpOutputModel[] DonationBlock { get; set; }
        public NewsOutputModel[] NewsBlock { get; set; }
        public PartnersModel PartnersBlock { get; set; }
        public NeedHelpOutputModel[] ClientsBlock { get; set; }
        public ContactsInputModel ContactsBlock { get; set; }
        public ProjectOutputModel[] ProjectsBlock { get; set; }
    }
}