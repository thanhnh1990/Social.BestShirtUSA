using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.BestShirtUSA
{
    class Query : ConnectMySQL
    {
        public string Check_Product_By_Id(string id)
        {
            string result = "";
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = string.Format("SELECT id FROM wp_slov_posts where post_content_filtered = '{0}'", id);
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.GetString(0);
                }
                reader.Close();
            }
            return result;
        }

        public string Check_Exists_Cat_Tag(string id, string cat, string type)
        {
            string result = "";
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = string.Format(@"SELECT DISTINCT c.term_id FROM wp_slov_term_taxonomy as t
                                INNER JOIN wp_slov_term_relationships as r on t.term_taxonomy_id = r.term_taxonomy_id
                                INNER JOIN wp_slov_terms as c on t.term_id = c.term_id
                                WHERE c.name= '{1}' and  r.object_id = '{0}' and t.taxonomy = '{2}'", id, cat, type);
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows) { 
                    while (reader.Read())
                        {
                            result = reader.GetString(0);
                        }
                }
                reader.Close();
            }
            return result;
        }

        public string Select_Category(string cat_name, string type)
        {
            string result = "";
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = string.Format("SELECT a.term_id FROM wp_slov_terms as a inner JOIN wp_slov_term_taxonomy as t on a.term_id = t.term_id where a.name = '{0}' and t.taxonomy = '{1}'", cat_name, type);
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.GetString(0);
                }
                reader.Close();
            }
            return result;
        }

        public List<string> Select_Taxonomy(string term_id)
        {
            var taxList = new List<string>();
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = string.Format("SELECT * FROM wp_slov_term_taxonomy where term_id = '{0}'", term_id);
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    taxList.Add(reader.GetString("term_taxonomy_id"));
                    taxList.Add(reader.GetString("term_id"));
                    taxList.Add(reader.GetString("taxonomy"));
                    taxList.Add(reader.GetString("parent"));
                }
                reader.Close();
            }
            return taxList;
        }

        public string Add_Relationships(string id, string id_tax, string id_tag)
        {
            string result = "";
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                MySqlCommand comm = dbCon.Connection.CreateCommand();
                comm.CommandText = @"INSERT INTO wp_slov_term_relationships (
                                        object_id,
                                        term_taxonomy_id,
                                        term_order)
                                        VALUES 
                                        (
                                            @object_id,
                                            249,
                                            0
                                        ),
                                        (
                                            @object_id,
                                            @id_tax,
                                            0
                                        ),
                                        (
                                            @object_id,
                                            @id_tag,
                                            0
                                        )";
                comm.Parameters.AddWithValue("@object_id",String.Format("{0}",  id));
                comm.Parameters.AddWithValue("@id_tax", String.Format("{0}", id_tax));
                comm.Parameters.AddWithValue("@id_tag", String.Format("{0}", id_tag));
                comm.ExecuteNonQuery();
            }
            return result;
        }

        public void Add_Tag_Relationships(string object_id, string taxonomy_id)
        {
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                MySqlCommand comm = dbCon.Connection.CreateCommand();
                comm.CommandText = @"INSERT INTO wp_slov_term_relationships (
                                    object_id,
                                    term_taxonomy_id,
                                    term_order)
                                    VALUES
                                    (
                                        @object_id,
                                        @taxonomy_id,
                                        0
                                    )";
                comm.Parameters.AddWithValue("@object_id", String.Format("{0}", object_id));
                comm.Parameters.AddWithValue("@taxonomy_id", String.Format("{0}", taxonomy_id));
                comm.ExecuteNonQuery();
            }
        }
        public string Update_Taxonomy_By_Taxonomy_Id(string taxonomy_id)
        {
            string result = "";
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                MySqlCommand comm = dbCon.Connection.CreateCommand();
                comm.CommandText = @"UPDATE wp_slov_term_taxonomy
                                    SET count = count + 1
                                    WHERE term_taxonomy_id = @taxonomy_id";
                comm.Parameters.AddWithValue("@taxonomy_id", String.Format("{0}", taxonomy_id));
                comm.ExecuteNonQuery();
            }
            return result;
        }

        public string Update_Taxonomy_By_Term_Id(string term_id)
        {
            string result = "";
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                MySqlCommand comm = dbCon.Connection.CreateCommand();
                comm.CommandText = @"UPDATE wp_slov_term_taxonomy
                                    SET count = count + 1
                                    WHERE term_id = @term_id";
                comm.Parameters.AddWithValue("@term_id", String.Format("{0}", term_id));
                comm.ExecuteNonQuery();
            }
            return result;
        }
        public string Update_Product(string id, DataLoad data)
        {
            string result = "";
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                MySqlCommand comm = dbCon.Connection.CreateCommand();
                comm.CommandText = @"UPDATE wp_slov_posts SET 
                                    post_excerpt = @description, 
                                    post_name = @url_name, 
                                    guid = @url 
                                    WHERE ID = @id";
                string url_name =  data.UrlName;
                comm.Parameters.AddWithValue("@id", String.Format("{0}", id));
                comm.Parameters.AddWithValue("@description", String.Format("{0}", data.Description));
                comm.Parameters.AddWithValue("@url_name", String.Format("{0}", url_name));
                comm.Parameters.AddWithValue("@url", String.Format("{0}", url_name));
                comm.ExecuteNonQuery();
            }
            return result;
        }

        public void Add_Product_Meta_Seo(string product_id, DataLoad data)
        {
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                MySqlCommand comm = dbCon.Connection.CreateCommand();
                comm.CommandText = @"INSERT INTO wp_slov_postmeta (
                                    post_id,
                                    meta_key,
                                    meta_value)
                                    VALUES
                                    (@id, '_yoast_wpseo_focuskw', @title),
                                    (@id, '_yoast_wpseo_focuskw_text_input', @title),
                                    (@id, '_yoast_wpseo_metadesc', @description),
                                    (@id, '_yoast_wpseo_metakeywords', @keywords),
                                    (@id, '_yoast_wpseo_primary_product_cat', ''),
                                    (@id, '_yoast_wpseo_linkdex', '41')";
                comm.Parameters.AddWithValue("@id", String.Format("{0}", product_id));
                comm.Parameters.AddWithValue("@title", String.Format("{0}", data.Title));
                comm.Parameters.AddWithValue("@description", String.Format("{0}", data.Description));
                comm.Parameters.AddWithValue("@keywords", String.Format("{0}", data.Keywords));
                comm.ExecuteNonQuery();
            }
        }
        public string Select_SEO(string id)
        {
            string result = "";
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = string.Format("SELECT meta_id FROM wp_slov_postmeta where post_id = '{0}' and meta_key= '_yoast_wpseo_primary_product_cat'", id);
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.GetString(0);
                }
                reader.Close();
            }
            return result;
        }

        public string AddNewProduct(DataLoad data)
        {
            string result = "";
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                MySqlCommand comm = dbCon.Connection.CreateCommand();
                comm.CommandText = @"INSERT INTO wp_slov_posts (
                                      post_author,
                                      post_date,
                                      post_date_gmt,
                                      post_content,
                                      post_title,
                                      post_excerpt,
                                      post_status,
                                      comment_status,
                                      ping_status,
                                      post_name,
                                      post_type,
                                      post_modified,
                                      post_modified_gmt,
                                      guid,
                                      post_content_filtered
                                      )
                                      VALUES (1,
                                      @datetime,
                                      @datetime,
                                      @url,
                                      @title,
                                      @description,
                                      'publish',
                                      'closed',
                                      'closed',
                                      @url_name,
                                      'product',
                                      @datetime,
                                      @datetime,
                                      @url_name,
                                      @sku
                                      )";
                string url_name = data.UrlName;
                comm.Parameters.AddWithValue("@datetime", String.Format("{0}", DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")));
                comm.Parameters.AddWithValue("@title", String.Format("{0}", data.Title));
                comm.Parameters.AddWithValue("@description", String.Format("{0}", data.Description));
                comm.Parameters.AddWithValue("@url", String.Format("{0}", data.Url));
                comm.Parameters.AddWithValue("@url_name", String.Format("{0}", url_name));
                comm.Parameters.AddWithValue("@sku", String.Format("{0}", data.Sku));
                comm.ExecuteNonQuery();
                result = comm.LastInsertedId.ToString();
            }
            return result;
        }
        public string AddNewProductMeta(string product_id, DataLoad data)
        {
            string result = "";
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                MySqlCommand comm = dbCon.Connection.CreateCommand();
                comm.CommandText = @"INSERT INTO wp_slov_postmeta (
                                        post_id,
                                        meta_key,
                                        meta_value)
                                        VALUES
                                        (@id, '_wpb_vc_js_status', 'false'),
                                        (@id, '_price', @price),
                                        (@id, '_stock_status', 'instock'),
                                        (@id, '_download_expiry', -1),
                                        (@id, '_download_limit', -1),
                                        (@id, '_product_image_gallery', ''),
                                        (@id, '_downloadable', 'no'),
                                        (@id, '_virtual', 'no'),
                                        (@id, '_default_attributes', 'a:0:{}'),
                                        (@id, '_purchase_note', 'a:0:{}'),
                                        (@id, '_crosssell_ids', 'a:0:{}'),
                                        (@id, '_upsell_ids', 'a:0:{}'),
                                        (@id, '_height', ''),
                                        (@id, '_width', ''),
                                        (@id, '_length', ''),
                                        (@id, '_weight', ''),
                                        (@id, '_sold_individually', 'no'),
                                        (@id, '_backorders', 'no'),
                                        (@id, '_manage_stock', 'no'),
                                        (@id, '_tax_class', ''),
                                        (@id, '_tax_status', 'taxable'),
                                        (@id, 'total_sales', '0'),
                                        (@id, '_sale_price_dates_to', ''),
                                        (@id, '_sale_price_dates_from', ''),
                                        (@id, '_sale_price', @price),
                                        (@id, '_regular_price', ''),
                                        (@id, '_sku', @sku),
                                        (@id, 'fifu_image_url', @image),
                                        (@id, 'fifu_image_alt', @title),
                                        (@id, '_yoast_wpseo_focuskw', @title),
                                        (@id, '_yoast_wpseo_focuskw_text_input', @title),
                                        (@id, '_yoast_wpseo_metadesc', @description),
                                        (@id, '_yoast_wpseo_metakeywords', @keywords),
                                        (@id, '_yoast_wpseo_primary_product_cat', ''),
                                        (@id, '_yoast_wpseo_linkdex', '41'),
                                        (@id, '_thumbnail_id', '-1')";
                string url_name = data.UrlName;
                comm.Parameters.AddWithValue("@id", String.Format("{0}", product_id));
                comm.Parameters.AddWithValue("@price", String.Format("{0}", data.Price));
                comm.Parameters.AddWithValue("@title", String.Format("{0}", data.Title));
                comm.Parameters.AddWithValue("@description", String.Format("{0}", data.Description));
                comm.Parameters.AddWithValue("@keywords", String.Format("{0}", data.Keywords));
                comm.Parameters.AddWithValue("@sku", String.Format("{0}", data.Sku));
                comm.Parameters.AddWithValue("@image", String.Format("{0}", data.Image));
                comm.ExecuteNonQuery();
            }
            return result;
        }
    }
}
