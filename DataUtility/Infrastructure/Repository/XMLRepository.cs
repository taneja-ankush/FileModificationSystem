using DataUtility.CustomException;
using DataUtility.Infrastructure.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace DataUtility.Infrastructure.Repository
{
    public sealed class XMLRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {

        public object Context { get; private set; }

        private readonly string _filePath;

        private Random _random;

        private XElement _fileContext;

        List<string> errors = new List<string>();

        /// <summary>
        /// Gets the selector to de-serealize from XElement to TEntity
        /// </summary>
        private Func<XElement, TEntity> Selector
        {
            get
            {
                return x =>
                {

                    var entity = JsonConvert.DeserializeObject<TEntity>(
                    JObject.Parse(JsonConvert.SerializeXNode(x))[typeof(TEntity).Name.ToLower()].ToString(),
                    new JsonSerializerSettings()
                    {
                        Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                        {
                            errors.Add(args.ErrorContext.Error.Message);
                            args.ErrorContext.Handled = true;
                        }
                    }) ?? default(TEntity);

                    return entity;
                };
            }
        }

        /// <summary>
        /// Creates instance of Repository.
        /// </summary>
        /// <param name="file">XMLFile</param>
        public XMLRepository(string file)
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
                    _fileContext = XElement.Load(_filePath);
                }
                else
                {
                    _fileContext = new XElement(_filePath);
                }

                Context = _fileContext;
                _random = new Random();
            }
            catch (XmlException xmlException)
            {
                // Log exception
                throw new InvalidDataSourceException($"Invalid XML data exception. Error : {xmlException.Message}");
            }
            catch (Exception exception)
            {
                // Log and throw exception further or Handle the exception.
                throw exception;
            }
        }

        /// <summary>
        /// Add TEntity to XML file
        /// </summary>
        /// <param name="entity">TEntity</param>
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

            var type = entity.GetType();

            List<XElement> xelements = new List<XElement>();

            var propertyNames = type.GetProperties().Select(p => p.Name);
            if (propertyNames.Any())
            {
                foreach (var name in propertyNames)
                {

                    object propValue = entity.GetType().GetProperty(name).GetValue(entity, null);
                    if (propValue.GetType().IsClass && propValue.GetType() != typeof(string))
                    {
                        xelements.Add(GetXelements(propValue));
                    }
                    else
                    {
                        xelements.Add(new XElement(name.ToLower(), propValue));
                    }
                }
            }

            _fileContext.Add(new XElement(type.Name.ToLower(), xelements));

            _fileContext.Save(_filePath);
        }

        private XElement GetXelements(object entity)
        {
            XElement xElement;
            var xelements = new List<XElement>();
            var propertyNames = entity.GetType().GetProperties().Select(p => p.Name);
            if (propertyNames.Any())
            {
                foreach (var name in propertyNames)
                {
                    object propValue = entity.GetType().GetProperty(name).GetValue(entity, null);

                    if (propValue.GetType().IsClass && propValue.GetType() != typeof(string))
                    {
                        xelements.Add(GetXelements(propValue));
                    }
                    else
                    {
                        xelements.Add(new XElement(name.ToLower(), propValue));
                    }
                }
            }
            return xElement = new XElement(entity.GetType().Name.ToLower(), xelements); ;
        }

        /// <summary>
        /// Delete entity by Id.
        /// </summary>
        /// <param name="Id">Id</param>
        public void Delete(int Id)
        {
            var entities = from entity in _fileContext.Elements()
                           where (int)entity.Element("id") == Id
                           select entity;

            if (entities.Any())
            {
                entities.Remove();
                _fileContext.Save(_filePath);
            }
        }

        /// <summary>
        /// Get all TEnities by Id.
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>List of TEntity</returns>
        public IEnumerable<TEntity> Get(int Id)
        {
            var entities = from entity in _fileContext.Elements()
                           where Convert.ToInt32(entity.Element("id").Value) == Id
                           select entity;

            var entityList = entities.Select(Selector);

            if (entityList.Count() > 0 && errors.Count > 0)
            {
                // Throw exception
                throw new JsonReaderException(string.Join(";", errors));
            }

            return entityList;
        }

        /// <summary>
        /// GetAll TEntities
        /// </summary>
        /// <returns>List of TEntity</returns>
        public IEnumerable<TEntity> GetAll()
        {
            var entityList = _fileContext.Elements().Select(Selector);

            if (entityList.Count() > 0 && errors.Count > 0)
            {
                // Log error or throw error.
                Console.WriteLine($"Errors - {string.Join(";", errors)}");
            }

            return entityList;
        }

        /// <summary>
        /// Update provided entity.
        /// </summary>
        /// <param name="entity">TEntity</param>
        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Delete(entity.Id);
            Add(entity);
        }
    }
}
