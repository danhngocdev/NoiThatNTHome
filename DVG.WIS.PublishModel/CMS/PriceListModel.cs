using DVG.WIS.Core;
using DVG.WIS.Core.Enums;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.PublicModel.CMS
{

    public class PriceListModel
    {
        public PriceListModel()
        {

        }
        public PriceListModel(Entities.PriceList model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
            this.Note = model.Note;
            this.Price = model.Price;
            this.Status = model.Status;
            //this.Link = model.Link;
            this.Unit = (PriceListUnitEnum)model.Unit;
            this.CreatedDate = model.CreateDate;
            this.ModifiedDate = model.ModifiedDate;
           

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        //public string Link { get; set; }
        public int Status { get; set; }
        public PriceListUnitEnum Unit { get; set; }
        public string Note { get; set; }
        //public string UnitName  { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }

        public string UnitName
        {
            get
            {
                return Unit.GetDescription();
            }
        }

        public string StatusName
        {
            get
            {
                return EnumHelper.Instance.ConvertEnumToList<PriceListStatusEnum>().Where(x => x.Id == Status).FirstOrDefault().Name;
            }
        }
        public string CreatedDateStr
        {
            get { return CreatedDate != null ? CreatedDate.Value.ToString("dd/MM/yyyy hh:mm") : string.Empty; }
        }
        public string ModifiedDateStr
        {
            get
            {
                return ModifiedDate != null ? ModifiedDate.Value.ToString("dd/MM/yyyy hh:mm") : string.Empty;
            }
        }


        public string IdStr
        {
            get { return Id.ToString(); }
            set
            {
                Id = !string.IsNullOrEmpty(value) ? Convert.ToInt32(value) : 0;
            }
        }

        public string EncryptProductId
        {
            get { return EncryptUtility.EncryptId(this.Id); }
        }

                                                                          
    }


    public class PriceListModelSearchModel
    {
        public PriceListModelSearchModel()
        {
            this.EditItem = new PriceListModel();
        }

        public int Status { get; set; }
        public string Keyword { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<PriceListModel> ListData { get; set; }
        public PriceListModel EditItem { get; set; }
        public IEnumerable<EnumHelper.Enums> ListStatus { get; set; }
        public IEnumerable<EnumHelper.Enums> ListUnit { get; set; }
    }



}
