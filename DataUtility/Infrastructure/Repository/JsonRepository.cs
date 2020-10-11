using DataUtility.CustomException;
using DataUtility.Infrastructure.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataUtility.Infrastructure.Repository
{
    public sealed class JsonRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {

        private readonly string _filePath;

        private Random _random;

        public object Context { get; private set; }

        public string _fileContext { get; set; }

        public JsonRepository(string file)
        {
            _filePath = file;
            Initialize();
        }

        private void Initialize()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    _fileContext = File.ReadAllText(_filePath);
                    if (!IsValidJson(_fileContext))
                    {
                        throw new InvalidDataSourceException($"Invalid Json data: {_fileContext}. FilePath : {_filePath}");
                    }
                }
                else
                {
                    File.WriteAllText(_filePath, JsonConvert.SerializeObject("[]", Formatting.Indented));
                }

                Context = _fileContext;
                _random = new Random();
            }
            catch (Exception exception)
            {
                // Log and throw exception further or Handle the exception.
                throw exception;
            }

        }

        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (entity.Id == 0)
            {
                entity.Id = _random.Next();
            }

            if (entity.CreatedDate == DateTime.MinValue || entity.CreatedDate == DateTime.MaxValue)
            {
                entity.CreatedDate = DateTime.Now;
            }

            IList<TEntity> output = GetAll().ToList();
            output.Add(entity);
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(output, Formatting.Indented));
        }

        public void Delete(int Id)
        {
            IList<TEntity> output = GetAll().ToList();
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(output.Where(x => x.Id != Id), Formatting.Indented));
        }

        public IEnumerable<TEntity> Get(int Id)
        {
            return GetAll().Where(obj => obj.Id == Id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            _fileContext = File.ReadAllText(_filePath);
            Context = _fileContext;
            return JsonConvert.DeserializeObject<IEnumerable<TEntity>>(_fileContext);
        }

        public void Update(TEntity entity)
        {
            var allEntities = GetAll();
            if (!allEntities.Any())
            {
                return;
            }

            var existingEntities = allEntities.Where(x => x.Id == entity.Id);
            if (!existingEntities.Any())
            {
                return;
            }

            var removedExistingEntiesList = allEntities.Except(existingEntities).ToList();
            removedExistingEntiesList.Add(entity);
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(removedExistingEntiesList, Formatting.Indented));
        }

        private bool IsValidJson(string data)
        {
            data = data.Trim();
            try
            {
                if (data.StartsWith("{") && data.EndsWith("}"))
                {
                    return false;
                }
                else if (data.StartsWith("[") && data.EndsWith("]"))
                {
                    JArray.Parse(data);
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
