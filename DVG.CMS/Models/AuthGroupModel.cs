using DVG.WIS.Core;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DVG.CMS.Models
{
    public class AuthGroupModel
    {
        public AuthGroupModel(AuthGroup obj)
        {
            Id = obj.Id;
            Name = obj.Name;
            Status = obj.Status;
            CreatedDate = obj.CreatedDate;
            CreatedBy = obj.CreatedBy;
            ModifiedDate = obj.ModifiedDate;
            ModifiedBy = obj.ModifiedBy;
            StatusStr = StringUtils.GetEnumDescription((AuthGroupStatusEnum)obj.Status);
            ModifiedDateStr = obj.ModifiedDate.ToString(Const.DateTimeFormatAdmin);
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string StatusStr { get; set; }
        public string ModifiedDateStr { get; set; }
    }

    public class AuthGroupInitModel
    {
        public AuthGroupInitModel(IEnumerable<DropdownTreeCheckboxModel> lstAuthAction, List<DropdownTreeCheckboxModel> lstCategoryModel, IEnumerable<EnumHelper.Enums> lstAuthGroupStatus, IEnumerable<DropdownTreeCheckboxModel> lstNewsStatusDropdown)
        {
            LstCategoryModel = lstCategoryModel;
            LstAuthAction = lstAuthAction;
            LstAuthGroupStatus = lstAuthGroupStatus;
            LstNewsStatus = lstNewsStatusDropdown;
        }

        public List<DropdownTreeCheckboxModel> LstCategoryModel { get; set; }
        public IEnumerable<DropdownTreeCheckboxModel> LstAuthAction { get; set; }
        public IEnumerable<DropdownTreeCheckboxModel> LstNewsStatus { get; set; }
        public IEnumerable<EnumHelper.Enums> LstAuthGroupStatus { get; set; }
    }

    public class AuthGroupViewModel
    {
        public AuthGroupViewModel()
        {
            AuthGroup = new AuthGroup();
            LstSeletedActionModel = new List<DropdownTreeCheckboxModel>();
            LstSeletedCategoryModel = new List<DropdownTreeCheckboxModel>();
        }

        public AuthGroupViewModel(AuthGroup authGroup, IEnumerable<DropdownTreeCheckboxModel> lstActionMapping, List<DropdownTreeCheckboxModel> lstCategoryModel, IEnumerable<DropdownTreeCheckboxModel> lstNewsStatusMapping)
        {
            AuthGroup = authGroup;
            LstSeletedActionModel = lstActionMapping;
            LstSeletedCategoryModel = lstCategoryModel;
            LstSeletedNewsStatusModel = lstNewsStatusMapping;
        }
        public AuthGroup AuthGroup { get; set; }
        public IEnumerable<DropdownTreeCheckboxModel> LstSeletedActionModel { get; set; }
        public List<DropdownTreeCheckboxModel> LstSeletedCategoryModel { get; set; }
        public IEnumerable<DropdownTreeCheckboxModel> LstSeletedNewsStatusModel { get; set; }
    }

    public class AuthGroupUpdateModel
    {
        public AuthGroupUpdateModel()
        {
            AuthGroup = new AuthGroup();
            LstAuthAction = new List<DropdownTreeCheckboxModel>();
            LstCategoryModel = new List<DropdownTreeCheckboxModel>();
            LstNewsStatus = new List<DropdownTreeCheckboxModel>();
        }
        public AuthGroupUpdateModel(AuthGroup authGroup, List<DropdownTreeCheckboxModel> lstCategoryModel, List<DropdownTreeCheckboxModel> lstAuthAction, List<DropdownTreeCheckboxModel> lstNewsStatus)
        {
            AuthGroup = authGroup;
            LstAuthAction = lstAuthAction;
            LstCategoryModel = lstCategoryModel;
            LstNewsStatus = lstNewsStatus;
        }
        public AuthGroup AuthGroup { get; set; }
        public List<DropdownTreeCheckboxModel> LstAuthAction { get; set; }
        public List<DropdownTreeCheckboxModel> LstCategoryModel { get; set; }
        public List<DropdownTreeCheckboxModel> LstNewsStatus { get; set; }
    }

    public class DropdownTreeCheckboxModel
    {
        public DropdownTreeCheckboxModel()
        {
            Children = new List<DropdownTreeCheckboxModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set;}
        public List<DropdownTreeCheckboxModel> Children { get; set; }
    }
    public enum AuthGroupStatusEnum
    {
        [Description("Không hoạt động")]
        Deactive = 0,
        [Description("Hoạt động")]
        Active = 1,
    }
}