using System.ComponentModel;

namespace DVG.WIS.Core.Enums
{
	public enum TopicStatus
	{
		[Description("Hoạt động")]
		Active = 1,
		[Description("Đã xóa")]
		Deleted = 2,
		[Description("Đã khóa")]
		Lock = 4
	}

    public enum TopicTypeEnum
    {

    }

    public enum TopicPositionEnum
    {
        [Description("Bình thường")]
        Normal = 0,
        [Description("Nổi bật")]
        Highlight = 1,
    }
}
