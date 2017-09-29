using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.BestShirtUSA
{
    class QueryLocal
    {

        public IEnumerable<DataLoad> GetProduct()
        {
            List<DataLoad> datas = new List<DataLoad>();
            mmoDataContext context = new mmoDataContext();
            var query = from prod in context.Products
                        join catProd in context.ProductCategoryDetails on prod.Id equals catProd.Product_Id
                        join cat in context.Categories on catProd.Category_Id equals cat.Id
                        where !(from post in context.Postings select post.Product_Id).Contains(cat.Id) && prod.Sku != null
                        select new { prod, cat};
            if(query != null)
            {
                foreach (var i in query)
                {
                    string parent_category = (from a in context.Categories where a.Id == i.cat.Parent_Id select a).FirstOrDefault().Name;
                    DataLoad data = new DataLoad();
                    data.Sku = i.prod.Sku;
                    data.Title = i.prod.Name;
                    data.Description = i.prod.Description;
                    data.Image = i.prod.Image;
                    data.Price = decimal.Parse(i.prod.Price.ToString());
                    data.Keywords = i.prod.Keywords;
                    data.GroupId = i.prod.Group_Id;
                    data.Tag = i.cat.Name;
                    data.Type =Int32.Parse(i.prod.ProductTypeId.ToString());
                    data.TypeName = i.prod.ProductType.Name;
                    data.Url = i.prod.Url_Source;
                    data.UrlName = i.prod.Url_Name.Split('/')[2];
                    data.Category = parent_category;
                    data.CategorySearch = parent_category;
                    data.CateProdId = i.cat.Id;
                    datas.Add(data);
                }
            }
            return datas;
        }

        public bool Insert_Posting(int cate, int id)
        {
            bool result = false;
            try
            {
                mmoDataContext db = new mmoDataContext();
                Posting prod = new Posting
                {
                    Product_Id = cate,
                    Source_Id = 1,
                    Site_id = id
                };
                db.Postings.InsertOnSubmit(prod);
                db.SubmitChanges();
                result = true;
            }
            catch (Exception ex) { }
            return result;
        }

        //public IEnumerable<Product_Type> Get_Product_Type_To_Search()
        //{
        //    mmoDataContext context = new mmoDataContext();
        //    var query = from type in context.Product_Types
        //                where type.Is_Active == 1
        //                select type;
        //    return query;
        //}

            //public Product_Type Get_Product_Type_By_Id(int type_id)
            //{
            //    mmoDataContext context = new mmoDataContext();
            //    var query = from type in context.Product_Types
            //                where type.Is_Active == 1 && type.Id == type_id
            //                select type;
            //    return query.FirstOrDefault();
            //}

            //public bool Check_Product(string product_id, string cat, string tag, int type)
            //{
            //    bool result = false;
            //    try
            //    {
            //        mmoDataContext db = new mmoDataContext();
            //        var query = from product in db.Products
            //                    where product.Id == int.Parse(product_id) 
            //                    && product.Category == cat 
            //                    && product.Tag == tag
            //                    && product.Product_Type_Id == type
            //                    select product;
            //        if (query.Count() > 0) { 
            //            result = true;
            //        }
            //    }
            //    catch (Exception ex) { }
            //    return result;
            //} 

            //public bool Insert_Product(DataLoad data)
            //{
            //    bool result = false;
            //    try { 
            //        mmoDataContext db = new mmoDataContext();
            //        Product prod = new Product {
            //                    Id = int.Parse(data.Sku),
            //                    Group_Id = int.Parse(data.GroupId),
            //                    Category = data.Category,
            //                    Tag = data.Tag,
            //                    Product_Type_Id = data.Type,
            //                    Title = data.Title,
            //                    Description = data.Description,
            //                    Url = data.Url,
            //                    Url_Name = data.UrlName,
            //                    Image = data.Image,
            //                    Price = data.Price,
            //                    Keywords = data.Keywords
            //                };
            //        db.Products.InsertOnSubmit(prod);
            //        db.SubmitChanges();
            //        result = true;
            //    }
            //    catch (Exception ex) { }
            //    return result;
            //}

            //public void Update_Exist(int id)
            //{
            //    try
            //    {
            //        mmoDataContext db = new mmoDataContext();
            //        Product prod = (from p in db.Products
            //                        where p.Id == id
            //                         select p).SingleOrDefault();

            //        prod.Exist = 0;
            //        db.SubmitChanges();
            //    }
            //    catch (Exception ex) { }
            //}
        }
}
