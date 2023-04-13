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
    public interface IAuthGroupService
    {
        void Add(Entities.AuthGroup post);

        void Update(Entities.AuthGroup post);

        void Delete(int id);

        IEnumerable<Entities.AuthGroup> GetAll();

        IEnumerable<Entities.AuthGroup> GetAllPaging(int page, int pageSize, out int totalRow);

        IEnumerable<Entities.AuthGroup> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow);

        Entities.AuthGroup GetById(int id);
        void SaveChanges();
    }
    public class AuthGroupService : IAuthGroupService
    {
        IAuthGroupRepository _authGroupRepository;
        IUnitOfWork _unitOfWork;
        public AuthGroupService(IAuthGroupRepository authGroupRepository, IUnitOfWork unitOfWork)
        {
            _authGroupRepository = authGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Entities.AuthGroup post)
        {
            _authGroupRepository.Add(post);
        }

        public void Delete(int id)
        {
            _authGroupRepository.Delete(id);
        }

        public IEnumerable<Entities.AuthGroup> GetAll()
        {
            return _authGroupRepository.GetAll();
        }

        public IEnumerable<Entities.AuthGroup> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow)
        {
            return _authGroupRepository.GetMultiPaging(x => x.Status == 1, out totalRow, page, pageSize);
        }

        public IEnumerable<Entities.AuthGroup> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return _authGroupRepository.GetMultiPaging(x => x.Status == 1, out totalRow, page, pageSize);
        }

        public Entities.AuthGroup GetById(int id)
        {
            return _authGroupRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Entities.AuthGroup post)
        {
            _authGroupRepository.Update(post);
        }
    }
}
