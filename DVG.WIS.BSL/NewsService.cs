using DVG.WIS.DAL.Infrastructure;
using DVG.WIS.DAL.Repositories;
using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business
{
    public interface INewsService
    {
        void Add(Entities.News post);

        void Update(Entities.News post);

        void Delete(int id);

        IEnumerable<Entities.News> GetAll();

        IEnumerable<Entities.News> GetAllPaging(int page, int pageSize, out int totalRow);

        IEnumerable<Entities.News> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow);

        Entities.News GetById(int id);
        void SaveChanges();
    }
    public class NewsService : INewsService
    {
        INewsRepository _NewsRepository;
        IUnitOfWork _unitOfWork;
        public NewsService(INewsRepository NewsRepository, IUnitOfWork unitOfWork)
        {
            _NewsRepository = NewsRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Entities.News post)
        {
            _NewsRepository.Add(post);
        }

        public void Delete(int id)
        {
            _NewsRepository.Delete(id);
        }

        public IEnumerable<Entities.News> GetAll()
        {
            return _NewsRepository.GetAll();
        }

        public IEnumerable<Entities.News> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow)
        {
            return _NewsRepository.GetMultiPaging(x => x.CategoryId == categoryId, out totalRow, page, pageSize);
        }

        public IEnumerable<Entities.News> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return _NewsRepository.GetMultiPaging(x => x.Status == 1, out totalRow, page, pageSize);
        }

        public Entities.News GetById(int id)
        {
            return _NewsRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Entities.News post)
        {
            _NewsRepository.Update(post);
        }
    }
}
