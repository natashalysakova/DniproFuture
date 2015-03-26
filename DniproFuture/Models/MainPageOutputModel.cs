using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace DniproFuture.Models
{
    public class MainPageOutputModel
    {
        public DonationOutputModel[] DonationBlock { get; set; }
        public NewsOutputModel[] NewsBlock { get; set; }
        public PartnersOutputModel[] PartnersBlock { get; set; }
        public ClientsOutputModel[] ClientsBlock { get; set; }
        public ContactsOutputModel ContactsBlock { get; set; }
    }

    static class ExtentionClass
    {
        public static bool Contain(this ClientsOutputModel[] model, string clientName)
        {
            for (int i = 0; i < model.Length; i++)
            {
                if (model[i] != null)
                {
                    if (model[i].FullName == clientName)
                        return true;
                }
            }
            return false;
        }
    }
}