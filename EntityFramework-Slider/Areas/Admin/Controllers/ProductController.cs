using EntityFramework_Slider.Areas.Admin.ViewModels;
using EntityFramework_Slider.Helpers;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EntityFramework_Slider.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
                                                     //page- hansi sehifede oldugun// take-sehifede productu nece dene gostersin. //
        public async Task< IActionResult> Index(int page = 1,int take = 5)
        {          
            List<Product> products = await _productService.GetPaginatedDatas(page,take);
           
            List<ProductListVM> mappedDatas = GetMappedDatas(products);

            int pageCount = await GetPageCountAsync(take);

            Paginate<ProductListVM> paginatedDatas = new(mappedDatas, page, pageCount); // mappedDatas-productlarin listi
                                                                                        // page- hal hazirda hasi sehifede oldugun
            return View(paginatedDatas);                                                // pageCount - ne qeder count varsa onlar
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var productCount = await _productService.GetCountAsync();
            return (int) Math.Ceiling ((decimal)productCount / take);  // Celling- (decimal) istediyi ucun decimala cust edirik.
        }                                                              //oz metoduna gore ise (int)-e cust edirik 

      //private-de (async) yazilmama sebebi   Baza ile hec bir elaqesi omamasi 

                                    //yuxaridaki produktu bu metodun icerisine gondermek ucun//
        private List<ProductListVM> GetMappedDatas(List<Product> products)                   //Mapp = birlesdirmek,uygunlasdirmaq
        {
             // productlar list seklinde yazildi ve onnan object yaradildi ( bir classdan object yaratmaq ucun(istifade etmek ucun) instance almaq lazimdi) //

            List<ProductListVM> mappedDatas = new();

            foreach (var product in products)   //  elde olan produclari bir-bir elde etmek ucun
            {
                ProductListVM productsVM = new()  //tipleri ferqli oldugu ucun ProductListVM-den new yaradilir.Listden ayri denesinnen yaradilib,icin doldurub ona aid olan liste qoyulur //
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Count = product.Count,
                    CategoryName = product.Category.Name,
                    MainImage = product.Images.Where(m => m.IsMain).FirstOrDefault()?.Image
                };

                mappedDatas.Add(productsVM);
            }
            return mappedDatas;
        }   

    }
}
