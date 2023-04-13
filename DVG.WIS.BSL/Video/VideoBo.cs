using DVG.WIS.DAL.Video;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DVG.WIS.Business.Video
{
    public class VideoBo : IVideoBo
    {

        private IVideoDal _videoDal;

        public VideoBo(IVideoDal videoDal)
        {
            _videoDal = videoDal;
        }
        public ErrorCodes Delete(int id)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                int result = _videoDal.Delete(id);
                if (result < 1)
                {
                    return ErrorCodes.BusinessError;
                }
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, id);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }

        public Entities.Video GetById(int id)
        {
            try
            {
                return _videoDal.GetById(id);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }

        public IEnumerable<Entities.Video> GetList(string keyword, int pageIndex, int pageSize, int status, out int totalRows)
        {
            try
            {
                return _videoDal.GetList(keyword, pageIndex, pageSize, status, out totalRows);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                totalRows = 0;
                return null;
            }
        }

        public ErrorCodes Update(Entities.Video video)
        {
            ErrorCodes errorCode = ErrorCodes.Success;
            try
            {
                // Validate
                if (null != video && !string.IsNullOrEmpty(video.Title))
                {
                    if (!string.IsNullOrEmpty(video.VideoUrl))
                    {
                        video.VideoUrl = GetYouTubeId(video.VideoUrl);
                    }
                    WIS.Entities.Video videoObj = new WIS.Entities.Video();
                    if (video.Id > 0)
                    {
                        videoObj = _videoDal.GetById(video.Id);
                        videoObj.Title = video.Title;
                        videoObj.VideoUrl = video.VideoUrl;
                        videoObj.Status = video.Status;
                        videoObj.Link = video.Link;
                        videoObj.Avatar = video.Avatar;
                        video = videoObj;
                    }
                    // Insert/Update
                    int numberRecords = _videoDal.Update(video);
                }
            }
            catch (Exception ex)
            {
                errorCode = ErrorCodes.Exception;
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, category);
                Logger.WriteLog(Logger.LogType.Error, string.Format("{0} => {1}", video.Id, ex.ToString()));
            }
            return errorCode;
        }


        public static string GetYouTubeId(string url)
        {
            var regex = @"(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?|watch)\/|.*[?&amp;]v=)|youtu\.be\/)([^""&amp;?\/ ]{11})";

            var match = Regex.Match(url, regex);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return url;
        }

        public IEnumerable<Entities.Video> GetListVideoTop(int top)
        {
            try
            {
                return _videoDal.GetListVideoTop(top); 
            }
            catch(Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }
    }
}
