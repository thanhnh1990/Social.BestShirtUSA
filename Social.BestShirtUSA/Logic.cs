using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.BestShirtUSA
{
    class Logic : ConnectMySQL
    {
        public int execute(DataLoad data)
        {
            int status = 0;
            Logs log = new Logs();
            try
            {
                QueryLocal local = new QueryLocal();
                if (!Is_Exist_Server(data))
                {
                    string id = NewProduct(data);
                    local.Insert_Posting(data.CateProdId, Int32.Parse(id));
                    status = 1;
                }else
                    status = 2;
                var db = DBConnection.Instance();
                db.Close();
                return status;
                
            }
            catch (Exception ex)
            {
                log.IErrors("Logic - Execute: " + ex.Message);
                return status;
            }
        }

        public bool Is_Exist_Server(DataLoad data)
        {
            bool result = false;
            Logs log = new Logs();
            Query server = new Query();
            var product_id = server.Check_Product_By_Id(data.Sku);
            if (!string.IsNullOrEmpty(product_id))
            {
                if(string.IsNullOrEmpty(server.Select_SEO(product_id))){
                    server.Add_Product_Meta_Seo(product_id, data);
                }
                string[] types = { "product_cat", "product_tag" };
                string cat = data.CategorySearch;
                foreach (var type in types)
                {
                    if (type == "product_tag")
                        cat = data.Tag;
                    //else 
                    //    if (cat != data.Category)
                    //        Update_New_Cat_Tag_Exist_Product(product_id, data.Category, type, 1);
                    Update_New_Cat_Tag_Exist_Product(product_id, cat, type, 1);
                }
                result = true;
            }
            return result;
        }

        public void Update_New_Cat_Tag_Exist_Product(string product_id, string category, string type, int show)
        {
            Query server = new Query();
            Logs log = new Logs();
            if (string.IsNullOrEmpty(server.Check_Exists_Cat_Tag(product_id, category, type)))
            {
                var category_id = server.Select_Category(category, type);
                if (!string.IsNullOrEmpty(category_id))
                {
                    var taxonomy = server.Select_Taxonomy(category_id);
                    server.Add_Tag_Relationships(product_id, taxonomy[0]);
                    server.Update_Taxonomy_By_Taxonomy_Id(taxonomy[0]);
                    if (show == 1)
                        log.ILogs("Exist: " + product_id + " - Add new : " + category);
                }
                else
                    log.ILogs("Exist: " + product_id);
            }
        }

        public string NewProduct(DataLoad data)
        {
            string result = null;
            Logs log = new Logs();
            try { 
                Query server = new Query();
                if (data.Sku != null)
                {
                    var product_id = server.AddNewProduct(data);
                    if (product_id != "")
                    {
                        server.AddNewProductMeta(product_id, data);
                        var category_id = server.Select_Category(data.CategorySearch, "product_cat");
                        var tag_id = server.Select_Category(data.TypeName, "product_tag");

                        var tax_cat = server.Select_Taxonomy(category_id);
                        var tax_tag = server.Select_Taxonomy(tag_id);

                        server.Update_Taxonomy_By_Term_Id(tax_cat[0]);
                        server.Update_Taxonomy_By_Term_Id(tax_cat[3]);
                        server.Update_Taxonomy_By_Term_Id(tag_id);
                        server.Add_Relationships(product_id, tax_cat[0], tax_tag[0]);
                        //server.Add_Tag_Relationships(product_id, tax_cat[0]);
                        //server.Add_Tag_Relationships(product_id, tax_tag[0]);

                        Update_New_Cat_Tag_Exist_Product(product_id, data.Tag, "product_tag", 0);
                        log.ILogs("NewProduct: " + product_id + " - " + data.CategorySearch + " - " + data.Tag + " - " + data.TypeName);
                    }
                    result = product_id;
                }
            }
            catch (Exception ex) {
                log.IErrors("Logic - NewProduct: " + ex.Message);
            }
            return result;
        }
    }
}
