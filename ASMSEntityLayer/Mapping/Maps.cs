using ASMSEntityLayer.Models;
using ASMSEntityLayer.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMSEntityLayer.Mapping
{
    public class Maps:Profile
    {
        //Buraya Maps ctor gelecek metodu gelecektir.
        //İçine CreateMap metodu gelecektir.

        public Maps()
        {
            ////UserAdress'ı UserAddressesVM'ye dönüştür
            //CreateMap<UsersAddress, UsersAddressVM>();   //DAL-->BLL

            ////UserAdressVM'yi UserAddresses'e dönüştür.
            //CreateMap<UsersAddress, UsersAddressVM>();   //PL-->BLL-->DAL

            //Yukarıdakinin aynısını tek seferde yapmak
            //UserAdress ve VM'yi birbirine dönüştür.
            CreateMap<UsersAddress, UsersAddressVM>().ReverseMap();


        }


    }
}
