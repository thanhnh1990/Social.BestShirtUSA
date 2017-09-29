using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.BestShirtUSA
{
    public class DataLoad
    {
        private string group_id;
        public string GroupId
        {
            get
            {
                return this.group_id;
            }
            set
            {
                this.group_id = value;
            }
        }

        private string sku;
        public string Sku
        {
            get
            {
                return this.sku;
            }
            set
            {
                this.sku = value;
            }
        }

        private string title;
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        private string description;
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        private string url_name;
        public string UrlName
        {
            get
            {
                return this.url_name;
            }
            set
            {
                this.url_name = value;
            }
        }

        private string keywords;
        public string Keywords
        {
            get
            {
                return this.keywords;
            }
            set
            {
                this.keywords = value;
            }
        }

        private decimal price;
        public decimal Price
        {
            get
            {
                return this.price;
            }
            set
            {
                this.price = value;
            }
        }

        private string image;
        public string Image
        {
            get
            {
                return this.image;
            }
            set
            {
                this.image = value;
            }
        }

        private string category;
        public string Category
        {
            get
            {
                return this.category;
            }
            set
            {
                this.category = value;
            }
        }

        private string category_search;
        public string CategorySearch
        {
            get
            {
                return this.category_search;
            }
            set
            {
                this.category_search = value;
            }
        }

        private string url;
        public string Url
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = value;
            }
        }

        private string tag;
        public string Tag
        {
            get
            {
                return this.tag;
            }
            set
            {
                this.tag = value;
            }
        }

        private int type;
        public int Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        private string type_name;
        public string TypeName
        {
            get
            {
                return this.type_name;
            }
            set
            {
                this.type_name = value;
            }
        }

        private int cate_prod_id;
        public int CateProdId
        {
            get
            {
                return this.cate_prod_id;
            }
            set
            {
                this.cate_prod_id = value;
            }
        }
    }
}
