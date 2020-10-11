using DataUtility.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataUtility.Infrastructure.Repository
{
    public interface IRepository<TEntity>
        where TEntity : Entity
    {
        object Context { get; }

        /// <summary>
        /// Add TEntity to XML file
        /// </summary>
        /// <param name="entity">TEntity</param>
        void Add(TEntity entity);

        /// <summary>
        /// Get all TEnities by Id.
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>List of TEntity</returns>
        IEnumerable<TEntity> Get(int Id);

        /// <summary>
        /// GetAll TEntities
        /// </summary>
        /// <returns>List of TEntity</returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Delete entity by Id.
        /// </summary>
        /// <param name="Id">Id</param>
        void Delete(int Id);

        /// <summary>
        /// Update provided entity.
        /// </summary>
        /// <param name="entity">TEntity</param>
        void Update(TEntity entity);
    }
}
