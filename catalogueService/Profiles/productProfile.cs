using catalogueService.Database;
using catalogueService.Models;
using catalogueService.requestETresponse;
using AutoMapper;
using catalogueService.Database.DBsets;
using catalogueService.Authentication;
using catalogueService.Models;

namespace catalogueService.Profiles
{
    public class productProfile : AutoMapper.Profile
    {
        public productProfile()
        {
            CreateMap<product, productModel>().ReverseMap();
            CreateMap<product, productRequest>().ReverseMap();
            CreateMap<category, categoryModel>().ReverseMap();
            CreateMap<category, categoryRequest>().ReverseMap();
            CreateMap<customer, customerModel>().ReverseMap();
            CreateMap<customer, customerRequest>().ReverseMap();
            CreateMap<employees, employeeModel>().ReverseMap();
            CreateMap<employees, employeeRequest>().ReverseMap();
            CreateMap<location, locationModel>().ReverseMap();
            CreateMap<location, locationRequest>().ReverseMap();
            CreateMap<supplier, supplierModel>().ReverseMap();
            CreateMap<supplier, supplierRequest>().ReverseMap();
            CreateMap<users, userModel>().ReverseMap();
            CreateMap<users, userRequest>().ReverseMap();
            CreateMap<users, authUserRequest>().ReverseMap();
            CreateMap<users, authUserModel>().ReverseMap();
            CreateMap<type, typeModel>().ReverseMap();
            CreateMap<type, typeRequest>().ReverseMap();
            CreateMap<orderModel, orders>().ReverseMap();
            CreateMap<orderRequest, orders>().ReverseMap();
            CreateMap<sales, saleModel>().ReverseMap();
        }
    }
}
