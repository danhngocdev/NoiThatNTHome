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
    public interface IAuthActionService
    {
        void Add(Entities.AuthAction post);

        void Update(Entities.AuthAction post);

        void Delete(int id);

        IEnumerable<Entities.AuthAction> GetAll();

        IEnumerable<Entities.AuthAction> GetAllPaging(int page, int pageSize, out int totalRow);

        IEnumerable<Entities.AuthAction> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow);

        Entities.AuthAction GetById(int id);
        void SaveChanges();
    }
    public class AuthActionService : IAuthActionService
    {
        IAuthActionRepository _authActionRepository;
        IUnitOfWork _unitOfWork;
        public AuthActionService(IAuthActionRepository authActionRepository, IUnitOfWork unitOfWork)
        {
            _authActionRepository = authActionRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Entities.AuthAction post)
        {
            _authActionRepository.Add(post);
        }

        public void Delete(int id)
        {
            _authActionRepository.Delete(id);
        }

        public IEnumerable<Entities.AuthAction> GetAll()
        {
            return _authActionRepository.GetAll();
        }

        public IEnumerable<Entities.AuthAction> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow)
        {
            return _authActionRepository.GetMultiPaging(x => x.Status == 1, out totalRow, page, pageSize);
        }

        public IEnumerable<Entities.AuthAction> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return _authActionRepository.GetMultiPaging(x => x.Status == 1, out totalRow, page, pageSize);
        }

        public Entities.AuthAction GetById(int id)
        {
            return _authActionRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Entities.AuthAction post)
        {
            _authActionRepository.Update(post);
        }
    }
}
